using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.PermissionsKit
{
    [Serializable]
    public class NativeFeatureUsagePermissionSettings
    {
        #region Fields

        [SerializeField]
        [Tooltip("Usage description displayed prior to accessing address book.")]
        private     NativeFeatureUsagePermissionDefinition      m_addressBookUsagePermission;

        [SerializeField]
        [Tooltip("Usage description displayed prior to accessing camera.")]
        private     NativeFeatureUsagePermissionDefinition      m_cameraUsagePermission;

        [SerializeField]
        [Tooltip("Usage description displayed prior to accessing gallery.")]
        private     NativeFeatureUsagePermissionDefinition      m_galleryUsagePermission;
       
        [SerializeField]
        [Tooltip("Usage description displayed prior to saving files to gallery.")]
        private     NativeFeatureUsagePermissionDefinition      m_galleryWritePermission;

        [SerializeField]
        [Tooltip("Usage description displayed prior to accessing location information.")]
        private     NativeFeatureUsagePermissionDefinition      m_locationWhenInUsePermission;

        [SerializeField]
        [Tooltip("Usage description displayed prior to loading friends.")]
        private     NativeFeatureUsagePermissionDefinition      m_accessFriendsPermission;




        #endregion

        #region Properties

        public NativeFeatureUsagePermissionDefinition AddressBookUsagePermission => m_addressBookUsagePermission;

        public NativeFeatureUsagePermissionDefinition CameraUsagePermission => m_cameraUsagePermission;

        public NativeFeatureUsagePermissionDefinition GalleryUsagePermission => m_galleryUsagePermission;

        public NativeFeatureUsagePermissionDefinition GalleryWritePermission => m_galleryWritePermission;

        public NativeFeatureUsagePermissionDefinition LocationWhenInUsePermission => m_locationWhenInUsePermission;

        public NativeFeatureUsagePermissionDefinition AccessFriendsPermission => m_accessFriendsPermission;

        #endregion

        #region Constructors

        public NativeFeatureUsagePermissionSettings(NativeFeatureUsagePermissionDefinition addressBookUsagePermission = null, NativeFeatureUsagePermissionDefinition cameraUsagePermission = null,
            NativeFeatureUsagePermissionDefinition galleryUsagePermission = null, NativeFeatureUsagePermissionDefinition galleryWritePermission = null,
            NativeFeatureUsagePermissionDefinition locationWhenInUsePermission = null, NativeFeatureUsagePermissionDefinition accessFriendsPermission = null)
        {
            // set properties
            m_addressBookUsagePermission    = addressBookUsagePermission ?? new NativeFeatureUsagePermissionDefinition(description: "$productName uses contacts.");
            m_cameraUsagePermission         = cameraUsagePermission ?? new NativeFeatureUsagePermissionDefinition(description: "$productName uses camera.");
            m_galleryUsagePermission        = galleryUsagePermission ?? new NativeFeatureUsagePermissionDefinition(description: "$productName uses gallery.");
            m_galleryWritePermission        = galleryWritePermission ?? new NativeFeatureUsagePermissionDefinition(description: "$productName wants to write to gallery.");
            m_locationWhenInUsePermission   = locationWhenInUsePermission ?? new NativeFeatureUsagePermissionDefinition(description: "$productName would like to user your location.");
            m_accessFriendsPermission       = accessFriendsPermission ?? new NativeFeatureUsagePermissionDefinition(description: "$productName wants to access friends.");
        }

        #endregion
    }
}