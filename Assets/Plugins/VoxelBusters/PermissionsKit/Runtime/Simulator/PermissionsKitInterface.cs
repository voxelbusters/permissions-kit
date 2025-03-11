using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.UnityUI;
using Object = UnityEngine.Object;

namespace VoxelBusters.PermissionsKit.Core.Simulator
{

    public sealed class PermissionsKitInterface : NativeFeatureInterfaceBase, INativePermissionsKitInterface
    {
        #region Constants

        private const string kPermissionsKey = "PermissionsKit.Permissions";

        #endregion
        
        #region Constructors

        public PermissionsKitInterface()
            : base(isAvailable: true)
        {
        }

        #endregion

        #region INativePermissionsKitInterface implementation methods
       
        public void Request(PermissionRequest request, EventCallback<PermissionRequestResult> callback)
        {
            var requestedPermissions            = request.Permissions;
            List<PermissionStatus> statusList   = new List<PermissionStatus>();

            Action<PermissionStatus> deferredStatusCallback = (PermissionStatus status) =>
            {
                statusList.Add(status);
                if (statusList.Count == requestedPermissions.Length)
                {
                    var result = new PermissionRequestResult(requestedPermissions, statusList.ToArray());
                    callback.Invoke(result, null);
                }
            };

            foreach (var permission in requestedPermissions)
            {
                var status = GetStatus(permission);
                var permissionConfiguration = PermissionsKit.UnitySettings.PermissionsConfiguration.GetPermissionConfiguration(permission);

                if (permissionConfiguration == null)
                {
                    callback.Invoke(null, PermissionsKitError.NoConfigurationAvailable(permission));
                    return;
                }

                if (!permissionConfiguration.IsEnabled)
                {
                    callback.Invoke(null, PermissionsKitError.NotEnabled(permission));
                    return;
                }

                if (status == PermissionStatus.Unknown)
                {
                    bool authorized = false;
                    #if UNITY_EDITOR
                    
                    authorized = EditorUtility.DisplayDialog("Permission Request",
                        permissionConfiguration.Description.GetDescriptionForActivePlatform(), "Authorize", "Deny");
                    
                    #endif
                    
                    SavePermissionStatus(permission, authorized ? PermissionStatus.Authorized : PermissionStatus.Denied);
                    deferredStatusCallback(GetStatus(permission));
                }
                else
                {
                    deferredStatusCallback(GetStatus(permission));
                }
            }
        }
        
        public PermissionStatus GetStatus(Permission permission)
        {
            IDictionary<string, object> savedPermissions = GetSavedPermissions();

            if (savedPermissions.TryGetValue(permission, out object savedPermission))
            {
                var status = Convert.ToInt32(savedPermission);
                return (PermissionStatus)status;
            }
            
            return PermissionStatus.Unknown;
        }
        
        #endregion

        #region Private methods
        private static IJsonServiceProvider GetJsonServiceProvider()
        {
            return ExternalServiceProvider.JsonServiceProvider;
        }
        
        private void SavePermissionStatus(Permission permission, PermissionStatus authorized)
        {
            var savedPermissions = GetSavedPermissions();

            savedPermissions[permission] = (int)authorized;
            PlayerPrefs.SetString(kPermissionsKey, GetJsonServiceProvider().ToJson(savedPermissions));
        }
        private static IDictionary<string, object> GetSavedPermissions()
        {

            IDictionary<string, object> savedPermissions;

            if (!PlayerPrefs.HasKey(kPermissionsKey))
            {
                savedPermissions = new Dictionary<string, object>();
            }
            else
            {
                savedPermissions = GetJsonServiceProvider().FromJson(PlayerPrefs.GetString(kPermissionsKey)) as IDictionary<string, object>;    
            }
            return savedPermissions;
        }

        #endregion
        
        #region Static methods
        
        public static void Reset()
        {
           PlayerPrefs.DeleteKey(kPermissionsKey);
        }

        #endregion
    }
}