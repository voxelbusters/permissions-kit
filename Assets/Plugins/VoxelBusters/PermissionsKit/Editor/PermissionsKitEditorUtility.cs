using UnityEditor;
using UnityEngine;


namespace VoxelBusters.PermissionsKit.Editor
{
    public class PermissionsKitEditorUtility
    {
        #region Constants
        
        private     const   string      kProductPage                            = "https://link.voxelbusters.com/permissions-kit";

        private     const   string      kPublisherPage                          = "https://link.voxelbusters.com/asset-store-publisher";

        private     const   string      kApiReferencePage                       = "https://link.voxelbusters.com/permissions-kit-api";

        private     const   string      kForumPage                              = "https://link.voxelbusters.com/permissions-kit-unity-forum";

        private     const   string      kTutorialPage                           = "https://link.voxelbusters.com/permissions-kit-tutorials";

        private     const   string      kDiscordPage                            = "https://link.voxelbusters.com/permissions-kit-support";
        
        #endregion

        #region Public methods

        #endregion

        #region Web action methods

        public static void OpenAssetStorePage()
        {
            Application.OpenURL(kProductPage);
        }

        public static void OpenPublisherPage()
        {
            Application.OpenURL(kPublisherPage);
        }

        public static void OpenDocumentation()
        {
            Application.OpenURL(kApiReferencePage);
        }

        public static void OpenForum()
        {
            Application.OpenURL(kForumPage);
        }

        public static void OpenTutorials()
        {
            Application.OpenURL(kTutorialPage);
        }

        public static void OpenSupport()
        {
            Application.OpenURL(kDiscordPage);
        }

        #endregion

        #region Private methods

        private static void RegisterForImportPackageCallbacks()
        {
            AssetDatabase.importPackageStarted     += OnImportPackageStarted;
            AssetDatabase.importPackageCompleted   += OnImportPackageCompleted;
            AssetDatabase.importPackageCancelled   += OnImportPackageCancelled;
            AssetDatabase.importPackageFailed      += OnImportPackageFailed;
        }

        private static void UnregisterFromImportPackageCallbacks()
        {
            AssetDatabase.importPackageStarted     -= OnImportPackageStarted;
            AssetDatabase.importPackageCompleted   -= OnImportPackageCompleted;
            AssetDatabase.importPackageCancelled   -= OnImportPackageCancelled;
            AssetDatabase.importPackageFailed      -= OnImportPackageFailed;
        }

        private static void ImportPackages(string[] packages)
        {
            RegisterForImportPackageCallbacks();
            foreach (var package in packages)
            {
                AssetDatabase.ImportPackage(package, false);
            }
            UnregisterFromImportPackageCallbacks();
        }
        
        #endregion

        #region Callback methods

        private static void OnImportPackageStarted(string packageName)
        {
            Debug.Log($"[UnityPackageUtility] Started importing package: {packageName}");
        }

        private static void OnImportPackageCompleted(string packageName)
        {
            Debug.Log($"[UnityPackageUtility] Imported package: {packageName}");
        }

        private static void OnImportPackageCancelled(string packageName)
        {
            Debug.Log($"[UnityPackageUtility] Cancelled the import of package: {packageName}");
        }

        private static void OnImportPackageFailed(string packageName, string errorMessage)
        {
            Debug.Log($"[UnityPackageUtility] Failed importing package: {packageName} with error: {errorMessage}");
        }

        #endregion
    }
}