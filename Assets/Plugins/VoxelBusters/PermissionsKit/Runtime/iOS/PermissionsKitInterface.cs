#if UNITY_IOS || UNITY_TVOS
using AOT;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.PermissionsKit.Core.iOS
{
    public sealed class PermissionsKitInterface : NativeFeatureInterfaceBase, INativePermissionsKitInterface
    {
        #region Constructors

        public PermissionsKitInterface() 
            : base(isAvailable: true)
        {
        }

        #endregion

        #region INativePermissionsKitInterface implementation methods
        
        public void Request(PermissionRequest request, EventCallback<PermissionRequestResult> callback)
        {
            PermissionsKitBinding.NPPermissionsKitRequest(request.Permissions.Select(each => each.ToString()).ToArray(), request.Permissions.Length, request.PurposeDescription, 1, HandlePermissionRequestNativeCallback, MarshalUtility.GetIntPtr(callback));
        }
        public PermissionStatus GetStatus(Permission permission)
        {
            return (PermissionStatus)PermissionsKitBinding.NPPermissionsKitGetStatus(permission);
        }
        
        #endregion

        #region Native callback methods
        
        [MonoPInvokeCallback(typeof(PermissionRequestNativeCallback))]
        private static void HandlePermissionRequestNativeCallback(IntPtr permissionsStatusInfoPtr, int permissionsStatusInfoLength, NativeError error, IntPtr tagptr)
        {
            var     tagHandle       = GCHandle.FromIntPtr(tagptr);
            try
            {
                ((EventCallback<PermissionRequestResult>)tagHandle.Target).Invoke(ConvertToPermissionResult(permissionsStatusInfoPtr, permissionsStatusInfoLength), error.Convert(PermissionsKit.Domain));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(PermissionsKit.Domain, exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }
        #endregion

        #region Private methods


        private static PermissionRequestResult ConvertToPermissionResult(IntPtr permissionsStatusInfoPtr, int permissionsStatusInfoLength)
        {
            if(permissionsStatusInfoPtr == IntPtr.Zero)
            {
                return null;
            }
            
            List<Permission>        permissions         = new List<Permission>();
            List<PermissionStatus>  statuses            = new List<PermissionStatus>();
            int                     sizeOfInputObject   = Marshal.SizeOf(typeof(NativePermissionStatusInfo));

            for (int i = 0; i < permissionsStatusInfoLength; i++)
            {
                NativePermissionStatusInfo data = MarshalUtility.PtrToStructure<NativePermissionStatusInfo>(new IntPtr(permissionsStatusInfoPtr.ToInt64() + (i * sizeOfInputObject)));
                
                Debug.Log($"Permission : {data.Permission} Status : {data.Status}");
                
                permissions.Add(data.Permission);
                statuses.Add((PermissionStatus)data.Status);
            }

            return new PermissionRequestResult(permissions.ToArray(), statuses.ToArray());
        }

        #endregion
        
    }
}
#endif