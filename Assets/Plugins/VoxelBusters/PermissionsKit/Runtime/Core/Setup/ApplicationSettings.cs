using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.PermissionsKit
{
    [Serializable]
    public class ApplicationSettings11
    {
        #region Fields

        [SerializeField]
        private     DebugLogger.LogLevel                    m_logLevel;

        [SerializeField]
        [Tooltip("Stores the registration ids of this app.")]
        private     RuntimePlatformConstantSet              m_appStoreIds;

        [SerializeField]
        [Tooltip("Usage permission settings.")]
        private     NativeFeatureUsagePermissionSettings    m_usagePermissionSettings;

        #endregion

        #region Properties

        public DebugLogger.LogLevel LogLevel => m_logLevel;

        public NativeFeatureUsagePermissionSettings UsagePermissionSettings => m_usagePermissionSettings;

        #endregion

        #region Constructors

        public ApplicationSettings11(RuntimePlatformConstantSet appStoreIds = null,
            NativeFeatureUsagePermissionSettings usagePermissionSettings = null, DebugLogger.LogLevel logLevel = DebugLogger.LogLevel.Critical)
        {
            // set properties
            m_appStoreIds               = appStoreIds ?? new RuntimePlatformConstantSet();
            m_usagePermissionSettings   = usagePermissionSettings ?? new NativeFeatureUsagePermissionSettings();
            m_logLevel                  = logLevel;
        }

        #endregion

        #region Public methods

        public string GetAppStoreIdForPlatform(RuntimePlatform platform)
        {
            return m_appStoreIds.GetConstantForPlatform(platform);
        }

        public string GetAppStoreIdForActivePlatform()
        {
            return m_appStoreIds.GetConstantForActivePlatform();
        }

        public string GetAppStoreIdForActiveOrSimulationPlatform()
        {
            return m_appStoreIds.GetConstantForActiveOrSimulationPlatform();
        }

        #endregion
    }
}