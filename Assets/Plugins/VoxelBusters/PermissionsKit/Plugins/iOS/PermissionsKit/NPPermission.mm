//
//  NPPermissionsKit.h
//  Permissions Kit
//
//  Created by Ayyappa on 09/03/25.
//  Copyright (c) 2025 Voxel Busters Interactive LLP. All rights reserved.

#import "NPPermission.h"

@implementation NPPermission

- (instancetype)initWithName:(NSString *)name {
    self = [super init];
    if (self) {
        _name = [name copy];
    }
    return self;
}

- (BOOL)equals:(id)object {
    if (![object isKindOfClass:[NPPermission class]]) {
        return NO;
    }
    return [self.name isEqualToString:((NPPermission *)object).name];
}

@end

@implementation NPPermission (Static)

+ (NPPermission *)ACCESS_INTERNET                { return [[NPPermission alloc] initWithName:@"ACCESS_INTERNET"]; }
+ (NPPermission *)ACCESS_NETWORK_STATE           { return [[NPPermission alloc] initWithName:@"ACCESS_NETWORK_STATE"]; }
+ (NPPermission *)ACCESS_WIFI_STATE              { return [[NPPermission alloc] initWithName:@"ACCESS_WIFI_STATE"]; }
+ (NPPermission *)ACCESS_LOCAL_NETWORK           { return [[NPPermission alloc] initWithName:@"ACCESS_LOCAL_NETWORK"]; }

// Location
+ (NPPermission *)ACCESS_FINE_LOCATION           { return [[NPPermission alloc] initWithName:@"ACCESS_FINE_LOCATION"]; }
+ (NPPermission *)ACCESS_COARSE_LOCATION         { return [[NPPermission alloc] initWithName:@"ACCESS_COARSE_LOCATION"]; }
+ (NPPermission *)ACCESS_LOCATION_IN_BACKGROUND  { return [[NPPermission alloc] initWithName:@"ACCESS_LOCATION_IN_BACKGROUND"]; }

// Notifications
+ (NPPermission *)PUSH_NOTIFICATIONS             { return [[NPPermission alloc] initWithName:@"PUSH_NOTIFICATIONS"]; }

// Media (For Stored Content)
+ (NPPermission *)READ_MEDIA_LIBRARY_IMAGES     { return [[NPPermission alloc] initWithName:@"READ_MEDIA_LIBRARY_IMAGES"]; }
+ (NPPermission *)READ_MEDIA_LIBRARY_VIDEOS     { return [[NPPermission alloc] initWithName:@"READ_MEDIA_LIBRARY_VIDEOS"]; }
+ (NPPermission *)READ_MEDIA_LIBRARY_AUDIO      { return [[NPPermission alloc] initWithName:@"READ_MEDIA_LIBRARY_AUDIO"]; }

+ (NPPermission *)ADD_MEDIA_LIBRARY_CONTENT     { return [[NPPermission alloc] initWithName:@"ADD_MEDIA_LIBRARY_CONTENT"]; }
+ (NPPermission *)WRITE_MEDIA_LIBRARY_CONTENT   { return [[NPPermission alloc] initWithName:@"WRITE_MEDIA_LIBRARY_CONTENT"]; }

// Billing
+ (NPPermission *)IN_APP_PURCHASES               { return [[NPPermission alloc] initWithName:@"IN_APP_PURCHASES"]; }

// Hardware
+ (NPPermission *)USE_CAMERA                     { return [[NPPermission alloc] initWithName:@"USE_CAMERA"]; }
+ (NPPermission *)RECORD_AUDIO                   { return [[NPPermission alloc] initWithName:@"RECORD_AUDIO"]; }
+ (NPPermission *)VIBRATE                        { return [[NPPermission alloc] initWithName:@"VIBRATE"]; }
+ (NPPermission *)ACCESS_BLUETOOTH               { return [[NPPermission alloc] initWithName:@"ACCESS_BLUETOOTH"]; }

@end

