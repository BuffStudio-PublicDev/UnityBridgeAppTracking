using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEditor.iOS.Xcode;

public class XcodeSettingsPostProcesser
{
    [PostProcessBuild(9999)]
    public static void OnPostprocessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            OnIOSBuild(target, path);
        }
    }

    private static void OnIOSBuild(BuildTarget target, string path)
    {
        DoAddAppTrackingFramework(path);
        DoAddNSUserTrackingUsageDescriptionToInfo(path);

        NativeLocale.AddLocalizedStringsIOS(path, Path.Combine(Application.dataPath, "10.Tools/NativeLocale/iOS"));
    }

    private static void DoAddAppTrackingFramework(string projectPath)
    {
        string pbxProjectPath = PBXProject.GetPBXProjectPath(projectPath);
        PBXProject pbxProj = new PBXProject();
        pbxProj.ReadFromFile(pbxProjectPath); 

#if UNITY_2019_3_OR_NEWER
        string target = pbxProj.GetUnityFrameworkTargetGuid();
#else
        string target = originalProj.TargetGuidByName("Unity-iPhone"); 
#endif
        pbxProj.AddFrameworkToProject(target, "AppTrackingTransparency.framework", false);
        pbxProj.WriteToFile(pbxProjectPath);
        
    }

    private static void DoAddNSUserTrackingUsageDescriptionToInfo(string projectPath)
    {
        // NSUserTrackingUsageDescription
        string Key = @"NSUserTrackingUsageDescription";
        string infoPlistPath = Path.Combine(projectPath, "Info.plist");

        PlistDocument plistDoc = new PlistDocument();
        plistDoc.ReadFromFile(infoPlistPath);
        if (plistDoc.root != null)
        {
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath("Assets/10.Tools/AppTrackingTransparency/Editor/NSUserTrackingUsageDescription.txt", typeof(TextAsset)) as TextAsset;
            
            plistDoc.root.SetString(Key, textAsset.text);
            plistDoc.WriteToFile(infoPlistPath);
        }
        else
        {
            Debug.LogError("Error: Can't open " + infoPlistPath);
        }

    }

}
