using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.UnityUI;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.PermissionsKit.Editor
{
    [InitializeOnLoad]
    public static class PermissionsKitSettingsEditorUtility
    {
        #region Constants

        private     const       string                      kLocalPathInProjectSettings     = "Project/Voxel Busters/Permissions Kit";

        #endregion

        #region Static fields

        private     static      PermissionsKitSettings        s_defaultSettings;

        #endregion

        #region Static properties

        public static PermissionsKitSettings DefaultSettings
        {
            get
            {
                if (s_defaultSettings == null)
                {
                    var     instance    = LoadDefaultSettingsObject(throwError: false);
                    if (null == instance)
                    {
                        instance        = CreateDefaultSettingsObject();
                    }
                    s_defaultSettings   = instance;
                }
                return s_defaultSettings;
            }
            set
            {
                Assert.IsPropertyNotNull(value, nameof(value));

                // set new value
                s_defaultSettings       = value;
            }
        }

        public static bool SettingsExists
        {
            get
            {
                if (s_defaultSettings == null)
                {
                    s_defaultSettings   = LoadDefaultSettingsObject(throwError: false);
                }
                return (s_defaultSettings != null);
            }
        }

        #endregion

        #region Constructors

        static PermissionsKitSettingsEditorUtility()
        {
            AddGlobalDefines();
        }

        #endregion

        #region Static methods

        public static void ShowSettingsNotFoundErrorDialog()
        {
            EditorUtility.DisplayDialog(
                title: "Error",
                message: "Permissions Kit plugin is not configured. Please select plugin settings file from menu and configure it according to your preference.",
                ok: "Ok");
        }

        public static bool TryGetDefaultSettings(out PermissionsKitSettings defaultSettings)
        {
            // Set default value
            defaultSettings     = null;

            // Set reference if the object exists
            if (SettingsExists)
            {
                defaultSettings = DefaultSettings;
                return true;
            }
            return false;
        }

        public static void AddGlobalDefines()
        {
            ScriptingDefinesManager.AddDefine("ENABLE_VOXELBUSTERS_PERMISSIONS_KIT");
        }
        
        public static void RemoveGlobalDefines()
        {
            ScriptingDefinesManager.RemoveDefine("ENABLE_VOXELBUSTERS_PERMISSIONS_KIT");
        }

        public static void OpenInProjectSettings()
        {
            if (!SettingsExists)
            {
                CreateDefaultSettingsObject();
            }
            Selection.activeObject  = null;
            SettingsService.OpenProjectSettings(kLocalPathInProjectSettings);
        }

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            return SettingsProviderZ.Create(
                settingsObject: DefaultSettings,
                path: kLocalPathInProjectSettings,
                scopes: SettingsScope.Project);
        }

        #endregion

        #region Private static methods

        private static PermissionsKitSettings CreateDefaultSettingsObject()
        {
            return AssetDatabaseUtility.CreateScriptableObject<PermissionsKitSettings>(
                assetPath: PermissionsKitSettings.DefaultSettingsAssetPath,
                onInit: (instance)  => SetDefaultProperties(instance));
        }

        private static PermissionsKitSettings LoadDefaultSettingsObject(bool throwError = true)
        {
            var     throwErrorFunc      = throwError ? () => Diagnostics.PluginNotConfiguredException() : (System.Func<System.Exception>)null;
            return AssetDatabaseUtility.LoadScriptableObject<PermissionsKitSettings>(
                assetPath: PermissionsKitSettings.DefaultSettingsAssetPath,
                onLoad: (instance)  => SetDefaultProperties(instance),
                throwErrorFunc: throwErrorFunc);
        }

        private static void SetDefaultProperties(PermissionsKitSettings settings)
        {
        }

        #endregion
    }
}