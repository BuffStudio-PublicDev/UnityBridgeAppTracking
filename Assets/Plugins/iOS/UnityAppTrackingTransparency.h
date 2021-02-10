#import <Foundation/Foundation.h>

@protocol DelegateCallBackRequestAppTrackingAuthorization <NSObject>
- (void)ResultStatus:(int)myStatus;
@end

@interface UnityAppTrackingTransparency : NSObject
+ (int)AppTrackingAuthorizationStatus;
+ (void)RequestAppTrackingAuthorization:(id<DelegateCallBackRequestAppTrackingAuthorization>)myDelegateCallBack;
+ (void)SendStatusToDelegate:(id<DelegateCallBackRequestAppTrackingAuthorization>)myDelegateCallBack status:(int)myStatus;
@end
