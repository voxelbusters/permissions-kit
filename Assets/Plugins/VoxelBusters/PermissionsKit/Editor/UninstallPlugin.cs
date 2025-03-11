using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using VoxelBusters.CoreLibrary;
using VoxelBusters.PermissionsKit;

namespace VoxelBusters.PermissionsKit.Editor
{
	public class UninstallPlugin
	{
		#region Constants
	
		private const	string		kAlertTitle		= "Uninstall - Permissions Kit";
        
		private const	string		kAlertMessage	= "Backup before doing this step to preserve changes done in this plugin. This deletes files only related to Permissions Kit plugin. Do you want to proceed?";
		
		#endregion	
	
		#region Static methods
	
		public static void Uninstall()
		{
			bool	confirmationStatus	= EditorUtility.DisplayDialog(kAlertTitle, kAlertMessage, "Uninstall", "Cancel");
			if (!confirmationStatus) return;

			// delete all the files and folders belonging to the plugin
			var		pluginFolders		= new string[]
			{
				PermissionsKitSettings.Package.GetInstallPath(),
				//PermissionsKitSettings.DefaultSettingsAssetPath,
				PermissionsKitPackageLayout.ExtrasPath
			};
			foreach (string folder in pluginFolders)
			{
                string	absolutePath	= Application.dataPath + "/../" + folder;
                IOServices.DeleteFileOrDirectory(absolutePath);
                IOServices.DeleteFileOrDirectory(absolutePath + ".meta");
			}
			PermissionsKitSettingsEditorUtility.RemoveGlobalDefines();
			AssetDatabase.Refresh();
			EditorUtility.DisplayDialog("Permissions Kit",
				                        "Uninstall successful!", 
				                        "Ok");
		}
		
		#endregion
	}
}