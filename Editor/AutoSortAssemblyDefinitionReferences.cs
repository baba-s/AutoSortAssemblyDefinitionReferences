using System.IO;
using System.Linq;
using UnityEditor;

namespace Kogane.Internal
{
    internal sealed class AutoSortAssemblyDefinitionReferences : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets
        (
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
        )
        {
            var assemblyDefinitionPathArray = importedAssets
                    .Where( x => x.EndsWith( ".asmdef" ) )
                    .ToArray()
                ;

            if ( assemblyDefinitionPathArray.Length <= 0 ) return;

            var excludeAssetPathArray = AutoSortAssemblyDefinitionReferencesSetting.instance.ExcludeList
                    .Select( x => AssetDatabase.GetAssetPath( x ) )
                    .ToArray()
                ;

            var currentDirectory = Directory
                    .GetCurrentDirectory()
                    .Replace( "\\", "/" )
                ;

            foreach ( var assetPath in assemblyDefinitionPathArray )
            {
                if ( excludeAssetPathArray.Contains( assetPath ) ) continue;

                var fullPath     = Path.GetFullPath( assetPath ).Replace( "\\", "/" );
                var relativePath = Path.GetRelativePath( currentDirectory, fullPath ).Replace( "\\", "/" );

                if ( !relativePath.StartsWith( "Assets/" ) &&
                     !relativePath.StartsWith( "Packages/" ) )
                {
                    continue;
                }

                AssemblyDefinitionReferencesSorter.Sort( assetPath );
            }

            AssetDatabase.Refresh();
        }
    }
}