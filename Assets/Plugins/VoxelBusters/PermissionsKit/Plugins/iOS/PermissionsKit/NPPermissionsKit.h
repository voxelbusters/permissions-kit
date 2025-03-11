//
//  NPPermissionsKit.h
//  Permissions Kit
//
//  Created by Ayyappa on 09/03/25.
//  Copyright (c) 2025 Voxel Busters Interactive LLP. All rights reserved.

#import "NPPermission.h"

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


@interface NPPermissionsKit : NSObject

-(void) request:(NSArray<NPPermission*>*) permissions withPurposeDescription:(NSString*) purposeDescription showApplicationSettingsIfDeniedOrRestricted:(BOOL) showApplicationSettingsIfDenied withCallback:(PermissionRequestCallback) callback;
-(PermissionsKitStatus) getStatus:(NPPermission*) permission;

@end
