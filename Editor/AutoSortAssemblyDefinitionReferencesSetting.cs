﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Kogane.Internal
{
    [FilePath( "UserSettings/Kogane/AutoSortAssemblyDefinitionReferencesSetting.asset", FilePathAttribute.Location.ProjectFolder )]
    internal sealed class AutoSortAssemblyDefinitionReferencesSetting : ScriptableSingleton<AutoSortAssemblyDefinitionReferencesSetting>
    {
        [SerializeField] private bool     m_isEnable;
        [SerializeField] private Object[] m_excludeArray = Array.Empty<Object>();

        public bool                  IsEnable    => m_isEnable;
        public IReadOnlyList<Object> ExcludeList => m_excludeArray;

        public void Save()
        {
            Save( true );
        }
    }
}