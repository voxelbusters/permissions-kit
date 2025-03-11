using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.PermissionsKit.Core
{
    public interface INativePermissionsKitInterface : INativeFeatureInterface
    {
        #region Methods

        void Request(PermissionRequest request, EventCallback<PermissionRequestResult> callback);
        PermissionStatus GetStatus(Permission permission);
        
        #endregion
        
    }

}