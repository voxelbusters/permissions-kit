using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.PermissionsKit.Core
{
    internal class NullPermissionsKitInterface : NativeFeatureInterfaceBase, INativePermissionsKitInterface
    {
        #region Constructors

        public NullPermissionsKitInterface(): base(isAvailable: false)
        { }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported(PermissionsKit.Domain);
        }

        #endregion

        #region INativePermissionsKitInterface implementation methods
        
        public void Request(PermissionRequest request, EventCallback<PermissionRequestResult> callback)
        {
            LogNotSupported();
            
            List<PermissionStatus> statusList = new ();
            foreach (var permission in request.Permissions)
            {
                statusList.Add(PermissionStatus.Unknown);
            }
            
            callback(new PermissionRequestResult(request.Permissions, statusList.ToArray()), new Error(PermissionsKit.Domain, (int)PermissionsKitErrorCode.Unknown, "PermissionsKit is not available on this platform."));
        }
        public PermissionStatus GetStatus(Permission permission)
        {
            LogNotSupported();
            return PermissionStatus.Unknown;
        }
        
        #endregion
    }
}