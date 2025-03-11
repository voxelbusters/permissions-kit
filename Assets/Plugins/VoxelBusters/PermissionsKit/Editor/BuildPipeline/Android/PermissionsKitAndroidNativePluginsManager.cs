#if UNITY_EDITOR && UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android;

namespace VoxelBusters.PermissionsKit.Editor.Build.Android
{
    public class PermissionsKitAndroidNativePluginsManager : AndroidNativePluginsManager
    {
        protected override bool IsSupported(BuildTarget target)
        {
            return true; //@@ Later if there is a way to detect Google Play store / Amazon Store, consider adding it here to return true for that store only.
        }
    }
}
#endif