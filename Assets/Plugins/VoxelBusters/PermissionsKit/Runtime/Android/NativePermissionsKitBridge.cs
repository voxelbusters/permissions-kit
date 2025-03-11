#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
namespace VoxelBusters.PermissionsKit.Core.Android
{
    public class NativePermissionsKitBridge : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion

        #region Constructor

        public NativePermissionsKitBridge(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
        {
        }

        #endregion
        #region Static methods
        private static AndroidJavaClass GetClass()
        {
            if (m_nativeClass == null)
            {
                m_nativeClass = new AndroidJavaClass(Native.kClassName);
            }
            return m_nativeClass;
        }
        #endregion
        #region Public methods

        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativePermissionsKitBridge][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public int GetStatus(string rootPermission)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativePermissionsKitBridge][Method : GetStatus]");
#endif
            return Call<int>(Native.Method.kGetStatus, rootPermission);
        }
        public void Request(string[] permissionsArray, string purposeDescription, int showApplicationSettingsIfDeniedOrRestricted, NativePermissionRequestBridgeListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativePermissionsKitBridge][Method : Request]");
#endif
            Call(Native.Method.kRequest, new object[] { permissionsArray, purposeDescription, showApplicationSettingsIfDeniedOrRestricted, listener } );
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.permissionskit.PermissionsKitBridge";

            internal class Method
            {
                internal const string kRequest = "request";
                internal const string kGetStatus = "getStatus";
                internal const string kGetFeatureName = "getFeatureName";
            }

        }
    }
}
#endif