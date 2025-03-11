#if UNITY_IOS || UNITY_TVOS
using System.Runtime.InteropServices;
namespace VoxelBusters.PermissionsKit.Core.iOS
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativePermissionStatusInfo
    {
        #region Properties

        public string Permission
        {
            get;
            internal set;
        }

        public NativePermissionStatus Status
        {
            get;
            internal set;
        }
        
        #endregion
    }
}
#endif