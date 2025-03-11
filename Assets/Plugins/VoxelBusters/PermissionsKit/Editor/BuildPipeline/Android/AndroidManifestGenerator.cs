#if UNITY_ANDROID
using System.Xml;
using UnityEditor;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android;
using AndroidPermission = VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android.Permission;

namespace VoxelBusters.PermissionsKit.Editor.Build.Android
{
    public class AndroidManifestGenerator
    {
#region Static fields

        private static string s_androidLibraryRootPackageName = "com.voxelbusters.permissionskit";

#endregion

#region Public methods

        public static void GenerateManifest(PermissionsKitSettings settings, string savePath = null)
        {
            Manifest manifest = new Manifest();
            manifest.AddAttribute("xmlns:android", "http://schemas.android.com/apk/res/android");
            manifest.AddAttribute("xmlns:tools", "http://schemas.android.com/tools");
            manifest.AddAttribute("package", s_androidLibraryRootPackageName + "plugin");
            manifest.AddAttribute("android:versionCode", "1");
            manifest.AddAttribute("android:versionName", "1.0");
            
            Application application = new Application();
            manifest.Add(application);
            AddPermissions(manifest, settings);
            AddFeatures(manifest, settings);


            XmlDocument xmlDocument = new XmlDocument();
            XmlNode xmlNode = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

            // Append xml node
            xmlDocument.AppendChild(xmlNode);

            // Get xml hierarchy
            XmlElement element = manifest.GenerateXml(xmlDocument);
            xmlDocument.AppendChild(element);

            // Save to androidmanifest.xml
            xmlDocument.Save(savePath == null ? IOServices.CombinePath(PermissionsKitPackageLayout.AndroidProjectPath, "AndroidManifest.xml") : savePath);
        }

#endregion

#region Private methods

        private static void AddPermissions(Manifest manifest, PermissionsKitSettings settings)
        {
            AndroidPermission permission = new AndroidPermission();

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.ACCESS_INTERNET).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.INTERNET");
                manifest.Add(permission);
            }

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.ACCESS_NETWORK_STATE).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.ACCESS_NETWORK_STATE");
                manifest.Add(permission);
            }

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.ACCESS_WIFI_STATE).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.ACCESS_WIFI_STATE");
                manifest.Add(permission);
            }

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.ACCESS_FINE_LOCATION).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.ACCESS_FINE_LOCATION");
                manifest.Add(permission);
            }

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.ACCESS_COARSE_LOCATION).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.ACCESS_COARSE_LOCATION");
                manifest.Add(permission);
            }

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.ACCESS_LOCATION_IN_BACKGROUND).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.ACCESS_LOCATION_IN_BACKGROUND");
                manifest.Add(permission);
            }

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.PUSH_NOTIFICATIONS).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "com.google.android.c2dm.permission.RECEIVE");
                manifest.Add(permission);
            }

            bool needsReadStorage = false;
            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.READ_MEDIA_LIBRARY_IMAGES).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.READ_MEDIA_IMAGES");
                manifest.Add(permission);
                needsReadStorage = true;
            }

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.READ_MEDIA_LIBRARY_VIDEOS).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.READ_MEDIA_VIDEO");
                manifest.Add(permission);
                needsReadStorage = true;
            }

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.READ_MEDIA_LIBRARY_AUDIO).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.READ_MEDIA_AUDIO");
                manifest.Add(permission);
                needsReadStorage = true;
            }

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.WRITE_MEDIA_LIBRARY_CONTENT).IsEnabled || settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.ADD_MEDIA_LIBRARY_CONTENT).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.WRITE_EXTERNAL_STORAGE");
                permission.AddAttribute("android:maxSdkVersion", "28");
                manifest.Add(permission);
                needsReadStorage = true;
            }

            if (needsReadStorage)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.READ_EXTERNAL_STORAGE");
                permission.AddAttribute("android:maxSdkVersion", "32");
                manifest.Add(permission);
            }
            
            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.IN_APP_PURCHASES).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "com.android.vending.BILLING");
                manifest.Add(permission);
            }

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.USE_CAMERA).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.CAMERA");
                manifest.Add(permission);
            }

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.RECORD_AUDIO).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.RECORD_AUDIO");
                manifest.Add(permission);
            }

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.VIBRATE).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.VIBRATE");
                permission.AddAttribute("android:maxSdkVersion", "18");
                manifest.Add(permission);
            }

            if (settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.ACCESS_BLUETOOTH).IsEnabled)
            {
                permission = new AndroidPermission();
                permission.AddAttribute("android:name", "android.permission.BLUETOOTH");
                manifest.Add(permission);
            }
        }

        private static void AddFeatures(Manifest manifest, PermissionsKitSettings settings)
        {
               Feature feature;
               PermissionConfiguration config;
               
               if ((config = settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.USE_CAMERA)) != null && config.IsEnabled)
               {
                   feature = new Feature();
                   feature.AddAttribute("android:name", "android.hardware.camera");
                   manifest.Add(feature);
                   
                   feature = new Feature();
                   feature.AddAttribute("android:name", "android.hardware.camera.autofocus");
                   feature.AddAttribute("android:required", config.IsOptional ? "false" : "true");
                   manifest.Add(feature);
               }
               
               if ((config = settings.PermissionsConfiguration.GetPermissionConfiguration(Permission.ACCESS_BLUETOOTH)) != null && config.IsEnabled)
               {
                   feature = new Feature();
                   feature.AddAttribute("android:name", "android.hardware.bluetooth");
                   feature.AddAttribute("android:required", config.IsOptional ? "false" : "true");
                   manifest.Add(feature);
               }
        }

#endregion
    }
}
#endif