using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

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

            foreach ( var assetPath in assemblyDefinitionPathArray )
            {
                if ( excludeAssetPathArray.Contains( assetPath ) )
                {
                    continue;
                }

                var fullPath         = Path.GetFullPath( assetPath ).Replace( "\\", "/" );
                var currentDirectory = Directory.GetCurrentDirectory();
                var relativePath     = Path.GetRelativePath( currentDirectory, fullPath ).Replace( "\\", "/" );

                if ( !relativePath.StartsWith( "Assets/" ) &&
                     !relativePath.StartsWith( "Packages/" ) )
                {
                    continue;
                }

                var json          = File.ReadAllText( assetPath );
                var jsonData      = JsonUtility.FromJson<JsonAssemblyDefinition>( json );
                var oldReferences = jsonData.references;

                var newReferences = oldReferences
                        .Select( x => x.Replace( "GUID:", "" ) )
                        .Select( x => AssetDatabase.GUIDToAssetPath( x ) )
                        .OrderBy( x => Path.GetFileName( x ), new NaturalComparer() )
                        .Select( x => AssetDatabase.AssetPathToGUID( x ) )
                        .Select( x => "GUID:" + x )
                        .ToArray()
                    ;

                if ( newReferences.SequenceEqual( oldReferences ) )
                {
                    continue;
                }

                jsonData.references = newReferences;

                var newJson = JsonUtility.ToJson( jsonData, true );

                File.WriteAllText( assetPath, newJson, Encoding.UTF8 );

                var asset = AssetDatabase.LoadAssetAtPath<Object>( assetPath );

                EditorUtility.SetDirty( asset );
            }

            AssetDatabase.Refresh();
        }
    }
}