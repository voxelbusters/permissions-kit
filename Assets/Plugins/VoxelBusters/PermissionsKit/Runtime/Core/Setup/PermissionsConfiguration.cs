using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
namespace VoxelBusters.PermissionsKit
{
    [System.Serializable]
    public class PermissionsConfiguration
    {
            [Header("Network & Connectivity")]
            [SerializeField]
            private PermissionConfiguration m_accessInternet                = new PermissionConfiguration(Permission.ACCESS_INTERNET, new NativeFeatureUsagePermissionDefinition("$productName access internet."));
            [SerializeField]
            private PermissionConfiguration m_accessNetworkState            = new PermissionConfiguration(Permission.ACCESS_NETWORK_STATE, new NativeFeatureUsagePermissionDefinition("$productName access network state."));
            [SerializeField]
            private PermissionConfiguration m_accessWifiState               = new PermissionConfiguration(Permission.ACCESS_WIFI_STATE, new NativeFeatureUsagePermissionDefinition("$productName access wifi state."));
            [SerializeField]
            private PermissionConfiguration m_accessLocalNetwork            = new PermissionConfiguration(Permission.ACCESS_LOCAL_NETWORK, new NativeFeatureUsagePermissionDefinition("$productName access local network."));
            
            [Header("Location")]
            [SerializeField]
            private PermissionConfiguration m_accessFineLocation            = new PermissionConfiguration(Permission.ACCESS_FINE_LOCATION, new NativeFeatureUsagePermissionDefinition("$productName access fine location."));
            [SerializeField]
            private PermissionConfiguration m_accessCoarseLocation          = new PermissionConfiguration(Permission.ACCESS_COARSE_LOCATION, new NativeFeatureUsagePermissionDefinition("$productName access coarse location."));
            [SerializeField]
            private PermissionConfiguration m_accessLocationInBackground    = new PermissionConfiguration(Permission.ACCESS_LOCATION_IN_BACKGROUND, new NativeFeatureUsagePermissionDefinition("$productName access location in background."));
            
            [Header("Notifications")]
            [SerializeField]
            private PermissionConfiguration m_pushNotifications             = new PermissionConfiguration(Permission.PUSH_NOTIFICATIONS, new NativeFeatureUsagePermissionDefinition("$productName uses push notifications."));
            
            [Header("Media Access")]           
            [SerializeField]
            private PermissionConfiguration m_readMediaLibraryImages        = new PermissionConfiguration(Permission.READ_MEDIA_LIBRARY_IMAGES, new NativeFeatureUsagePermissionDefinition("$productName reads images."));
            [SerializeField]
            private PermissionConfiguration m_readMediaLibraryVideos        = new PermissionConfiguration(Permission.READ_MEDIA_LIBRARY_VIDEOS, new NativeFeatureUsagePermissionDefinition("$productName reads videos."));
            [SerializeField]
            private PermissionConfiguration m_readMediaLibraryAudio         = new PermissionConfiguration(Permission.READ_MEDIA_LIBRARY_AUDIO, new NativeFeatureUsagePermissionDefinition("$productName reads audio files."));
            [SerializeField]
            private PermissionConfiguration m_addMediaLibraryContent        = new PermissionConfiguration(Permission.ADD_MEDIA_LIBRARY_CONTENT, new NativeFeatureUsagePermissionDefinition("$productName adds to media gallery."));
            [SerializeField]
            private PermissionConfiguration m_writeMediaLibraryContent      = new PermissionConfiguration(Permission.WRITE_MEDIA_LIBRARY_CONTENT, new NativeFeatureUsagePermissionDefinition("$productName writes to media gallery."));
            
            [Header("Commerce")]
            [SerializeField]
            private PermissionConfiguration m_inAppPurchases                = new PermissionConfiguration(Permission.IN_APP_PURCHASES, new NativeFeatureUsagePermissionDefinition("$productName uses in-app purchases."));
            
            [Header("Hardware")]
            [SerializeField]
            private PermissionConfiguration m_usesCamera                    = new PermissionConfiguration(Permission.USE_CAMERA, new NativeFeatureUsagePermissionDefinition("$productName uses camera."));
            [SerializeField]
            private PermissionConfiguration m_recordAudio                   = new PermissionConfiguration(Permission.RECORD_AUDIO, new NativeFeatureUsagePermissionDefinition("$productName records audio."));
            [SerializeField]
            private PermissionConfiguration m_vibrate                       = new PermissionConfiguration(Permission.VIBRATE, new NativeFeatureUsagePermissionDefinition("$productName uses vibration."));
            [SerializeField]
            private PermissionConfiguration m_accessBluetooth               = new PermissionConfiguration(Permission.ACCESS_BLUETOOTH, new NativeFeatureUsagePermissionDefinition("$productName access bluetooth."));
            
            private List<PermissionConfiguration> m_permissionList = new ();
            public PermissionsConfiguration()
            {
                m_permissionList.Add(m_accessInternet);
                m_permissionList.Add(m_accessNetworkState);
                m_permissionList.Add(m_accessWifiState);
                m_permissionList.Add(m_accessLocalNetwork);
                m_permissionList.Add(m_accessFineLocation);
                m_permissionList.Add(m_accessCoarseLocation);
                m_permissionList.Add(m_accessLocationInBackground);
                m_permissionList.Add(m_pushNotifications);
                m_permissionList.Add(m_readMediaLibraryImages);
                m_permissionList.Add(m_readMediaLibraryVideos);
                m_permissionList.Add(m_readMediaLibraryAudio);
                m_permissionList.Add(m_writeMediaLibraryContent);
                m_permissionList.Add(m_addMediaLibraryContent);
                m_permissionList.Add(m_inAppPurchases);
                m_permissionList.Add(m_usesCamera);
                m_permissionList.Add(m_recordAudio);
                m_permissionList.Add(m_vibrate);
                m_permissionList.Add(m_accessBluetooth);
            }
        
            public PermissionConfiguration GetPermissionConfiguration(Permission permission)
            {
                foreach (var permissionConfiguration in m_permissionList)
                {
                    if (permissionConfiguration.Permission.Equals(permission))
                    {
                        return permissionConfiguration;
                    }
                }

                DebugLogger.LogException(new Exception($"Inform developer of this plugin to add {permission} to configuration list."));
                return null;
            }
            
            public PermissionConfiguration GetAnyPermissionConfiguration(params Permission[] permissions)
            {
                foreach (var permission in permissions)
                {
                    var config = GetPermissionConfiguration(permission);
                    if (config != null)
                    {
                        return config; 
                    }
                }

                return null;
            }

            public bool IsAnyPermissionEnabled(params Permission[] permissions)
            {
                foreach (var permission in permissions)
                {
                    var config = GetPermissionConfiguration(permission);
                    if (config != null && config.IsEnabled)
                    {
                        return true; 
                    }
                }

                return false;
            }
    }
}