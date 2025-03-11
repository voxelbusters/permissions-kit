//
//  NPPermissionsKit.mm
//  Permissions Kit
//
//  Created by Ayyappa on 09/03/25.
//  Copyright (c) 2024 Voxel Busters Interactive LLP. All rights reserved.

#import "NPKit.h"
#import "NPPermissionsKit.h"
#import "UnityInterface.h"
#import "NSError+Utility.h"
#import "UnityInterface.h"
#import "UnityAppController.h"

#if PERMISSIONS_KIT_USES_AVFOUNDATION_FRAMEWORK
#import <AVFoundation/AVFoundation.h>
#endif
#if PERMISSIONS_KIT_USES_LOCATION_FRAMEWORK
#import <CoreLocation/CoreLocation.h>
#endif
#if PERMISSIONS_KIT_USES_NOTIFICATIONS_FRAMEWORK
#import <UserNotifications/UserNotifications.h>
#endif
#if PERMISSIONS_KIT_USES_PHOTOS_FRAMEWORK
#import <Photos/Photos.h>
#endif
#if PERMISSIONS_KIT_USES_BLUETOOTH_FRAMEWORK
#import <CoreBluetooth/CoreBluetooth.h>
#endif

typedef void (^PermissionAlertCallback)(BOOL didOpenSettings);

@interface NPPermissionsKit ()

@end

@implementation NPPermissionsKit

-(NPPermissionsKit*) init
{
    
    self  =  [super init];
    return self;;
}

-(PermissionsKitStatus) getStatus:(NPPermission*) permission
{
    return PermissionsKitStatusUnknown;
}


-(void) request:(NSArray<NPPermission*>*) permissions withPurposeDescription:(NSString*) purposeDescription showApplicationSettingsIfDeniedOrRestricted:(BOOL) showApplicationSettingsIfDeniedOrRestricted withCallback:(PermissionRequestCallback) callback
{
        __block NSInteger totalRequests = permissions.count;
            
        __block NSString *syncObject = @"";
        __block int recievedPermissions = 0;
        __block int deniedPermissions = 0;
        PermissionsKitStatus *statusesArray = (PermissionsKitStatus*) malloc(sizeof(PermissionsKitStatus) * permissions.count);
    
        void (^completionHandler)(NPPermission *permission, PermissionsKitStatus status, NSError * _Nullable err) =
        ^(NPPermission *permission, PermissionsKitStatus status, NSError * _Nullable err) {
            @synchronized (syncObject) {
                statusesArray[recievedPermissions] = status;
                recievedPermissions++;
                
                if((status == PermissionsKitStatusDenied || status == PermissionsKitStatusRestricted))
                {
                    deniedPermissions++;
                }
                                
                if (recievedPermissions == totalRequests) {
                    
                    [self handleFinalCallbackWithPermissions:permissions
                                               statusesArray:statusesArray
                                                       error:err
                                     showApplicationSettings: (showApplicationSettingsIfDeniedOrRestricted && (deniedPermissions > 0))
                                          purposeDescription:purposeDescription
                                                    callback:callback];
                }
            }
        };

        for (NPPermission *permission in permissions) {
                        
#if PERMISSIONS_KIT_USES_LOCATION_FRAMEWORK
            
            if ([permission equals:[NPPermission ACCESS_FINE_LOCATION]] ||
                [permission equals:[NPPermission ACCESS_COARSE_LOCATION]]) {

                // Request "When In Use" location permission
                [self requestLocationPermission:NO completion:^(PermissionsKitStatus status, NSError * _Nullable err) {
                    completionHandler(permission, status, err);
                }];

            } else if ([permission equals:[NPPermission ACCESS_LOCATION_IN_BACKGROUND]]) {

                // Request "Always" location permission (background access)
                [self requestLocationPermission:YES completion:^(PermissionsKitStatus status, NSError * _Nullable err) {
                    completionHandler(permission, status, err);
                }];
            } else
#endif
#if PERMISSIONS_KIT_USES_NOTIFICATIONS_FRAMEWORK
                if ([permission equals:[NPPermission PUSH_NOTIFICATIONS]]) {
                
                [self requestPushNotificationPermission:^(PermissionsKitStatus status, NSError * _Nullable err) {
                    completionHandler(permission, status, err);
                }];
                
            } else
#endif
#if PERMISSIONS_KIT_USES_AVFOUNDATION_FRAMEWORK
                if ([permission equals:[NPPermission USE_CAMERA]]) {
                
                [self requestCameraPermission:^(PermissionsKitStatus status, NSError * _Nullable err) {
                    completionHandler(permission, status, err);
                }];
                
            } else if ([permission equals:[NPPermission RECORD_AUDIO]]) {
                
                [self requestMicrophonePermission:^(PermissionsKitStatus status, NSError * _Nullable err) {
                    completionHandler(permission, status, err);
                }];
                
            } else
#endif
#if PERMISSIONS_KIT_USES_PHOTOS_FRAMEWORK
                if ([permission equals:[NPPermission READ_MEDIA_LIBRARY_IMAGES]] ||
                    [permission equals:[NPPermission READ_MEDIA_LIBRARY_VIDEOS]] ||
                    [permission equals:[NPPermission READ_MEDIA_LIBRARY_AUDIO]] ||
                    [permission equals:[NPPermission WRITE_MEDIA_LIBRARY_CONTENT]]) {
                
                    [self requestPhotoLibraryPermission:PHAccessLevelReadWrite withCallback:^(PermissionsKitStatus status, NSError * _Nullable err) {
                        completionHandler(permission, status, err);
                    }];
                } else if ([permission equals:[NPPermission ADD_MEDIA_LIBRARY_CONTENT]]) {
                    [self requestPhotoLibraryPermission:PHAccessLevelAddOnly withCallback: ^(PermissionsKitStatus status, NSError * _Nullable err) {
                        completionHandler(permission, status, err);
                    }];
                    
                } else
#endif
#if PERMISSIONS_KIT_USES_BLUETOOTH_FRAMEWORK
                if ([permission equals:[NPPermission ACCESS_BLUETOOTH]]) {
                
                [self checkBluetoothPermission:^(PermissionsKitStatus status, NSError * _Nullable err) {
                    completionHandler(permission, status, err);
                }];
                
            } else
#endif
            {
                completionHandler(permission, PermissionsKitStatusAuthorized, nil);
            }
        }
    }

    // MARK: - Individual Permission Methods
#if PERMISSIONS_KIT_USES_LOCATION_FRAMEWORK
- (void)requestLocationPermission:(BOOL)requestAlways
                       completion:(void (^)(PermissionsKitStatus status, NSError * _Nullable error))completion {
    
    CLLocationManager *locationManager = [[CLLocationManager alloc] init];
    CLAuthorizationStatus authStatus = [CLLocationManager authorizationStatus];
    PermissionsKitStatus status = PermissionsKitStatusUnknown;

    if (authStatus == kCLAuthorizationStatusNotDetermined) {
        // First-time request
        if (requestAlways) {
            [locationManager requestAlwaysAuthorization];
        } else {
            [locationManager requestWhenInUseAuthorization];
        }
        status = PermissionsKitStatusUnknown;
    } else if (authStatus == kCLAuthorizationStatusAuthorizedWhenInUse) {
        status = requestAlways ? PermissionsKitStatusLimited : PermissionsKitStatusAuthorized;
        if (requestAlways) {
            [locationManager requestAlwaysAuthorization];
        }
    } else if (authStatus == kCLAuthorizationStatusAuthorizedAlways) {
        status = PermissionsKitStatusAuthorized;
    } else if (authStatus == kCLAuthorizationStatusDenied) {
        status = PermissionsKitStatusDenied;
    } else {
        status = PermissionsKitStatusRestricted;
    }

    completion(status, nil);
}
#endif

#if PERMISSIONS_KIT_USES_LOCATION_FRAMEWORK
    - (void)requestPushNotificationPermission:(void (^)(PermissionsKitStatus status, NSError * _Nullable error))completion {
        UNUserNotificationCenter *center = [UNUserNotificationCenter currentNotificationCenter];
        [center requestAuthorizationWithOptions:(UNAuthorizationOptionAlert | UNAuthorizationOptionSound | UNAuthorizationOptionBadge)
                              completionHandler:^(BOOL granted, NSError * _Nullable error) {
            completion(granted ? PermissionsKitStatusAuthorized : PermissionsKitStatusDenied, error);
        }];
    }
#endif

#if PERMISSIONS_KIT_USES_AVFOUNDATION_FRAMEWORK
    - (void)requestCameraPermission:(void (^)(PermissionsKitStatus status, NSError * _Nullable error))completion {
        AVAuthorizationStatus authStatus = [AVCaptureDevice authorizationStatusForMediaType:AVMediaTypeVideo];
        
        PermissionsKitStatus status = PermissionsKitStatusUnknown;
        if (authStatus == AVAuthorizationStatusAuthorized) {
            status = PermissionsKitStatusAuthorized;
        } else if (authStatus == AVAuthorizationStatusDenied) {
            status = PermissionsKitStatusDenied;
        } else if (authStatus == AVAuthorizationStatusRestricted) {
            status = PermissionsKitStatusRestricted;
        }
        
        completion(status, nil);
    }

    - (void)requestMicrophonePermission:(void (^)(PermissionsKitStatus status, NSError * _Nullable error))completion {
        [[AVAudioSession sharedInstance] requestRecordPermission:^(BOOL granted) {
            completion(granted ? PermissionsKitStatusAuthorized : PermissionsKitStatusDenied, nil);
        }];
    }
#endif

#if PERMISSIONS_KIT_USES_PHOTOS_FRAMEWORK
    - (void) requestPhotoLibraryPermission:(PHAccessLevel) accessLevel withCallback:(void (^)(PermissionsKitStatus status, NSError * _Nullable error))completion {
        [PHPhotoLibrary requestAuthorizationForAccessLevel:accessLevel handler:^(PHAuthorizationStatus authStatus) {
            PermissionsKitStatus status = PermissionsKitStatusUnknown;
            if (authStatus == PHAuthorizationStatusAuthorized) {
                status = PermissionsKitStatusAuthorized;
            } else if (authStatus == PHAuthorizationStatusDenied) {
                status = PermissionsKitStatusDenied;
            } else if (authStatus == PHAuthorizationStatusRestricted) {
                status = PermissionsKitStatusRestricted;
            } else if (@available(iOS 14, *)) {
                if (authStatus == PHAuthorizationStatusLimited) {
                    status = PermissionsKitStatusLimited;
                }
            }
            
            completion(status, nil);
        }];
    }

    - (void) requestWriteToPhotoLibraryPermission: (void (^)(PermissionsKitStatus status, NSError * _Nullable error))completion {
        [PHPhotoLibrary requestAuthorization:^(PHAuthorizationStatus authStatus) {
            PermissionsKitStatus status = PermissionsKitStatusUnknown;
            if (authStatus == PHAuthorizationStatusAuthorized) {
                status = PermissionsKitStatusAuthorized;
            } else if (authStatus == PHAuthorizationStatusDenied) {
                status = PermissionsKitStatusDenied;
            } else if (authStatus == PHAuthorizationStatusRestricted) {
                status = PermissionsKitStatusRestricted;
            } else if (@available(iOS 14, *)) {
                if (authStatus == PHAuthorizationStatusLimited) {
                    status = PermissionsKitStatusLimited;
                }
            }
            
            completion(status, nil);
        }];
    }
#endif

#if PERMISSIONS_KIT_USES_BLUETOOTH_FRAMEWORK
    - (void)checkBluetoothPermission:(void (^)(PermissionsKitStatus status, NSError * _Nullable error))completion {
        CBCentralManager *bluetoothManager = [[CBCentralManager alloc] initWithDelegate:nil queue:nil];
        
        PermissionsKitStatus status = PermissionsKitStatusUnknown;
        if (bluetoothManager.state == CBManagerStatePoweredOn) {
            status = PermissionsKitStatusAuthorized;
        } else if (bluetoothManager.state == CBManagerStateUnauthorized) {
            status = PermissionsKitStatusDenied;
        } else if (bluetoothManager.state == CBManagerStateUnsupported) {
            status = PermissionsKitStatusRestricted;
        }
        
        completion(status, nil);
    }
#endif


- (void)handleFinalCallbackWithPermissions:(NSArray<NPPermission *> *)permissions
                             statusesArray:(PermissionsKitStatus *)statusesArray
                                     error:(NSError * _Nullable)err
                   showApplicationSettings:(BOOL)showApplicationSettings
                      purposeDescription:(NSString *)purposeDescription
                                callback:(PermissionRequestCallback)callback
{
    if (showApplicationSettings) {
        dispatch_async(dispatch_get_main_queue(), ^{
            [self showPermissionDeniedAlertWithMessage:purposeDescription
                                              callback:^(BOOL didOpenSettings) {
                callback(permissions, statusesArray, err);
            }];
        });
    } else {
        callback(permissions, statusesArray, err);
    }
}

- (void)showPermissionDeniedAlertWithMessage:(NSString *)message callback:(PermissionAlertCallback)callback {
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:@"Permission Denied"
                                                                             message:message
                                                                      preferredStyle:UIAlertControllerStyleAlert];

    UIAlertAction *cancelAction = [UIAlertAction actionWithTitle:@"Cancel"
                                                           style:UIAlertActionStyleCancel
                                                         handler:^(UIAlertAction * _Nonnull action) {
        if (callback) {
            callback(NO);
        }
    }];

    UIAlertAction *settingsAction = [UIAlertAction actionWithTitle:@"Settings"
                                                             style:UIAlertActionStyleDefault
                                                           handler:^(UIAlertAction * _Nonnull action) {
        NSURL *settingsURL = [NSURL URLWithString:UIApplicationOpenSettingsURLString];
        [[UIApplication sharedApplication] openURL:settingsURL options:@{} completionHandler:^(BOOL success) {
            callback(success);
        }];
    }];

    [alertController addAction:cancelAction];
    [alertController addAction:settingsAction];

    [UnityGetGLViewController() presentViewController:alertController animated:YES completion:nil];
}
                 
@end
