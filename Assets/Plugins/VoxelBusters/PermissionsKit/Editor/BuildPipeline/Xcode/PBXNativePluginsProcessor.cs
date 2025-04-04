#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build;

namespace VoxelBusters.PermissionsKit.Editor.Build.Xcode
{
    public class PBXNativePluginsProcessor : CoreLibrary.Editor.NativePlugins.Build.Xcode.PBXNativePluginsProcessor
    {
#region Properties

        private const string kExporterName           = "PermissionsKit";
        private const string kExportersRootName      = kExporterName + "Root";
        private PermissionsKitSettings Settings { get; set; }

#endregion

        #region Base class methods

        public override void OnUpdateExporterObjects()
        {
            // Check whether plugin is configured
            if (!EnsureInitialised()) return;

            DebugLogger.Log(PermissionsKit.Domain, "Updating permissions kit's native plugins exporter settings.");

            foreach (var exporter in NativePluginsExporterObject.FindObjects<PBXNativePluginsExporterObject>(includeInactive: true))
            {
                if(!exporter.Group.Name.Equals(kExportersRootName))
                    continue;
                
                switch (exporter.name)
                {
                    case kExporterName:
                        exporter.IsEnabled = true;
                        SetupMacrosAndFrameworks(exporter);
                        SetupCapabilities(exporter);
                        break;

                    default:
                        if (!exporter.name.Equals("Base"))
                            DebugLogger.LogWarning("[Developer Check] : This feature is not handled : " + exporter.name);

                        break;
                }
                EditorUtility.SetDirty(exporter);
            }
        }
        public override void OnUpdateInfoPlist(PlistDocument doc)
        {
            // Check whether plugin is configured
            if (!EnsureInitialised()) return;

            // Add usage permissions
            var     rootDict    = doc.root;
            var     permissions = GetUsagePermissions();
            foreach (string key in permissions.Keys)
            {
                rootDict.SetString(key, permissions[key]);
            }
        }

#endregion

#region Private methods

        private bool EnsureInitialised()
        {
            if (Settings != null) return true;

            if (PermissionsKitSettingsEditorUtility.TryGetDefaultSettings(out PermissionsKitSettings settings))
            {
                Settings = settings;
                return true;
            }
            else
            {
                PermissionsKitSettingsEditorUtility.ShowSettingsNotFoundErrorDialog();
                return false;
            }
        }
        
        private void SetupMacrosAndFrameworks(PBXNativePluginsExporterObject exporter)
        {
            exporter.ClearMacros();
            exporter.ClearFrameworks();

            if (Settings.PermissionsConfiguration.IsAnyPermissionEnabled(Permission.ACCESS_FINE_LOCATION, Permission.ACCESS_COARSE_LOCATION, Permission.ACCESS_LOCATION_IN_BACKGROUND))
            {
                exporter.AddMacro(name: "PERMISSIONS_KIT_USES_LOCATION_FRAMEWORK", value: "1");
                AddFramework("CoreLocation");
            }
            
            if (Settings.PermissionsConfiguration.IsAnyPermissionEnabled(Permission.PUSH_NOTIFICATIONS))
            {
                exporter.AddMacro(name: "PERMISSIONS_KIT_USES_NOTIFICATIONS_FRAMEWORK", value: "1");    
                AddFramework("UserNotifications");
            }
            
            if (Settings.PermissionsConfiguration.IsAnyPermissionEnabled(Permission.USE_CAMERA, Permission.RECORD_AUDIO))
            {
                exporter.AddMacro(name: "PERMISSIONS_KIT_USES_AVFOUNDATION_FRAMEWORK", value: "1");    
                AddFramework("AVFoundation");
            }
            
            if (Settings.PermissionsConfiguration.IsAnyPermissionEnabled(Permission.READ_MEDIA_LIBRARY_IMAGES, Permission.READ_MEDIA_LIBRARY_VIDEOS, Permission.RECORD_AUDIO))
            {
                exporter.AddMacro(name: "PERMISSIONS_KIT_USES_PHOTOS_FRAMEWORK", value: "1");    
                AddFramework("Photos");
            }
            
            if (Settings.PermissionsConfiguration.IsAnyPermissionEnabled(Permission.ACCESS_BLUETOOTH))
            {
                exporter.AddMacro(name: "PERMISSIONS_KIT_USES_BLUETOOTH_FRAMEWORK", value: "1");    
                AddFramework("CoreBluetooth");
            }
            
            void AddFramework(string framework)
            {
                exporter.AddFramework(new PBXFramework($"{framework}.framework", PBXTargetMembership.UnityFramework, false));
            }
        }
        
        private void SetupCapabilities(PBXNativePluginsExporterObject exporter)
        {
            exporter.ClearCapabilities();
            if (Settings.PermissionsConfiguration.IsAnyPermissionEnabled(Permission.PUSH_NOTIFICATIONS))
            {
                exporter.AddCapability(PBXCapability.PushNotifications());
            }

            if (Settings.PermissionsConfiguration.IsAnyPermissionEnabled(Permission.IN_APP_PURCHASES))
            {
                exporter.AddCapability(PBXCapability.InAppPurchase());
            }
        }

        
        private Dictionary<string, string> GetUsagePermissions()
        {
            var requiredPermissionsDict = new Dictionary<string, string>();

            // Helper method to add the usage description if any of the specified permissions are enabled.
            void AddPermission(string key, params Permission[] permissions)
            {
                var config = Settings.PermissionsConfiguration.GetAnyPermissionConfiguration(permissions);
                if (config != null && config.IsEnabled)
                {
                    requiredPermissionsDict[key] = config.Description.GetDescriptionForActivePlatform();
                }
            }

            

            // iOS requires usage descriptions for these permissions:
            AddPermission("NSCameraUsageDescription", Permission.USE_CAMERA);
            AddPermission("NSMicrophoneUsageDescription", Permission.RECORD_AUDIO);

            // For photo library access, check both READ_MEDIA_LIBRARY_IMAGES and READ_MEDIA_LIBRARY_VIDEOS.
            AddPermission("NSPhotoLibraryUsageDescription", Permission.READ_MEDIA_LIBRARY_IMAGES, Permission.READ_MEDIA_LIBRARY_VIDEOS);

            AddPermission("NSPhotoLibraryAddUsageDescription", Permission.WRITE_MEDIA_LIBRARY_CONTENT);
            AddPermission("NSBluetoothAlwaysUsageDescription", Permission.ACCESS_BLUETOOTH);
            AddPermission("NSBluetoothPeripheralUsageDescription", Permission.ACCESS_BLUETOOTH);
            AddPermission("NSLocationWhenInUseUsageDescription", Permission.ACCESS_FINE_LOCATION, Permission.ACCESS_COARSE_LOCATION);
            AddPermission("NSLocationAlwaysAndWhenInUseUsageDescription", Permission.ACCESS_LOCATION_IN_BACKGROUND);
            AddPermission("NSLocationUsageDescription", Permission.ACCESS_COARSE_LOCATION);
            AddPermission("NSLocalNetworkUsageDescription", Permission.ACCESS_LOCAL_NETWORK);
            AddPermission("NSUserTrackingUsageDescription", Permission.PUSH_NOTIFICATIONS);

            // [Auto Handled by OS - No permission required] ACCESS_INTERNET, ACCESS_NETWORK_STATE, ACCESS_WIFI_STATE, VIBRATE, IN_APP_PURCHASES, READ_MEDIA_LIBRARY_AUDIO

            return requiredPermissionsDict;
        }

        #endregion
    }
}
#endif