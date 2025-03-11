using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build;

namespace VoxelBusters.PermissionsKit.Editor.Build
{
    public class UnsupportedPlatformNativePluginsProcessor : CoreLibrary.Editor.NativePlugins.Build.UnsupportedPlatformNativePluginsProcessor
    {
        #region Properties

        private PermissionsKitSettings Settings { get; set; }

        #endregion

        #region Base class methods

        public override void OnUpdateLinkXml(LinkXmlWriter writer)
        {
            // Check whether plugin is configured
            if (!EnsureInitialized()) return;

            // Add active configurations
            var     platform        = EditorApplicationUtility.ConvertBuildTargetToRuntimePlatform(BuildTarget);
            writer.AddConfiguration("PermissionsKit", ImplementationSchema.Default, platform, useFallbackPackage: false);
        }

        #endregion

        #region Private methods

        private bool EnsureInitialized()
        {
            if (Settings != null) return true;

            if (PermissionsKitSettingsEditorUtility.TryGetDefaultSettings(out PermissionsKitSettings settings))
            {
                Settings    = settings;
                return true;
            }

            return false;
        }

        #endregion
    }
}