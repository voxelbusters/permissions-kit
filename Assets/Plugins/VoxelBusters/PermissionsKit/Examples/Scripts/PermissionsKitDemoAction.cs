using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.PermissionsKit.Demo
{
	public enum PermissionsKitDemoActionType
    {
        Request,
        GetStatus,
        ResourcePage
    }

    public class PermissionsKitDemoAction : DemoActionBehaviour<PermissionsKitDemoActionType> 
    {}
}