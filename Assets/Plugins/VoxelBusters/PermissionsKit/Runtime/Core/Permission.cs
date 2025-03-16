using System;
namespace VoxelBusters.PermissionsKit
{
    public class Permission
    {
        //Network & Connectivity
        public static Permission ACCESS_INTERNET                = new Permission("ACCESS_INTERNET");
        public static Permission ACCESS_NETWORK_STATE           = new Permission("ACCESS_NETWORK_STATE");
        public static Permission ACCESS_WIFI_STATE              = new Permission("ACCESS_WIFI_STATE");
        public static Permission ACCESS_LOCAL_NETWORK           = new Permission("ACCESS_LOCAL_NETWORK");
        
        //Location
        public static Permission ACCESS_FINE_LOCATION           = new Permission("ACCESS_FINE_LOCATION");
        public static Permission ACCESS_COARSE_LOCATION         = new Permission("ACCESS_COARSE_LOCATION");
        public static Permission ACCESS_LOCATION_IN_BACKGROUND  = new Permission("ACCESS_LOCATION_IN_BACKGROUND");
        
        //Notifications
        public static Permission PUSH_NOTIFICATIONS             = new Permission("PUSH_NOTIFICATIONS");
        
        
        //Media (For Stored content)
        public static Permission READ_MEDIA_LIBRARY_IMAGES      = new Permission("READ_MEDIA_LIBRARY_IMAGES");
        public static Permission READ_MEDIA_LIBRARY_VIDEOS      = new Permission("READ_MEDIA_LIBRARY_VIDEOS");
        public static Permission READ_MEDIA_LIBRARY_AUDIO       = new Permission("READ_MEDIA_LIBRARY_AUDIO");
        
        public static Permission WRITE_MEDIA_LIBRARY_CONTENT    = new Permission("WRITE_MEDIA_LIBRARY_CONTENT");
        public static Permission ADD_MEDIA_LIBRARY_CONTENT      = new Permission("ADD_MEDIA_LIBRARY_CONTENT"); 
        
        
        //BILLING
        public static Permission IN_APP_PURCHASES               = new Permission("IN_APP_PURCHASES");
        
        //Hardware
        public static Permission USE_CAMERA                     = new Permission("USE_CAMERA");
        public static Permission RECORD_AUDIO                   = new Permission("RECORD_AUDIO");
        public static Permission VIBRATE                        = new Permission("VIBRATE");
        public static Permission ACCESS_BLUETOOTH               = new Permission("ACCESS_BLUETOOTH");
        
        public static readonly Permission[] AllPermissions = new Permission[] 
        { 
            ACCESS_INTERNET,
            ACCESS_NETWORK_STATE,
            ACCESS_WIFI_STATE,
            ACCESS_LOCAL_NETWORK,
            ACCESS_FINE_LOCATION,
            ACCESS_COARSE_LOCATION,
            ACCESS_LOCATION_IN_BACKGROUND,
            PUSH_NOTIFICATIONS,
            READ_MEDIA_LIBRARY_IMAGES,
            READ_MEDIA_LIBRARY_VIDEOS,
            READ_MEDIA_LIBRARY_AUDIO,
            ADD_MEDIA_LIBRARY_CONTENT,
            WRITE_MEDIA_LIBRARY_CONTENT,
            IN_APP_PURCHASES,
            USE_CAMERA,
            RECORD_AUDIO,
            VIBRATE,
            ACCESS_BLUETOOTH
        };
        
        private string Value { get; }
        private Permission(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
            }
            Value = value;
        }

        public override string ToString() => Value;

        // Implicit conversion from string
        public static implicit operator Permission(string value) => new Permission(value);

        // Implicit conversion to string
        public static implicit operator string(Permission permisison) => permisison.Value;
        public override bool Equals(object obj)
        {
            if (obj is Permission other)
            {
                return string.Equals(Value, other.Value, StringComparison.Ordinal);
            }

            throw new AggregateException("Passed object for comparision is not of type Permission.");
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}