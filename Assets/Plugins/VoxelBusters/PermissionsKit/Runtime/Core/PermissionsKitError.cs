using VoxelBusters.CoreLibrary;

namespace VoxelBusters.PermissionsKit
{
    public static class PermissionsKitError
    {

        public const string kDomain = "Permissions Kit";

        public static Error Unknown() => CreateError(PermissionsKitErrorCode.Unknown, "Unknown error");
        
        public static Error NoConfigurationAvailable(Permission permission) => CreateError(PermissionsKitErrorCode.NoConfigurationAvailable, $"Contact plugin publisher to include {permission} permission in configuration.");
        public static Error NotEnabled(Permission permission)  => CreateError(PermissionsKitErrorCode.NotEnabled, $"You need to enable the permission ({permission}) in the PermissionsKit settings to request this permission.");

        private static Error CreateError(PermissionsKitErrorCode code, string description)
        {
            return new Error(kDomain, (int)code, description);
        }
    }
}