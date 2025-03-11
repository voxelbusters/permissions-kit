#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
namespace VoxelBusters.PermissionsKit.Core.Android
{
    public class NativeError : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

        private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Constructor

        // Default constructor
        public NativeError(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeError(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }
        public NativeError(int code, string description) : base(Native.kClassName ,(object)code, (object)description)
        {
        }
        public NativeError(string description) : base(Native.kClassName ,(object)description)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeError()
        {
            DebugLogger.Log("Disposing NativeError");
        }
#endif
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

        public int GetCode()
        {
            return Call<int>(Native.Method.kGetCode);
        }
        public string GetDescription()
        {
            return Call<string>(Native.Method.kGetDescription);
        }
        public override string ToString()
        {
            return Call<string>(Native.Method.kToString);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.permissionskit.common.Error";

            internal class Method
            {
                internal const string kToString = "toString";
                internal const string kGetCode = "getCode";
                internal const string kGetDescription = "getDescription";
            }

        }
    }
}
#endif