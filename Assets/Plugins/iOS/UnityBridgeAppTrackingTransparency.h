#import "UnityAppTrackingTransparency.h"

extern "C"
{
 	int IOS_GetAppTrackingAuthorizationStatus();
 	void IOS_RequestAppTrackingAuthorization();
}
