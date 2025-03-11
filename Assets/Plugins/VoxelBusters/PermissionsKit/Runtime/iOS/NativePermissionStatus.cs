#if UNITY_IOS || UNITY_TVOS
namespace VoxelBusters.PermissionsKit.Core.iOS
{
    public enum NativePermissionStatus
    {
        Unknown,
        Authorized,
        Restricted,
        Denied
    }


}
#endif