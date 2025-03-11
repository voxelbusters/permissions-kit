using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

/// <summary> 
/// Namespace for permissions kit features. You need to import this namespace along with <see cref="VoxelBusters.CoreLibrary"/> to use permissions kit features.
/// </summary>
namespace VoxelBusters.PermissionsKit
{
    public class PermissionsKitManager : PrivateSingletonBehaviour<PermissionsKitManager>
    {
        #region Static methods

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnLoad()
        {
#pragma warning disable 
            var     singleton   = GetSingleton();
#pragma warning restore 
        }

        #endregion

        #region Unity methods

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();

            // Create required systems
            CallbackDispatcher.Initialize();
            ApplicationLifecycleObserver.Initialize();

            // Set environment variables
            var     settings    = PermissionsKitSettings.Instance;
            DebugLogger.SetLogLevel(settings.LogLevel,
                                    CoreLibraryDomain.Default,
                                    CoreLibraryDomain.NativePlugins,
                                    PermissionsKit.Domain);

            settings.InitialiseFeatures();
        }

        #endregion
    }
}