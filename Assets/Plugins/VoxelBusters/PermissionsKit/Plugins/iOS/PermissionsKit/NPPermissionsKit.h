//
//  NPPermissionsKit.h
//  Permissions Kit
//
//  Created by Ayyappa on 09/03/25.
//  Copyright (c) 2025 Voxel Busters Interactive LLP. All rights reserved.

#import "NPPermission.h"
#if PERMISSIONS_KIT_USES_BLUETOOTH
#import <CoreBluetooth/CoreBluetooth.h>
#endif

#define Domain @"Permissions Kit"

typedef enum : NSInteger
{
    PermissionsKitStatusUnknown,
    PermissionsKitStatusAuthorized,
    PermissionsKitStatusLimited,
    PermissionsKitStatusRestricted,
    PermissionsKitStatusDenied,
} PermissionsKitStatus;

typedef void (^PermissionRequestCallback)(NSArray<NPPermission*> *permissions, PermissionsKitStatus statuses[], NSError* error);


#if PERMISSIONS_KIT_USES_BLUETOOTH
@interface NPPermissionsKit : NSObject<CBCentralManagerDelegate>
#else
@interface NPPermissionsKit : NSObject
#endif
-(void) request:(NSArray<NPPermission*>*) permissions withPurposeDescription:(NSString*) purposeDescription showApplicationSettingsIfDeniedOrRestricted:(BOOL) showApplicationSettingsIfDenied withCallback:(PermissionRequestCallback) callback;
-(PermissionsKitStatus) getStatus:(NPPermission*) permission;

@end
