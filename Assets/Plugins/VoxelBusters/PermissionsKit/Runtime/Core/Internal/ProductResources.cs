using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.PermissionsKit
{
    public static class ProductResources
    {
        #region Constants

        private     const   string      kResourcePage                        = "https://assetstore.permissionskit.voxelbusters.com/";
        
        #endregion

        #region Public static methods

        public static void OpenResourcePage()
        {
            Application.OpenURL(kResourcePage);
        }

        #endregion
    }
}