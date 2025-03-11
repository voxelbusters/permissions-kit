#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.PermissionsKit.Core.Android
{
    public class NativePermissionRequestBridgeListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnFailureDelegate(NativeError errorInfo);
        public delegate void OnSuccessDelegate(string[] permissions, int[] statuses);

        #endregion

        #region Public callbacks

        public OnFailureDelegate  onFailureCallback;
        public OnSuccessDelegate  onSuccessCallback;

        #endregion


        #region Constructors

        public NativePermissionRequestBridgeListener() : base("com.voxelbusters.permissionskit.IPermissionRequestBridgeListener")
        {
        }

        #endregion


        #region Public methods
#if NATIVE_PLUGINS_DEBUG_ENABLED
        public override AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
        {
            DebugLogger.Log("**************************************************");
            DebugLogger.Log("[Generic Invoke : " +  methodName + "]" + " Args Length : " + (javaArgs != null ? javaArgs.Length : 0));
            if(javaArgs != null)
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();

                foreach(AndroidJavaObject each in javaArgs)
                {
                    if(each != null)
                    {
                        builder.Append(string.Format("[Type : {0} Value : {1}]", each.Call<AndroidJavaObject>("getClass").Call<string>("getName"), each.Call<string>("toString")));
                        builder.Append("\n");
                    }
                    else
                    {
                        builder.Append("[Value : null]");
                        builder.Append("\n");
                    }
                }

                DebugLogger.Log(builder.ToString());
            }
            DebugLogger.Log("-----------------------------------------------------");
            return base.Invoke(methodName, javaArgs);
        }
#endif

        public void onFailure(AndroidJavaObject errorInfo)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onFailure"  + " " + "[" + "errorInfo" + " : " + errorInfo +"]");
#endif
            if(onFailureCallback != null)
            {
                onFailureCallback(new NativeError(errorInfo));
            }
        }
        public void onSuccess(string[] permissions, int[] statuses)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onSuccess"  + " " + "[" + "permissions" + " : " + permissions +"]" + " " + "[" + "statuses" + " : " + statuses +"]");
#endif
            if(onSuccessCallback != null)
            {
                onSuccessCallback(permissions, statuses);
            }
        }

        #endregion
    }
}
#endif