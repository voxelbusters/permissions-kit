//
//  NPPermissionsKitBinding.mm
//  Permissions Kit
//
//  Created by Ayyappa on 09/03/25.
//  Copyright (c) 2024 Voxel Busters Interactive LLP. All rights reserved.

#import "NPKit.h"
#import "NPPermissionsKit.h"

// callback signatures
typedef void (*PermissionRequestNativeCallback)(void* permissionsStatusInfoArray, int length, NPError error, void* tagPtr);

struct NativePermissionsKitPermissionStatusInfoData
{
    const char* permission;
    int status;
};
typedef NativePermissionsKitPermissionStatusInfoData NativePermissionsKitPermissionStatusInfoData;


static NPPermissionsKit* cachedPermissionsKit;

void* createPermissionsStatusInfoArray(NSArray<NPPermission*> *permissions, PermissionsKitStatus statuses[])
{
    if (permissions)
    {
        // set length
        int length     = (int)[permissions count];
        
        // create data array
        NativePermissionsKitPermissionStatusInfoData*      newDataArray    = (NativePermissionsKitPermissionStatusInfoData*)calloc(length, sizeof(NativePermissionsKitPermissionStatusInfoData));
        for (int iter = 0; iter < length; iter++)
        {
            NPPermission* permission        = [permissions objectAtIndex:iter];
            NativePermissionsKitPermissionStatusInfoData*  newDataObject   = &newDataArray[iter];
            newDataObject->permission = [permission.name UTF8String];
            newDataObject->status = (int)statuses[iter];
        }
        
        return newDataArray;
    }
    else
    {
        return nil;
    }
}


NPPermissionsKit* getPermissionsKit()
{
    if(cachedPermissionsKit == nil)
    {
        cachedPermissionsKit = [[NPPermissionsKit alloc] init];
    }
    
    return cachedPermissionsKit;
}


NPBINDING DONTSTRIP void NPPermissionsKitRequest(const char** permissions, int length, const char* purposeDescription, int showApplicationSettingsIfDeniedOrRestricted, PermissionRequestNativeCallback callback, void* tagPtr)
{
    NPPermissionsKit *permissionsKit = getPermissionsKit();
    NSMutableArray *permissionsArray = [NSMutableArray array];
    for(int i=0; i<length; i++)
    {
        NSString *name = NPCreateNSStringFromCString(permissions[i]);
        [permissionsArray addObject:[[NPPermission alloc] initWithName:name]];
    }
    

    [permissionsKit request:permissionsArray withPurposeDescription:NPCreateNSStringFromCString(purposeDescription)
                        showApplicationSettingsIfDeniedOrRestricted:showApplicationSettingsIfDeniedOrRestricted == 1
                                                        withCallback:^(NSArray<NPPermission *> *permissions, PermissionsKitStatus statuses[], NSError *error) {
        int count = (int)[permissions count];
        void* infoArray = createPermissionsStatusInfoArray(permissions, statuses);
        callback(infoArray, count, NPCreateError(error), tagPtr);
    }];
}

NPBINDING DONTSTRIP int NPPermissionsKitGetStatus(const char* permission)
{
    NPPermissionsKit *permissionsKit = getPermissionsKit();
    return (int)[permissionsKit getStatus:[[NPPermission alloc] initWithName:NPCreateNSStringFromCString(permission)]];
}



