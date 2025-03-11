using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.PermissionsKit.Core;

namespace VoxelBusters.PermissionsKit
{


    /** 
     * @defgroup PermissionsKit  Permissions Kit
     * @brief Provides cross-platform interface to schedule or continue tasks in background.
     */

    /// <summary>
    /// The PermissionsKit class provides cross-platform interface to schedule or continue tasks in background. This can be used for running tasks uninterruptedly.
    /// </summary>
    /// <description> 
    /// <para>
    /// iOS platform has limited support for background execution compared to Android platform.
    /// </para>
    /// </description>
    /// @ingroup PermissionsKit  
    public static class PermissionsKit
    {
        #region Static fields

        [ClearOnReload]
        private     static  INativePermissionsKitInterface    s_iNativeInterface    = null;

        #endregion

        #region Static properties

        public static string Domain => "VoxelBusters.PermissionsKit";

        /// <summary>
        ///  The settings used for initialization.
        /// </summary>
        public static PermissionsKitSettings UnitySettings { get; private set; }

        #endregion
        
        #region Static methods

        public static bool IsAvailable()
        {
            return (s_iNativeInterface != null) && s_iNativeInterface.IsAvailable;
        }

        /// <summary>
        /// [Optional] Initialize the Task Services module with the given settings. This call is optional and only need to be called if you have custom settings to initialize this feature.
        /// </summary>
        /// <param name="settings">The settings to be used for initialization.</param>
        /// <remarks>
        /// <para>
        /// The settings configure the behavior of the Task Services module.
        /// </para>
        /// </remarks>
        public static void Initialize(PermissionsKitSettings settings)
        {
            Assert.IsArgNotNull(settings, nameof(settings));

            // Set default properties
            UnitySettings                       = settings;

            // Configure interface
            s_iNativeInterface                   = NativeFeatureActivator.CreateInterface<INativePermissionsKitInterface>(ImplementationSchema.Default, true);
        }

        
        /// <summary>
        ///  Requests the permission. 
        /// </summary>
        /// \note This can call the callback quickly if the permission is already granted or no runtime permission prompt is required.
        /// <param name="request">PermissionRequest built with <see cref="PermissionRequest.Builder"/>    </param>
        /// <param name="callback">Callback to be invoked when the permission request is completed </param>
        public static void Request(PermissionRequest request, EventCallback<PermissionRequestResult> callback)
        {
            //Check here the permissions passed and alert if the options are not enabled in settings - in dev mode.
            s_iNativeInterface.Request(request, (result, error) =>
            {
                CallbackDispatcher.InvokeOnMainThread(callback, result, error);
            });
        }

        public static PermissionStatus GetStatus(Permission permission)
        {
            return s_iNativeInterface.GetStatus(permission);
        }
        
        #endregion
    }
}