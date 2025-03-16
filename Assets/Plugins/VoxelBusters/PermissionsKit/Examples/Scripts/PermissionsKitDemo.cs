using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;


namespace VoxelBusters.PermissionsKit.Demo
{
	public class PermissionsKitDemo : DemoActionPanelBase<PermissionsKitDemoAction, PermissionsKitDemoActionType>
    {
        #region Fields
        
        [SerializeField]
        private Dropdown m_permissionsDropdown;
        
        private Permission m_currentSelectedPermission;
        
        #endregion
        

        #region Base methods

        protected override void Start()
        {
            SetupPermissionsOptions();
        }

        protected override void OnEnable()
        {
            m_permissionsDropdown.onValueChanged.AddListener(OnPermissionSelected);
        }

        protected override void OnDisable()
        {
            m_permissionsDropdown.onValueChanged.RemoveListener(OnPermissionSelected);
        }

        private void SetupPermissionsOptions()
        {

            var options = new List<Dropdown.OptionData>();
            foreach (var permission in Permission.AllPermissions)
            {
                options.Add(new Dropdown.OptionData(permission));
            }

            m_permissionsDropdown.options = options;
            OnPermissionSelected(0);
        }

        protected override void OnActionSelectInternal(PermissionsKitDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case PermissionsKitDemoActionType.Request:
                    PermissionRequest.Builder builder = new PermissionRequest.Builder();
                    builder.AddPermission(m_currentSelectedPermission);
                    
                    //You can add multiple permissions per permission request.
                    /*builder.AddPermission(Permission.ADD_MEDIA_LIBRARY_CONTENT);
                    builder.AddPermission(Permission.WRITE_MEDIA_LIBRARY_CONTENT);*/
                    
                    builder.SetPurposeDescription("Need camera and audio recording for video recording!");
                    builder.ShowApplicationSettingsIfDeniedOrRestricted(true);
                    PermissionRequest request = builder.Build();

                    Log("Requesting permissions..." + string.Join(", ", request.Permissions.Select(each => each)));
                    
                    PermissionsKit.Request(request, (result, error) =>
                    {
                        if (error == null)
                        {
                            if (result.Authorized(allowLimitedAccess : true)) //Considering limited access permissions also as authorized
                            {
                                Log("All Permission/Permissions are authorized.");
                            }
                            else
                            {
                                Log("One or more permissions are not authorized.");
                                foreach (var eachPermission in request.Permissions)
                                {
                                    Log("Permission: " + eachPermission + " -> " + result.GetStatus(eachPermission));
                                }
                            }
                        }
                        else
                        {
                            Log(error.Description);
                        }
                    });
                    break;
                case PermissionsKitDemoActionType.GetStatus:
                    PermissionStatus status = PermissionsKit.GetStatus(m_currentSelectedPermission);
                    Log($"Status of {m_currentSelectedPermission} permission is {status}");
                    break;
                case PermissionsKitDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage();
                    break;
                default:    
                    break;
            }
        }
        
        #endregion

        #region Private methods

        private void OnPermissionSelected(int change)
        {
            m_currentSelectedPermission = Permission.AllPermissions[change];
        }
        
        #endregion
	}

}