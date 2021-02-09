using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using XcodeProjectForLocalization = ChillyRoom.UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode;

public class NativeLocale
{
    public static void AddLocalizedStringsIOS(string projectPath, string localizedDirectoryPath)
    {        
        DirectoryInfo dir = new DirectoryInfo(localizedDirectoryPath);
        if(!dir.Exists)
            return;

        DoAddLocalizationFolder(projectPath);
        
        List<string> locales = new List<string>();
        var localeDirs = dir.GetDirectories("*.lproj", SearchOption.TopDirectoryOnly);

        foreach(var sub in localeDirs)
        {
            locales.Add(Path.GetFileNameWithoutExtension(sub.Name));
        }


        AddLocalizedStringsIOS(projectPath, localizedDirectoryPath, locales);
    }

    private static void DoAddLocalizationFolder(string projectPath)
    {
        string pbxProjectPath = UnityEditor.iOS.Xcode.PBXProject.GetPBXProjectPath(projectPath);
        UnityEditor.iOS.Xcode.PBXProject originalProj = new UnityEditor.iOS.Xcode.PBXProject();

        originalProj.ReadFromFile(pbxProjectPath);
        string localizationFolderPath = $"{projectPath}/Localization";
        if (false == Directory.Exists(localizationFolderPath))
        {
            Directory.CreateDirectory(localizationFolderPath);
        }
        originalProj.AddFolderReference(localizationFolderPath, "Localization");
        originalProj.WriteToFile(pbxProjectPath);
    }


    public static void AddLocalizedStringsIOS(string projectPath, string localizedDirectoryPath, List<string> validLocales)
    {
        string projPath = projectPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
        XcodeProjectForLocalization.PBXProject proj = new XcodeProjectForLocalization.PBXProject();
        proj.ReadFromFile(projPath);

        string variantGroupName = "InfoPlist.strings";
        proj.ClearVariantGroupEntries(variantGroupName);

        foreach (var locale in validLocales)
        {
            // copy contents in the localization directory to project directory
            string src = Path.Combine(localizedDirectoryPath, locale + ".lproj");
            DirectoryCopy(src, Path.Combine(projectPath, "Localization/" + locale + ".lproj"));

            string fileRelatvePath = string.Format("Localization/{0}.lproj/InfoPlist.strings", locale);
            
            proj.AddLocalization(variantGroupName, locale, fileRelatvePath);
        }

        proj.WriteToFile(projPath);
    }


    private static void DirectoryCopy(string sourceDirName, string destDirName)
    {
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
            return;

        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        FileInfo[] files = dir.GetFiles();

        foreach (FileInfo file in files)
        {
            // skip unity meta files
            if(file.FullName.EndsWith(".meta"))
                continue;
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, true);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        foreach (DirectoryInfo subdir in dirs)
        {
            string temppath = Path.Combine(destDirName, subdir.Name);
            DirectoryCopy(subdir.FullName, temppath);
        }
    }
}
