#if UNITY_EDITOR && UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build;
using System;


namespace VoxelBusters.PermissionsKit.Editor.Build.Android
{
    public class PermissionsKitAndroidNativePluginsProcessor : VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android.AndroidNativePluginsProcessor
    {
#region Properties

        private PermissionsKitSettings Settings { get; set; }

#endregion

#region Overriden methods

        public override void OnUpdateLinkXml(LinkXmlWriter writer)
        {
            if (!EnsureInitialized())
                return;

            // Add active configurations
            var platform = EditorApplicationUtility.ConvertBuildTargetToRuntimePlatform(BuildTarget);
            writer.AddConfiguration("Permissions Kit", ImplementationSchema.Default, platform, useFallbackPackage: false);
        }

#endregion

#region Helper methods

        private bool EnsureInitialized()
        {
            if (Settings != null) return true;

            if (PermissionsKitSettingsEditorUtility.TryGetDefaultSettings(out PermissionsKitSettings settings))
            {
                Settings = settings;
                return true;
            }

            return false;
        }
#endregion
    }
}
#endif