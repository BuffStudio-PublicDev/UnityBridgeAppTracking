using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using AOT;

namespace Util.AD
{
    public class AppTrackingTransparency
    {
        public class AppTrackingCallBackData
        {
            public int statusCode;
        }

        public enum AuthorizationStatus
        {
            notDetermined = 0,
            restricted = 1,
            denied = 2,
            authorized = 3,
        }

        public static AuthorizationStatus status { get { return DoGetAppTrackingAuthorizationStatus(); } }

        public static void RequestAppTrackingAuthorization()
        {

#if UNITY_IOS && !UNITY_EDITOR
            IOS_RequestAppTrackingAuthorization();
#else
            AppTrackingAuthorization ata = GameObject.FindObjectOfType<AppTrackingAuthorization>();
            ata.OnCallBackAuthorizationForNoneIOS(AuthorizationStatus.notDetermined);
#endif
        }

        
        private static AuthorizationStatus DoGetAppTrackingAuthorizationStatus()
        {
#if UNITY_IOS && !UNITY_EDITOR
            return (AuthorizationStatus)IOS_GetAppTrackingAuthorizationStatus();
#else
            return AuthorizationStatus.notDetermined;
#endif
        }



#if UNITY_IOS
        #region IOS Native Bridge

        [DllImport("__Internal")]
        private static extern int IOS_GetAppTrackingAuthorizationStatus();

        [DllImport("__Internal")]
        private static extern void IOS_RequestAppTrackingAuthorization();

        
#endregion

#endif
    }

}

