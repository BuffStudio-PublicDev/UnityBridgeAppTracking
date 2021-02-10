#import "UnityBridgeAppTrackingTransparency.h"

@interface UnityBridgeAppTrackingTransparency : NSObject<DelegateCallBackRequestAppTrackingAuthorization>
@end

static UnityBridgeAppTrackingTransparency *_delegateBrigde = nil;

int IOS_GetAppTrackingAuthorizationStatus()
{
    return [UnityAppTrackingTransparency AppTrackingAuthorizationStatus];
}

void IOS_RequestAppTrackingAuthorization()
{
    if (!_delegateBrigde)
    {
        _delegateBrigde = [[UnityBridgeAppTrackingTransparency alloc] init];
    }
 
    [UnityAppTrackingTransparency RequestAppTrackingAuthorization:_delegateBrigde];
}

@implementation UnityBridgeAppTrackingTransparency

-(void)ResultStatus:(int)myStatus
{
    NSDictionary *jsonDic = @{ @"statusCode": [NSNumber numberWithInt:myStatus] };
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:jsonDic options:kNilOptions error:nil];
    NSString* jsonDataStr = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    UnitySendMessage("AppTrackingAuthorization", "DoOnCallBackAuthorization", [jsonDataStr UTF8String]);
}

@end
