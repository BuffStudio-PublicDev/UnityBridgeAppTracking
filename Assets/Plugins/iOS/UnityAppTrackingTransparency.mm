#import "UnityAppTrackingTransparency.h"
#import <AppTrackingTransparency/AppTrackingTransparency.h>

@implementation UnityAppTrackingTransparency

+ (int) AppTrackingAuthorizationStatus
{
    int result = 0;
    if (@available(iOS 14.5, *))
    {
        result = (int)[ATTrackingManager trackingAuthorizationStatus];
    }
    
    return result;
}

+ (void)SendStatusToDelegate:(id<DelegateCallBackRequestAppTrackingAuthorization>)myDelegateCallBack status:(int)myStatus
{
 	if (myDelegateCallBack && [myDelegateCallBack respondsToSelector:@selector(ResultStatus:)])
 	{
 		[myDelegateCallBack ResultStatus:myStatus];
 	}
}


+ (void)RequestAppTrackingAuthorization:(id<DelegateCallBackRequestAppTrackingAuthorization>)myDelegateCallBack
{
	if (@available(iOS 14.5, *)) 
	{
		// ATT 알림을 통한 권한 요청
		[ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus myStatus)
		{
			[self SendStatusToDelegate:myDelegateCallBack status:(int)myStatus];
			
		}];
	}
	else
	{
        [self SendStatusToDelegate:myDelegateCallBack status:0];
	}
}

@end
