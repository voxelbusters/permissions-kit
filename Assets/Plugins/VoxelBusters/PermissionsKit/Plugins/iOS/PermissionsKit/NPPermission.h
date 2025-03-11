//
//  NPPermissionsKit.h
//  Permissions Kit
//
//  Created by Ayyappa on 09/03/25.
//  Copyright (c) 2025 Voxel Busters Interactive LLP. All rights reserved.


@interface NPPermission : NSObject

@property (nonatomic, strong, readonly) NSString *name;

- (instancetype)initWithName:(NSString *)name;
- (BOOL)equals:(id)object;

@end

@interface NPPermission (Static)

+ (NPPermission *)ACCESS_INTERNET;
+ (NPPermission *)ACCESS_NETWORK_STATE;
+ (NPPermission *)ACCESS_WIFI_STATE;
+ (NPPermission *)ACCESS_LOCAL_NETWORK;

// Location
+ (NPPermission *)ACCESS_FINE_LOCATION;
+ (NPPermission *)ACCESS_COARSE_LOCATION;
+ (NPPermission *)ACCESS_LOCATION_IN_BACKGROUND;

// Notifications
+ (NPPermission *)PUSH_NOTIFICATIONS;

// Media (For Stored Content)
+ (NPPermission *)READ_MEDIA_LIBRARY_IMAGES;
+ (NPPermission *)READ_MEDIA_LIBRARY_VIDEOS;
+ (NPPermission *)READ_MEDIA_LIBRARY_AUDIO;

+ (NPPermission *)ADD_MEDIA_LIBRARY_CONTENT;
+ (NPPermission *)WRITE_MEDIA_LIBRARY_CONTENT;

// Billing
+ (NPPermission *)IN_APP_PURCHASES;

// Hardware
+ (NPPermission *)USE_CAMERA;
+ (NPPermission *)RECORD_AUDIO;
+ (NPPermission *)VIBRATE;
+ (NPPermission *)ACCESS_BLUETOOTH;

@end
