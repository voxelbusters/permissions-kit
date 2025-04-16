//
//  NPLocationManagerListener.h
//  Permissions Kit
//
//  Created by Ayyappa on 03/04/25.
//  Copyright (c) 2025 Voxel Busters Interactive LLP. All rights reserved.

#import "NPConfig.h"

#if PERMISSIONS_KIT_USES_LOCATION
#import <CoreLocation/CoreLocation.h>
typedef void (^LocationAuthorizationStatusCallback) (CLAuthorizationStatus authStatus);

@interface NPLocationManagerListener : NSObject<CLLocationManagerDelegate>

-(NPLocationManagerListener*) init;

-(void) addListener:(LocationAuthorizationStatusCallback) completion;

@end
#endif
