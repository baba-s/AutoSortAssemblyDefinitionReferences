using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Kogane.Internal
{
    [FilePath( "ProjectSettings/Kogane/AutoSortAssemblyDefinitionReferences.asset", FilePathAttribute.Location.ProjectFolder )]
    internal sealed class AutoSortAssemblyDefinitionReferencesSetting : ScriptableSingleton<AutoSortAssemblyDefinitionReferencesSetting>
    {
        [SerializeField] private Object[] m_excludeArray = Array.Empty<Object>();

        public IReadOnlyList<Object> ExcludeList => m_excludeArray;

        public void Save()
        {
            Save( true );
        }
    }
}