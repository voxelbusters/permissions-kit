using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.UnityUI;
using Object = UnityEngine.Object;

namespace VoxelBusters.PermissionsKit
{
    public class PermissionsKitSettings : SettingsObject
    {
        #region Static fields

        [ClearOnReload]
        private     static      PermissionsKitSettings    s_sharedInstance;

        [ClearOnReload]
        private     static      UnityPackageDefinition  s_package;

        #endregion

        #region Fields
        
        [SerializeField]
        private     DebugLogger.LogLevel                    m_logLevel;

        [SerializeField]
        [Tooltip("Usage permission settings.")]
        private PermissionsConfiguration m_permissionsConfiguration = new PermissionsConfiguration();
        
        #endregion

        #region Properties

        public DebugLogger.LogLevel LogLevel => m_logLevel;

        public PermissionsConfiguration PermissionsConfiguration => m_permissionsConfiguration;

        #endregion

        #region Static properties

        internal static UnityPackageDefinition Package => ObjectHelper.CreateInstanceIfNull(
            ref s_package,
            () =>
            {
                return new UnityPackageDefinition(name: "com.voxelbusters.permissionskit",
                                                  displayName: "Permissions Kit",
                                                  version: "1.1.1",
                                                  defaultInstallPath: $"Assets/Plugins/VoxelBusters/PermissionsKit",
                                                  dependencies: CoreLibrarySettings.Package);
            });

        public static string PackageName => Package.Name;

        public static string DisplayName => Package.DisplayName;

        public static string Version => Package.Version;

        public static string DefaultSettingsAssetName => "PermissionsKitSettings";

        public static string DefaultSettingsAssetPath => $"{Package.GetMutableResourcesPath()}/{DefaultSettingsAssetName}.asset";

        public static string PersistentDataPath => Package.PersistentDataPath;

        public static PermissionsKitSettings Instance => GetSharedInstanceInternal();
        
        #endregion
        #region Static methods

        public static void SetSettings(PermissionsKitSettings settings)
        {
            Assert.IsArgNotNull(settings, nameof(settings));

            // set properties
            s_sharedInstance    = settings;
        }

        private static PermissionsKitSettings GetSharedInstanceInternal(bool throwError = true)
        {
            if (null == s_sharedInstance)
            {
                // check whether we are accessing in edit or play mode
                var     assetPath   = DefaultSettingsAssetName;
                var     settings    = Resources.Load<PermissionsKitSettings>(assetPath);
                if (throwError && (null == settings))
                {
                    throw Diagnostics.PluginNotConfiguredException("Permissions Kit");
                }

                // store reference
                s_sharedInstance = settings;
            }

            return s_sharedInstance;
        }

        #endregion

        #region Base class methods

        protected override void UpdateLoggerSettings()
        {
#if NATIVE_PLUGINS_DEBUG
            DebugLogger.SetLogLevel(m_logLevel, PermissionsKit.Domain, CoreLibraryDomain.NativePlugins, CoreLibraryDomain.Default, "VoxelBusters");
#else
            DebugLogger.SetLogLevel(m_logLevel, PermissionsKit.Domain);
#endif
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            UpdateLoggerSettings();
            SyncSettings();
        }

        #endregion

        #region Private methods
        private void SyncSettings()
        {
        }

        #endregion

        #region Public methods

        public void InitialiseFeatures()
        {
            PermissionsKit.Initialize(this);
        }

        #endregion
    }
}