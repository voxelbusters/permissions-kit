using System;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;
namespace VoxelBusters.PermissionsKit
{
    public class PermissionRequest
    {
        public Permission[] Permissions { get; private set; }
        
        public string       PurposeDescription { get; private set; }
        
        public bool ShowApplicationSettingsIfDeniedOrRestricted { get; set; }
        
        private PermissionRequest(Permission[] permissions, string purposeDescription, bool showApplicationSettingsIfDeniedOrRestricted)
        {
            Permissions                                 = permissions;
            PurposeDescription                          = purposeDescription;
            ShowApplicationSettingsIfDeniedOrRestricted = showApplicationSettingsIfDeniedOrRestricted;
        }

        public class Builder
        {
            private readonly List<Permission> m_permissions = new ();
            private string m_purposeDescription;
            private bool m_showApplicationSettingsIfDeniedOrRestricted = true;
            public Builder AddPermission(Permission permission)
            {
                m_permissions.Add(permission);
                return this;
            }

            public Builder SetPurposeDescription(string purposeDescription)
            {
                m_purposeDescription = purposeDescription;
                return this;
            }

            public Builder ShowApplicationSettingsIfDeniedOrRestricted(bool showSettings)
            {
                m_showApplicationSettingsIfDeniedOrRestricted = showSettings;
                return this;
            }

            public PermissionRequest Build()
            {
                if (m_permissions.Count == 0)
                {
                    DebugLogger.LogException(PermissionsKit.Domain, new Exception("There should be at-least one permission (from Permission class) added via AddPermission"));
                    return null;    
                }

                if (string.IsNullOrEmpty(m_purposeDescription))
                {
                    DebugLogger.LogException(PermissionsKit.Domain, new Exception("You need to set purpose description for which this permission is requested. When a user denies the permission, this will be used to inform about the purpose to the user if required."));
                    return null;
                }

                return new PermissionRequest(m_permissions.ToArray(), m_purposeDescription, m_showApplicationSettingsIfDeniedOrRestricted);
            }
        }    
    }
}