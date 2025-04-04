//
//  NPLocationManagerListener.mm
//  Permissions Kit
//
//  Created by Ayyappa on 03/04/25.
//  Copyright (c) 2025 Voxel Busters Interactive LLP. All rights reserved.

#import "NPLocationManagerListener.h"

#if PERMISSIONS_KIT_USES_LOCATION_FRAMEWORK
#import <CoreLocation/CoreLocation.h>

@interface NPLocationManagerListener ()

@property (nonatomic, strong) NSMutableArray<LocationAuthorizationStatusCallback> *callbackList;
@property (nonatomic) CLAuthorizationStatus initialStatus;

@end
 
@implementation NPLocationManagerListener

@synthesize callbackList;
@synthesize initialStatus;

-(NPLocationManagerListener*) initwithInitialStatus:(CLAuthorizationStatus) initialStatus
{
    self.callbackList = [[NSMutableArray alloc] init];
    self.initialStatus = initialStatus;
    return self;
}

-(void) addListener:(LocationAuthorizationStatusCallback) completion
{
    [callbackList addObject:completion];
}

- (void)locationManagerDidChangeAuthorization:(CLLocationManager *)manager
{

    CLAuthorizationStatus authStatus = [CLLocationManager authorizationStatus];
    if(initialStatus != authStatus) {
        for (LocationAuthorizationStatusCallback eachCallback in callbackList) {
            eachCallback(authStatus);
        }
    }
}

@end
#endif
