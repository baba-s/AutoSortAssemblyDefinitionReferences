using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Kogane.Internal
{
    [FilePath( "ProjectSettings/Kogane/AutoSortAssemblyDefinitionReferences.asset", FilePathAttribute.Location.ProjectFolder )]
    internal sealed class AutoSortAssemblyDefinitionReferencesSetting : ScriptableSingleton<AutoSortAssemblyDefinitionReferencesSetting>
    {
        [SerializeField] private AssemblyDefinitionAsset[] m_excludeArray = Array.Empty<AssemblyDefinitionAsset>();

        public IReadOnlyList<AssemblyDefinitionAsset> ExcludeList => m_excludeArray;

        public void Save()
        {
            Save( true );
        }
    }
}