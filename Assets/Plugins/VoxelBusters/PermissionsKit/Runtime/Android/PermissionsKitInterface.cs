#if UNITY_ANDROID
using System.Linq;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.PermissionsKit.Core.Android
{
    public sealed class PermissionsKitInterface : NativeFeatureInterfaceBase, INativePermissionsKitInterface
    {
        #region Private fields
        
        private readonly NativePermissionsKitBridge m_instance;
        
        #endregion
        
        #region Constructors

        public PermissionsKitInterface() : base(isAvailable: true)
        {
            m_instance = new NativePermissionsKitBridge(NativeUnityPluginUtility.GetContext());
        }
        
        #endregion
        
        #region Public methods

        public void Request(PermissionRequest request, EventCallback<PermissionRequestResult> callback)
        {
            m_instance.Request(request.Permissions.Select(each => each.ToString()).ToArray(), request.PurposeDescription, request.ShowApplicationSettingsIfDeniedOrRestricted ? 1 : 0, new NativePermissionRequestBridgeListener()
            {
                onSuccessCallback = (permissions, statuses) =>
                {
                    var result = new PermissionRequestResult(permissions.Select(each => (Permission)each).ToArray(), statuses.Select(each => (PermissionStatus)each).ToArray());
                    callback.Invoke(result, null);
                },
                onFailureCallback = (error) =>
                {
                    callback.Invoke(null, new Error(PermissionsKit.Domain, error.GetCode(), error.GetDescription()));
                }
            });
        }
        public PermissionStatus GetStatus(Permission permission)
        {
            return (PermissionStatus)m_instance.GetStatus(permission);
        }
        
        #endregion
    }
}
#endif