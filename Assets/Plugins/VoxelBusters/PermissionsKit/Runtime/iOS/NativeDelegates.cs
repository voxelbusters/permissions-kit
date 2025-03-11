#if UNITY_IOS || UNITY_TVOS
using System;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.PermissionsKit.Core.iOS
{
    internal delegate void PermissionRequestNativeCallback(IntPtr permissionsStatusInfoPtr, int permissionsStatusInfoLength, NativeError error, IntPtr tagPtr);
}
#endif