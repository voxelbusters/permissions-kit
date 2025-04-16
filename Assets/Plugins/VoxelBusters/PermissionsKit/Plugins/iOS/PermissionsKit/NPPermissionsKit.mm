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

#if PERMISSIONS_KIT_USES_CAMERA || PERMISSIONS_KIT_USES_RECORD_AUDIO
#import <AVFoundation/AVFoundation.h>
#endif
#if PERMISSIONS_KIT_USES_LOCATION
#import <CoreLocation/CoreLocation.h>
#import "NPLocationManagerListener.h"
#endif
#if PERMISSIONS_KIT_USES_NOTIFICATIONS
#import <UserNotifications/UserNotifications.h>
#endif
#if PERMISSIONS_KIT_USES_MEDIA
#import <Photos/Photos.h>
#endif

typedef void (^PermissionAlertCallback)(BOOL didOpenSettings);
typedef void (^PermissionsKitCallback)(PermissionsKitStatus status, NSError * _Nullable error);

@interface NPPermissionsKit ()

#if PERMISSIONS_KIT_USES_LOCATION
@property (nonatomic, strong) CLLocationManager *locationManager;
@property (nonatomic, strong) NPLocationManagerListener *locationStatusListener;
#endif

#if PERMISSIONS_KIT_USES_BLUETOOTH
@property (nonatomic, strong) CBCentralManager *bluetoothManager;
@property (nonatomic, copy) PermissionsKitCallback bluetoothCallback;
#endif

@end

@implementation NPPermissionsKit

#if PERMISSIONS_KIT_USES_LOCATION
@synthesize locationManager;
@synthesize locationStatusListener;
#endif

#if PERMISSIONS_KIT_USES_BLUETOOTH
@synthesize bluetoothManager;
@synthesize bluetoothCallback;
#endif


-(NPPermissionsKit*) init
{
    
    self  =  [super init];
    return self;;
}


-(PermissionsKitStatus) getStatus:(NPPermission*) permission
{
#if PERMISSIONS_KIT_USES_LOCATION
    if ([permission equals:[NPPermission ACCESS_FINE_LOCATION]] ||
        [permission equals:[NPPermission ACCESS_COARSE_LOCATION]]) {

        return [self getLocationStatus: NO];

    } else if ([permission equals:[NPPermission ACCESS_LOCATION_IN_BACKGROUND]]) {

        return [self getLocationStatus: YES];
    } else
#endif
#if PERMISSIONS_KIT_USES_NOTIFICATIONS
        if ([permission equals:[NPPermission PUSH_NOTIFICATIONS]]) {
           return [self getNotificationAuthorizationStatus];
    } else
#endif
#if PERMISSIONS_KIT_USES_CAMERA
        if ([permission equals:[NPPermission USE_CAMERA]]) {
            return [self getAVAuthorizationStatus:AVMediaTypeVideo];
    } else
#endif
#if PERMISSIONS_KIT_USES_RECORD_AUDIO
        if ([permission equals:[NPPermission RECORD_AUDIO]]) {
            return [self getAudioRecordingAuthorizationStatus];
    } else
#endif
#if PERMISSIONS_KIT_USES_MEDIA
        if ([permission equals:[NPPermission READ_MEDIA_LIBRARY_IMAGES]] ||
            [permission equals:[NPPermission READ_MEDIA_LIBRARY_VIDEOS]] ||
            [permission equals:[NPPermission READ_MEDIA_LIBRARY_AUDIO]] ||
            [permission equals:[NPPermission WRITE_MEDIA_LIBRARY_CONTENT]]) {
        
            return [self convertPHAuthorizationStatus: [PHPhotoLibrary authorizationStatusForAccessLevel: PHAccessLevelReadWrite]];
        } else if ([permission equals:[NPPermission ADD_MEDIA_LIBRARY_CONTENT]]) {
            return [self convertPHAuthorizationStatus: [PHPhotoLibrary authorizationStatusForAccessLevel: PHAccessLevelAddOnly]];
        } else
#endif
#if PERMISSIONS_KIT_USES_BLUETOOTH
        if ([permission equals:[NPPermission ACCESS_BLUETOOTH]]) {
            return [self getBluetoothPermissionStatus];
    } else
#endif
        if ([permission equals:[NPPermission ACCESS_INTERNET]] ||
            [permission equals:[NPPermission ACCESS_WIFI_STATE]] ||
            [permission equals:[NPPermission VIBRATE]] ||
            [permission equals:[NPPermission IN_APP_PURCHASES]])
        {
            NSLog(@"No need of any request for this permission(%@)", permission);
            return PermissionsKitStatusAuthorized;
        }
        else if ([permission equals:[NPPermission ACCESS_NETWORK_STATE]])
        {
            NSLog(@"TODO: Currently passing unknown for permission(%@)", permission);//This is because, currently there is no direct way to check it.
            return PermissionsKitStatusUnknown;
        }
        else
        {
            return PermissionsKitStatusUnknown;
        }
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
                        
#if PERMISSIONS_KIT_USES_LOCATION
            
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
#if PERMISSIONS_KIT_USES_NOTIFICATIONS
                if ([permission equals:[NPPermission PUSH_NOTIFICATIONS]]) {
                
                [self requestPushNotificationPermission:^(PermissionsKitStatus status, NSError * _Nullable err) {
                    completionHandler(permission, status, err);
                }];
                
            } else
#endif
#if PERMISSIONS_KIT_USES_CAMERA
                if ([permission equals:[NPPermission USE_CAMERA]]) {
                
                [self requestCameraPermission:^(PermissionsKitStatus status, NSError * _Nullable err) {
                    completionHandler(permission, status, err);
                }];
                
            } else 
#endif
#if PERMISSIONS_KIT_USES_RECORD_AUDIO 
                if ([permission equals:[NPPermission RECORD_AUDIO]]) {
                
                [self requestMicrophonePermission:^(PermissionsKitStatus status, NSError * _Nullable err) {
                    completionHandler(permission, status, err);
                }];
                
            } else
#endif
#if PERMISSIONS_KIT_USES_MEDIA
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
#if PERMISSIONS_KIT_USES_BLUETOOTH
                if ([permission equals:[NPPermission ACCESS_BLUETOOTH]]) {
                
                [self checkBluetoothPermission:^(PermissionsKitStatus status, NSError * _Nullable err) {
                    completionHandler(permission, status, err);
                }];
                
            } else
#endif
                if ([permission equals:[NPPermission ACCESS_INTERNET]] ||
                    [permission equals:[NPPermission ACCESS_WIFI_STATE]] ||
                    [permission equals:[NPPermission VIBRATE]] ||
                    [permission equals:[NPPermission IN_APP_PURCHASES]])
                {
                    NSLog(@"No need of any request for this permission(%@)", permission);
                    completionHandler(permission, PermissionsKitStatusAuthorized, nil);
                }
                else if ([permission equals:[NPPermission ACCESS_NETWORK_STATE]])
                {
                    NSLog(@"TODO: Currently passing unknown for permission(%@)", permission);
                    completionHandler(permission, PermissionsKitStatusUnknown, nil);
                }
                else
                {
                    NSLog(@"Permission(%@) is not handled. This can be due to not enabling the permission in settings or report to this plugin developer with the permission name.", permission);
                    completionHandler(permission, PermissionsKitStatusUnknown, nil);
                }
        }
    }

    // MARK: - Individual Permission Methods
#if PERMISSIONS_KIT_USES_LOCATION

-(PermissionsKitStatus) getLocationStatus:(BOOL) requestAlways
{
    CLAuthorizationStatus authStatus = [CLLocationManager authorizationStatus];
    return [self convertStatus:authStatus withRequestAlways:requestAlways];
}

- (void)requestLocationPermission:(BOOL)requestAlways
                       completion:(void (^)(PermissionsKitStatus status, NSError * _Nullable error))completion {

    CLAuthorizationStatus authStatus = [CLLocationManager authorizationStatus];
    PermissionsKitStatus status = PermissionsKitStatusUnknown;
    
    if(self.locationManager == nil) {
        self.locationManager = [[CLLocationManager alloc] init];
        self.locationStatusListener = [[NPLocationManagerListener alloc] init];
    }
    
    __weak NPPermissionsKit *weakSelf = self;
    
    [self.locationStatusListener addListener:^(CLAuthorizationStatus authStatus) {
        PermissionsKitStatus newStatus = [weakSelf convertStatus:authStatus withRequestAlways:requestAlways];
        completion(newStatus, nil);
        weakSelf.locationManager = nil;
        weakSelf.locationStatusListener = nil;
    }];
    
    status = [self getLocationStatus:requestAlways];
    
    if(status == PermissionsKitStatusUnknown || (status == PermissionsKitStatusLimited && requestAlways)) {
        locationManager.delegate = self.locationStatusListener;
        if (requestAlways) {
            [locationManager requestAlwaysAuthorization];
        } else {
            [locationManager requestWhenInUseAuthorization];
        }
    } else {
        self.locationManager = nil;
        completion(status, nil);
    }
}

-(PermissionsKitStatus) convertStatus:(CLAuthorizationStatus) authStatus withRequestAlways:(BOOL) requestAlways
{
    PermissionsKitStatus status;
    if (authStatus == kCLAuthorizationStatusNotDetermined) {
        status = PermissionsKitStatusUnknown;
    } else if (authStatus == kCLAuthorizationStatusAuthorizedWhenInUse) {
        status = requestAlways ? PermissionsKitStatusLimited : PermissionsKitStatusAuthorized;
    } else if (authStatus == kCLAuthorizationStatusAuthorizedAlways) {
        status = PermissionsKitStatusAuthorized;
    } else if (authStatus == kCLAuthorizationStatusDenied) {
        status = PermissionsKitStatusDenied;
    } else {
        status = PermissionsKitStatusRestricted;
    }
    
    return status;
}


#endif

#if PERMISSIONS_KIT_USES_NOTIFICATIONS
- (void)requestPushNotificationPermission:(void (^)(PermissionsKitStatus status, NSError * _Nullable error))completion {
    UNUserNotificationCenter *center = [UNUserNotificationCenter currentNotificationCenter];
    [center requestAuthorizationWithOptions:(UNAuthorizationOptionAlert | UNAuthorizationOptionSound | UNAuthorizationOptionBadge)
                          completionHandler:^(BOOL granted, NSError * _Nullable error) {
        completion(granted ? PermissionsKitStatusAuthorized : PermissionsKitStatusDenied, error);
    }];
}

- (PermissionsKitStatus)getNotificationAuthorizationStatus {
    __block UNNotificationSettings *result = nil;
    dispatch_semaphore_t semaphore = dispatch_semaphore_create(0);
    
    [[UNUserNotificationCenter currentNotificationCenter] getNotificationSettingsWithCompletionHandler:^(UNNotificationSettings * _Nonnull settings) {
        result = settings;
        dispatch_semaphore_signal(semaphore);
    }];
    
    dispatch_semaphore_wait(semaphore, DISPATCH_TIME_FOREVER);
    
    switch (result.authorizationStatus) {
        case UNAuthorizationStatusNotDetermined:
            return PermissionsKitStatusUnknown;
            break;
        case UNAuthorizationStatusDenied:
            return PermissionsKitStatusDenied;
            break;
        case UNAuthorizationStatusAuthorized:
        case UNAuthorizationStatusEphemeral:
            return PermissionsKitStatusAuthorized;
            break;
        case UNAuthorizationStatusProvisional:
            return PermissionsKitStatusLimited;
            break;
        default:
            break;
    }
    
    return PermissionsKitStatusUnknown;
}
#endif

#if PERMISSIONS_KIT_USES_CAMERA
- (void)requestCameraPermission:(void (^)(PermissionsKitStatus status, NSError * _Nullable error))completion {
    
        [AVCaptureDevice requestAccessForMediaType:AVMediaTypeVideo completionHandler:^(BOOL granted) {
            PermissionsKitStatus status = [self getAVAuthorizationStatus: AVMediaTypeVideo];
        
            // send callback
            completion(status, nil);
        }];
 
    }
- (PermissionsKitStatus) getAVAuthorizationStatus:(AVMediaType) AVMediaType
{
    AVAuthorizationStatus   authStatus  = [AVCaptureDevice authorizationStatusForMediaType:AVMediaTypeVideo];
    
    PermissionsKitStatus status = PermissionsKitStatusUnknown;
    if (authStatus == AVAuthorizationStatusAuthorized) {
        status = PermissionsKitStatusAuthorized;
    } else if (authStatus == AVAuthorizationStatusDenied) {
        status = PermissionsKitStatusDenied;
    } else if (authStatus == AVAuthorizationStatusRestricted) {
        status = PermissionsKitStatusRestricted;
    }
    
    return status;
}

#endif

#if PERMISSIONS_KIT_USES_RECORD_AUDIO
    - (void)requestMicrophonePermission:(void (^)(PermissionsKitStatus status, NSError * _Nullable error))completion {
        [[AVAudioSession sharedInstance] requestRecordPermission:^(BOOL granted) {
            completion(granted ? PermissionsKitStatusAuthorized : PermissionsKitStatusDenied, nil);
        }];
    }
    
    - (PermissionsKitStatus) getAudioRecordingAuthorizationStatus {
        AVAudioSession *session = [AVAudioSession sharedInstance];
        AVAudioSessionRecordPermission permissionStatus = [session recordPermission];
        
        switch (permissionStatus) {
            case AVAudioSessionRecordPermissionUndetermined:
                return PermissionsKitStatusUnknown;
                break;
                
            case AVAudioSessionRecordPermissionDenied:
                return PermissionsKitStatusDenied;
                break;
                
            case AVAudioSessionRecordPermissionGranted:
                return PermissionsKitStatusAuthorized;
                break;
                
            default:
                return PermissionsKitStatusUnknown;
                break;
        }
    }
#endif

#if PERMISSIONS_KIT_USES_MEDIA
- (PermissionsKitStatus) convertPHAuthorizationStatus:(PHAuthorizationStatus)authStatus {
    PermissionsKitStatus status = PermissionsKitStatusUnknown;
    if (authStatus == PHAuthorizationStatusAuthorized) {
        status = PermissionsKitStatusAuthorized;
    } else if (authStatus == PHAuthorizationStatusDenied) {
        status = PermissionsKitStatusDenied;
    } else if (authStatus == PHAuthorizationStatusRestricted) {
        status = PermissionsKitStatusRestricted;
    } else if (authStatus == PHAuthorizationStatusLimited) {
        status = PermissionsKitStatusLimited;
    } else {
        NSLog(@"PHAuthorizationStatus not handled %ld", authStatus);
    }

    return status;
}

- (void) requestPhotoLibraryPermission:(PHAccessLevel) accessLevel withCallback:(void (^)(PermissionsKitStatus status, NSError * _Nullable error))completion  API_AVAILABLE(ios(14)) {
        
        void (^handler)(PHAuthorizationStatus status) = ^(PHAuthorizationStatus authStatus) {
            PermissionsKitStatus status = [self convertPHAuthorizationStatus:authStatus];
            completion(status, nil);
        };
    
        if (@available(iOS 14, *)) {
            [PHPhotoLibrary requestAuthorizationForAccessLevel:accessLevel handler:handler];
        } else {
            // Fallback on earlier versions
            [PHPhotoLibrary requestAuthorization:handler];
        }
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

#if PERMISSIONS_KIT_USES_BLUETOOTH
    - (PermissionsKitStatus) getBluetoothPermissionStatus
    {
        CBManagerAuthorization authStatus = CBCentralManager.authorization;
        PermissionsKitStatus status;
        switch (authStatus) {
            case CBManagerAuthorizationNotDetermined:
                return PermissionsKitStatusUnknown;
                break;
            case CBManagerAuthorizationRestricted:
                return PermissionsKitStatusRestricted;
                break;
            case CBManagerAuthorizationDenied:
                return PermissionsKitStatusDenied;
                break;
            case CBManagerAuthorizationAllowedAlways:
                return PermissionsKitStatusAuthorized;
                break;
            default:
                return PermissionsKitStatusUnknown;
                break;
        }
    }

    - (void)checkBluetoothPermission:(void (^)(PermissionsKitStatus status, NSError * _Nullable error))completion {
        self.bluetoothManager = [[CBCentralManager alloc] initWithDelegate:self queue:nil];
        self.bluetoothCallback = completion;
    }

    - (void)centralManagerDidUpdateState:(CBCentralManager *)central
    {
        if(self.bluetoothCallback != nil) {
            PermissionsKitStatus status = [self getBluetoothPermissionStatus];
            self.bluetoothCallback(status, nil);
            self.bluetoothManager   = nil;
            self.bluetoothCallback  = nil;
        }
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
