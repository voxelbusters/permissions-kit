using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
namespace VoxelBusters.PermissionsKit
{
    [System.Serializable]
    public class PermissionConfiguration
    {
        [SerializeField]
        [ReadOnly]
        private string m_permission;
        
        [SerializeField]
        private bool m_isEnabled;

        [SerializeField]
        private bool m_isOptional;
        
        [SerializeField]
        private NativeFeatureUsagePermissionDefinition m_description;
        
        public Permission   Permission  => m_permission;
        public bool     IsEnabled   => m_isEnabled;
        
        public bool     IsOptional  => m_isOptional;

        public NativeFeatureUsagePermissionDefinition Description => m_description;
        
        public PermissionConfiguration(Permission permission, NativeFeatureUsagePermissionDefinition description)
        {
            m_permission    = permission;
            m_description   = description;
        }
    }
}