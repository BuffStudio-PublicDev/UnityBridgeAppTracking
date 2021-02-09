# UnityBridgeAppTracking
IOS 14 App Tracking Transparency Bridge For Unity

## Build Environment

Unity 2018.x / Unity 2019.x

Xcode 12.3

## How to Use
1. Modify your NSUserTrackingUsageDescription in NSUserTrackingUsageDescription.txt
```
Assets/10.Tools/AppTrackingTransparency/Editor/NSUserTrackingUsageDescription.txt
```
So, Unity PostprocessBuildScript insert NSUserTrackingUsageDescription to Info.plist.

2. Modify Multi Language NSUserTrackingUsageDescription And App Name.
Examples to Base, Korean, Chinese(Simple), Chinese(Traditional), English, Japanese
```
Assets/10.Tools/NativeLocale/iOS/Base.lproj/InfoPlist.strings
Assets/10.Tools/NativeLocale/iOS/ko.lproj/InfoPlist.strings
Assets/10.Tools/NativeLocale/iOS/zh-Hans.lproj/InfoPlist.strings
Assets/10.Tools/NativeLocale/iOS/zh-Hant.lproj/InfoPlist.strings
Assets/10.Tools/NativeLocale/iOS/en.lproj/InfoPlist.strings
Assets/10.Tools/NativeLocale/iOS/ja.lproj/InfoPlist.strings
```
option)
If you do not want to use Multi Language App Name, remove CFBundleDisplayName, CFBundleName in InfoPlist.strings

3. Attach AppTrackingAuthorization.prefab in your Scene.
```
Assets/03.Prefebs/AppTrackingAuthorization.prefab
```
4. Modify AppTrackingAuthorization.cs in your flow.
```
Assets/02.Scripts/AppTrackingTransparency/AppTrackingAuthorization.cs
```
- Request App Trackin Authorization
```
AppTrackingTransparency.RequestAppTrackingAuthorization();
```
- Request Call back 
```
public void OnCallBackAuthorizationForNoneIOS(AppTrackingTransparency.AuthorizationStatus myStatus)
```
- Check current App Tracking Authorization Status
```
AppTrackingTransparency.status
```
5. Build Unity Xcode Project and Build Xcode and Test!

## This UnityBridgeAppTracking use UnityAppNameLocalizationForIOS

https://github.com/zeyangl/UnityAppNameLocalizationForIOS

## IOS 14 App Tracking Transparency Documents

https://developer.apple.com/documentation/apptrackingtransparency

https://developer.apple.com/documentation/adsupport/asidentifiermanager/1614151-advertisingidentifier?language=objc
