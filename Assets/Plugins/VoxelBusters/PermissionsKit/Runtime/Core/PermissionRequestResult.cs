using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoxelBusters.CoreLibrary;
namespace VoxelBusters.PermissionsKit.Core
{
    public class PermissionRequestResult
    {
        public IDictionary<Permission, PermissionStatus> StatusMap { get; }

        public PermissionRequestResult(Permission[] permissions, PermissionStatus[] statuses)
        {
            Dictionary<Permission, PermissionStatus> map = new Dictionary<Permission, PermissionStatus>();
            for(int i=0; i<permissions.Length; i++)
            {
                map.Add(permissions[i], statuses[i]);
            }

            StatusMap = map;
        }
        
        public bool Authorized(bool allowLimitedAccess = true)
        {
            foreach (var status in StatusMap.Values)
            {
                if (status != PermissionStatus.Authorized  && (allowLimitedAccess && status != PermissionStatus.Limited))
                    return false;
            }

            return true;
        }

        public string[] GetDenied()
        {
            List<string> deniedPermissions = new ();
            foreach (var each in StatusMap.Keys)
            {
                if (StatusMap[each] == PermissionStatus.Denied)
                    deniedPermissions.Add(each);
            }

            return deniedPermissions.ToArray();
        }

        public PermissionStatus GetStatus(Permission permission)
        {
            foreach (var each in StatusMap.Keys)
            {
                if (each.Equals(permission))
                {
                    return StatusMap[each];
                }
            }

            DebugLogger.LogException(new Exception($"Passed permission({permission}) is not available in the list of permissions you requested."));
            return PermissionStatus.Unknown;
        }
    }
}