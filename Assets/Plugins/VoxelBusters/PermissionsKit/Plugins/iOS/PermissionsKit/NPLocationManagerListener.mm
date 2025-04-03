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

@property (nonatomic, copy) LocationAuthorizationStatusCallback callback;
@property (nonatomic) CLAuthorizationStatus initialStatus;

@end
 
@implementation NPLocationManagerListener

@synthesize callback;
@synthesize initialStatus;

-(NPLocationManagerListener*) init:(LocationAuthorizationStatusCallback) callback withInitialStatus:(CLAuthorizationStatus) initialStatus
{
    self.callback = callback;
    self.initialStatus = initialStatus;
    return self;
}

- (void)locationManagerDidChangeAuthorization:(CLLocationManager *)manager
{

    CLAuthorizationStatus authStatus = [CLLocationManager authorizationStatus];
    
    if(initialStatus != authStatus) {
        self.callback(authStatus);
    }
}

@end
#endif
