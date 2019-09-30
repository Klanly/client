#if UNITY_IPHONE

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

public static class BuildiOSPlayer
{
    internal static void CopyAndReplaceDirectory(string srcPath, string dstPath)
    {
        if (Directory.Exists(dstPath))
            Directory.Delete(dstPath);
        if (File.Exists(dstPath))
            File.Delete(dstPath);

        Directory.CreateDirectory(dstPath);

        foreach (var file in Directory.GetFiles(srcPath))
            File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));

        foreach (var dir in Directory.GetDirectories(srcPath))
            CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
    }

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            PBXProject pbxProj = new PBXProject();

            pbxProj.ReadFromString(File.ReadAllText(projPath));
            string target = pbxProj.TargetGuidByName("Unity-iPhone");

            //// システムのフレームワークを追加
            //proj.AddFrameworkToProject(target, "AssetsLibrary.framework", false);

            //// 自前のフレームワークを追加
            //CopyAndReplaceDirectory("Assets/Lib/mylib.framework", Path.Combine(path, "Frameworks/mylib.framework"));
            //proj.AddFileToBuild(target, proj.AddFile("Frameworks/mylib.framework", "Frameworks/mylib.framework", PBXSourceTree.Source));

            //// ファイルを追加
            //var fileName = "my_file.xml";
            //var filePath = Path.Combine("Assets/Lib", fileName);
            //File.Copy(filePath, Path.Combine(path, fileName));
            //proj.AddFileToBuild(target, proj.AddFile(fileName, fileName, PBXSourceTree.Source));

            //// Yosemiteでipaが書き出せないエラーに対応するための設定
            //proj.SetBuildProperty(target, "CODE_SIGN_RESOURCE_RULES_PATH", "$(SDKROOT)/ResourceRules.plist");

            //// フレームワークの検索パスを設定・追加
            //proj.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
            //proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks");

            //proj.AddBuildProperty(target, "Other Linker Flags", "-ObjC");
            pbxProj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
            pbxProj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(SRCROOT)/Libraries");


            // 書き出し
            //File.WriteAllText(projPath, pbxProj.WriteToString());
            pbxProj.WriteToFile(projPath);


            EditUnityAppController(pathToBuiltProject);

            //清理Android下的资源
            if (Directory.Exists(pathToBuiltProject + "/Libraries/Plugins/Android"))
                Directory.Delete(pathToBuiltProject + "/Libraries/Plugins/Android", true);

            //foreach (string file in Directory.GetFiles(destDirName, "*.*", SearchOption.AllDirectories))
            //    pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile(file, file.Replace(pathToBuiltProject + "/", ""), PBXSourceTree.Source));
        }
    }


    static void EditUnityAppController(string pathToBuiltProject)
    {
        string unityAppControllerPath = pathToBuiltProject + "/Classes/UnityAppController.mm";
        if (File.Exists(unityAppControllerPath))
        {
           // string headerCode = "#import <SMPCQuickSDK/SMPCQuickSDK.h>\n" +
           //                     "#import \"QuickSDK_ios.h\"\n\n";
           // string unityAppController = headerCode + File.ReadAllText(unityAppControllerPath);

           // //Match match = Regex.Match(unityAppController, @"- \(void\)startUnity:\(UIApplication\*\)application\s+\{{FNXX==XXFN}+\}");
           // //if (match.Success)
           // //{
           // //    string newCode = match.Groups[0].Value.Remove(match.Groups[0].Value.Length - 1);
           // //    newCode += "\n" +
           // //               "    [[AVAudioSession sharedInstance] setCategory: AVAudioSessionCategoryPlayback error: nil];\n" +
           // //               "    [[AVAudioSession sharedInstance] setActive:YES error:nil];\n" +
           // //               "}\n\n" +
           // //               "- (void)application:(UIApplication*)application performActionForShortcutItem: (UIApplicationShortcutItem*)shortcutItem completionHandler: (void(^)(BOOL))completionHandler\n" +
           // //               "{\n" +
           // //               "    [[SDKPlatform share] performActionForShortcutItem:shortcutItem];\n" +
           // //               "}";
           // //    unityAppController = unityAppController.Replace(match.Groups[0].Value, newCode);
           // //}

           // //test  Product_Key and Product_Code: 
           // //string newCode = "\n" +
           // //           "//regist listener for quicksdk\n" +
           // //           "[[QuickSDK_ios shareInstance] addNotifications];\n" +
           // //           "//init quicksdk\n" +
           // //           "SMPCQuickSDKInitConfigure* cfg = [[SMPCQuickSDKInitConfigure alloc] init];\n" +
           // //           "cfg.productKey = @\"82293239\";\n" +
           // //           "cfg.productCode = @\"15741094574158973297877541456257\";\n" +
           // //           "int error = [[SMPCQuickSDK defaultInstance] initWithConfig:cfg application:application didFinishLaunchingWithOptions:launchOptions];\n" +
           // //           "if (error !=0){\n" +
           // //           "NSLog(@\"can not init quick sdk: % d\", error);\n" +
           // //           "}" + "\n\n\n// if you wont use keyboard you may comment it out at save some memory";

           // string newCode = "\n" +
           //"//regist listener for quicksdk\n" +
           //"[[QuickSDK_ios shareInstance] addNotifications];\n" +
           //"//init quicksdk\n" +
           //"SMPCQuickSDKInitConfigure* cfg = [[SMPCQuickSDKInitConfigure alloc] init];\n" +
           //"cfg.productKey = @\"89091971\";\n" +
           //"cfg.productCode = @\"05041187040340104293832071585395\";\n" +
           //"int error = [[SMPCQuickSDK defaultInstance] initWithConfig:cfg application:application didFinishLaunchingWithOptions:launchOptions];\n" +
           //"if (error !=0){\n" +
           //"NSLog(@\"can not init quick sdk: % d\", error);\n" +
           //"}" + "\n\n\n// if you wont use keyboard you may comment it out at save some memory";

           // unityAppController = unityAppController.Replace("// if you wont use keyboard you may comment it out at save some memory", newCode);

           // File.WriteAllText(unityAppControllerPath, unityAppController);
        }
    }

}

#endif