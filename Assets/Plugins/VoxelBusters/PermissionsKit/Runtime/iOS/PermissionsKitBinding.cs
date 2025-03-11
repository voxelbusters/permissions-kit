#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.PermissionsKit.Core.iOS
{
    internal static class PermissionsKitBinding
    {
        [DllImport("__Internal")]
        public static extern void NPPermissionsKitRequest(string[] permissions, int length, string purposeDescription, int showApplicationSettingsIfDeniedOrRestricted, PermissionRequestNativeCallback callback, IntPtr tagPtr); 
        
        [DllImport("__Internal")]
        public static extern NativePermissionStatus NPPermissionsKitGetStatus(string permission); 

    }
}
#endif