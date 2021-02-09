using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;
using Util.AD;

// Do Not Modify Class
public class AppTrackingAuthorization : MonoBehaviour
{
    #region TEST CODE

    [SerializeField]
    private Text appTrackingStatus;
    [SerializeField]
    private Text advertisingIdentifier;

    
    private void Start()
    {
        DoTestView(AppTrackingTransparency.status);
       
    }

    private void DoTestView(AppTrackingTransparency.AuthorizationStatus myStatus)
    {
        DoSetAppTrackingStatus(myStatus);
        DoSetAdvertisingIdentifier();
    }


    private void DoSetAppTrackingStatus(AppTrackingTransparency.AuthorizationStatus myAuthorizationStatus)
    {
        appTrackingStatus.text = $"{myAuthorizationStatus}";
    }

    private void DoSetAdvertisingIdentifier()
    {
        advertisingIdentifier.text = Device.advertisingIdentifier;
    }


    public void OnClick()
    {
        // Request App Tracking Authorization
        AppTrackingTransparency.RequestAppTrackingAuthorization();

    }
    #endregion


    #region AppTrackingAuthorization 
    // Do Not Modify Method because Native Call DoOnCallBackAuthorization by UnitySendMessage("AppTrackingAuthorization", "DoOnCallBackAuthorization", ...)
    private void DoOnCallBackAuthorization(string myStatusData)
    {
        AppTrackingTransparency.AppTrackingCallBackData data = JsonUtility.FromJson<AppTrackingTransparency.AppTrackingCallBackData>(myStatusData);

        DoTestView((AppTrackingTransparency.AuthorizationStatus)data.statusCode);
     
    }

    // Do Not Modify Method, AppTrackingTransparency Call
    public void OnCallBackAuthorizationForNoneIOS(AppTrackingTransparency.AuthorizationStatus myStatus)
    {
        DoTestView(myStatus);
  
    }
    #endregion

}
