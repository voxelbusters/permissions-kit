using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;

namespace VoxelBusters.PermissionsKit.Editor
{
    public static class PermissionsKitMenuManager
    {
        #region Constants

        private const string kMenuItemPath = "Window/Voxel Busters/Permissions Kit";

        #endregion

        #region Menu items

        [MenuItem(kMenuItemPath + "/Open Settings")]
        public static void OpenSettings()
        {
            PermissionsKitSettingsEditorUtility.OpenInProjectSettings();
        }

#if NATIVE_PLUGINS_SHOW_UPM_MIGRATION
        [MenuItem(kMenuItemPath + "/Migrate To UPM")]
        public static void MigrateToUpm()
        {
            PermissionsKitSettings.Package.MigrateToUpm();
        }

        [MenuItem(kMenuItemPath + "/Migrate To UPM", isValidateFunction: true)]
        private static bool ValidateMigrateToUpm()
        {
            return PermissionsKitSettings.Package.IsInstalledWithinAssets();
        }
#endif

        [MenuItem(kMenuItemPath + "/Uninstall")]
        public static void Uninstall()
        {
            UninstallPlugin.Uninstall();
        }

#endregion
    }
}