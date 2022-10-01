using System.IO;
using System.Linq;
using UnityEditor;

namespace Kogane.Internal
{
    internal sealed class AutoSortAssemblyDefinitionReferences : AssetPostprocessor
    {
        private static string[] m_excludeAssetPathArray;
        private static string   m_currentDirectory;

        private static void OnPostprocessAllAssets
        (
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
        )
        {
            const string extension = ".asmdef";

            if ( !importedAssets.Any( x => x.EndsWith( extension ) ) ) return;

            var assemblyDefinitionPathArray = importedAssets
                    .Where( x => x.EndsWith( extension ) )
                    .Where( x => !IsExclude( x ) )
                    .Where( x => IsAssetsOrPackages( x ) )
                    .ToArray()
                ;

            m_excludeAssetPathArray = null;
            m_currentDirectory      = null;

            if ( assemblyDefinitionPathArray.Length <= 0 ) return;

            foreach ( var assetPath in assemblyDefinitionPathArray )
            {
                AssemblyDefinitionReferencesSorter.Sort( assetPath );
            }

            AssetDatabase.Refresh();
        }

        private static bool IsExclude( string assetPath )
        {
            m_excludeAssetPathArray ??= AutoSortAssemblyDefinitionReferencesSetting.instance.ExcludeList
                    .Select( x => AssetDatabase.GetAssetPath( x ) )
                    .ToArray()
                ;

            return m_excludeAssetPathArray.Any( x => assetPath.StartsWith( x ) );
        }

        private static bool IsAssetsOrPackages( string assetPath )
        {
            m_currentDirectory ??= Directory
                    .GetCurrentDirectory()
                    .Replace( "\\", "/" )
                ;

            var fullPath     = Path.GetFullPath( assetPath ).Replace( "\\", "/" );
            var relativePath = Path.GetRelativePath( m_currentDirectory, fullPath ).Replace( "\\", "/" );

            return relativePath.StartsWith( "Assets/" ) ||
                   relativePath.StartsWith( "Packages/" )
                ;
        }
    }
}