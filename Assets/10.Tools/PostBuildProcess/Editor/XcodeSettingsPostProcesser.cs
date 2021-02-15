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
        DoAddSKAdNetworkItemsToInfo(path);
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

    private static void DoAddSKAdNetworkItemsToInfo(string projectPath)
    {
        string infoPlistPath = Path.Combine(projectPath, "Info.plist");

        PlistDocument plistDoc = new PlistDocument();
        plistDoc.ReadFromFile(infoPlistPath);
        if (plistDoc.root != null)
        {

            PlistElementDict rootDict = plistDoc.root;

            PlistElement elementSKAdNetworkItems = rootDict["SKAdNetworkItems"];

            if (null == elementSKAdNetworkItems)
            {
                rootDict["SKAdNetworkItems"] = new PlistElementArray();

                elementSKAdNetworkItems = rootDict["SKAdNetworkItems"];
            }
            else
            {
                PlistElementArray arrayAdNetworkItems = elementSKAdNetworkItems.AsArray();
                int count = arrayAdNetworkItems.values.Count;
                arrayAdNetworkItems.values.RemoveRange(0, count);
            }

            PlistElementArray arrayItems = elementSKAdNetworkItems.AsArray();

            PlistElementArray addAdNetworks = DoGetAdNetworks();

            for (int i = 0; i < addAdNetworks.values.Count; i++)
            {
                PlistElementDict item = addAdNetworks.values[i] as PlistElementDict;
                arrayItems.values.Add(item);
            }

            plistDoc.WriteToFile(infoPlistPath);
        }
        else
        {
            Debug.LogError("Error: Can't open " + infoPlistPath);
        }

    }

    private static PlistElementArray DoGetAdNetworks()
    {

        string adNetworksPlistPath = Path.Combine(DoGetProjectFolder(), "Assets/10.Tools/AppTrackingTransparency/Editor/SKAdNetworkItems.plist");

        PlistDocument plistDoc = new PlistDocument();
        plistDoc.ReadFromFile(adNetworksPlistPath);

        PlistElement elementSKAdNetworkItems = plistDoc.root["SKAdNetworkItems"];

        if (null == elementSKAdNetworkItems)
        {
            plistDoc.root["SKAdNetworkItems"] = new PlistElementArray();

            elementSKAdNetworkItems = plistDoc.root["SKAdNetworkItems"];
        }

        PlistElementArray arrayItems = elementSKAdNetworkItems.AsArray();

        return arrayItems;


    }

    private static string DoGetProjectFolder()
    {
        int position = Application.dataPath.IndexOf("/Assets");
        string projectPath = Application.dataPath.Substring(0, position);

        return projectPath;
    }

}
