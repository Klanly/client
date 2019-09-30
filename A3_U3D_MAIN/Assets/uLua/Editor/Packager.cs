using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleFramework;
using MuGame;

public class Packager
{
    public static string platform = string.Empty;
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();

    /// <summary>
    /// 载入素材
    /// </summary>
    static UnityEngine.Object LoadAsset(string file)
    {
        if (file.EndsWith(".lua")) file += ".txt";
        return AssetDatabase.LoadMainAssetAtPath("Assets/Builds/" + file);
    }

    [MenuItem("Game/清理重新导出ab/Build iPhone Resource", false, 11)]
    public static void BuildiPhoneResource()
    {
        if (!EditorUtility.DisplayDialog("提示", "该操作将重新导出所欲资源（需要花费较长时间）", "继续", "停止"))
            return;

        if (PackAllAsset.checkPlatform(RuntimePlatform.IPhonePlayer))
        {
            BuildTarget target;
            target = BuildTarget.iOS;
            BuildAssetResource(target, false);
        }
        else
        {
            EditorUtility.DisplayDialog("Error!", "请切换平台", "确定");
        }
    }

    [MenuItem("Game/清理重新导出ab/Build Android Resource", false, 12)]
    public static void BuildAndroidResource()
    {
        if (!EditorUtility.DisplayDialog("提示", "该操作将重新导出所欲资源（需要花费较长时间）", "继续", "停止"))
            return;
        BuildOutAssetsXml.Build_OutAssetsXmlnodesc();
        if (PackAllAsset.checkPlatform(RuntimePlatform.Android))
        {
            BuildAssetResource(BuildTarget.Android, true);
        }
        else
        {
            EditorUtility.DisplayDialog("Error!", "请切换平台", "确定");
        }
    }

    [MenuItem("Game/清理重新导出ab/Build Windows Resource", false, 13)]
    public static void BuildWindowsResource()
    {
        if (!EditorUtility.DisplayDialog("提示", "该操作将重新导出所欲资源（需要花费较长时间）", "继续", "停止"))
            return;

        if (PackAllAsset.checkPlatform(RuntimePlatform.WindowsPlayer))
        {
            BuildAssetResource(BuildTarget.StandaloneWindows, true);
        }
        else
        {
            EditorUtility.DisplayDialog("Error!", "请切换平台", "确定");
        }
    }

    [MenuItem("Game/编译uLua代码")]
    public static void BuildULua2Res()
    {
        BuildULuaRes(true);
    }

    [MenuItem("荣耀军团/导出当前android更新包")]
    static void buildAndroidUpdate()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
      
        BuildAndroidResource();
        //EditorUtility.DisplayProgressBar("删除中", "删除中", 0.4f);
        //Util.DeleteFolder(Application.dataPath + "/../../更新包/Android更新包/");
        EditorUtility.DisplayProgressBar("复制中", "复制中", 0.8f);
        Util.CopyDirectory(Application.dataPath + "/" + AppConst.AssetDirname + "/", Application.dataPath + "/../../更新包/" + Convert.ToInt64(ts.TotalSeconds) + "/android/", "", ".meta");
        EditorUtility.ClearProgressBar();

        EditorUtility.DisplayDialog("完成", "更新android包[" + Convert.ToInt64(ts.TotalSeconds) + "]导出完成！", "确认");
    }

    [MenuItem("荣耀军团/导出当前ios更新包")]
    static void buildiosUpdate()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

        BuildiPhoneResource();

        EditorUtility.DisplayProgressBar("复制中", "复制中", 0.8f);
        Util.CopyDirectory(Application.dataPath + "/" + AppConst.AssetDirname + "/", Application.dataPath + "/../../更新包/" + Convert.ToInt64(ts.TotalSeconds) + "/ios/", "", ".meta");
        EditorUtility.ClearProgressBar();

        EditorUtility.DisplayDialog("完成", "更新ios包[" + Convert.ToInt64(ts.TotalSeconds) + "]导出完成！", "确认");
    }

    [MenuItem("荣耀军团/导出当前所有更新包")]
    static void buildAllUpdate()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

        BuildiPhoneResource();

        EditorUtility.DisplayProgressBar("复制ios中", "复制中", 0.4f);
        Util.CopyDirectory(Application.dataPath + "/" + AppConst.AssetDirname + "/", Application.dataPath + "/../../更新包/" + Convert.ToInt64(ts.TotalSeconds) + "/ios/", "", ".meta");

        BuildAndroidResource();

        EditorUtility.DisplayProgressBar("复制android中", "复制中", 0.8f);
        Util.CopyDirectory(Application.dataPath + "/" + AppConst.AssetDirname + "/", Application.dataPath + "/../../更新包/" + Convert.ToInt64(ts.TotalSeconds) + "/android/", "", ".meta");

        EditorUtility.ClearProgressBar();

        EditorUtility.DisplayDialog("完成", "更新android包[" + Convert.ToInt64(ts.TotalSeconds) + "]导出完成！", "确认");
    }

   




    [MenuItem("Game/仅同步当前assetBundle")]
    public static void buildAndroidAsset()
    {
        if (PackAllAsset.checkPlatform(RuntimePlatform.WindowsPlayer,false))
        {
            string resPath = AppDataPath + "/" + AppConst.AssetDirname + "/";
            if (!Directory.Exists(resPath)) Directory.CreateDirectory(resPath);
            buildasetBundle(BuildTarget.StandaloneWindows, resPath);
        }
        else if (PackAllAsset.checkPlatform(RuntimePlatform.Android, false))
        {
            string resPath = AppDataPath + "/" + AppConst.AssetDirname + "/";
            if (!Directory.Exists(resPath)) Directory.CreateDirectory(resPath);
            buildasetBundle(BuildTarget.Android, resPath);
        }
        else if (PackAllAsset.checkPlatform(RuntimePlatform.IPhonePlayer, false))
        {
            string resPath = AppDataPath + "/" + AppConst.AssetDirname + "/";
            if (!Directory.Exists(resPath)) Directory.CreateDirectory(resPath);
            buildasetBundle(BuildTarget.iOS, resPath);
        }
    }


    public static void buildasetBundle(BuildTarget target, string resPath)
    {
        BuildPipeline.BuildAssetBundles(resPath, BuildAssetBundleOptions.None, target);
    }
    /// <summary>
    /// 生成绑定素材
    /// </summary>
    public static void BuildAssetResource(BuildTarget target, bool isWin)
    {
        string dataPath = Util.DataPath;
        if (Directory.Exists(dataPath))
        {
            Directory.Delete(dataPath, true);
            AssetDatabase.Refresh();
        }
        string assetfile = string.Empty;  //素材文件名
        string resPath = AppDataPath + "/" + AppConst.AssetDirname + "/";
        if (!Directory.Exists(resPath))
        {
            Directory.CreateDirectory(resPath);
            AssetDatabase.Refresh();
        }
       
        if (AppConst.ExampleMode)
        {
          //  BuildPipeline.BuildAssetBundles(resPath, BuildAssetBundleOptions.None, target);

            buildasetBundle(target, resPath);
        }

        BuildULuaRes(isWin);
        //string luaPath = resPath + "/lua/";

        ////----------复制Lua文件----------------
        //if (Directory.Exists(luaPath))
        //{
        //    Directory.Delete(luaPath, true);
        //}
        //Directory.CreateDirectory(luaPath);

        //paths.Clear(); files.Clear();
        //string luaDataPath = Application.dataPath + "/lua/".ToLower();
        //Recursive(luaDataPath);
        //int n = 0;
        //foreach (string f in files)
        //{
        //    if (f.EndsWith(".meta")) continue;
        //    string newfile = f.Replace(luaDataPath, "");
        //    string newpath = luaPath + newfile;
        //    string path = Path.GetDirectoryName(newpath);
        //    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        //    if (File.Exists(newpath))
        //    {
        //        File.Delete(newpath);
        //    }
        //    if (AppConst.LuaEncode)
        //    {
        //        UpdateProgress(n++, files.Count, newpath);
        //        EncodeLuaFile(f, newpath, isWin);
        //    }
        //    else
        //    {
        //        File.Copy(f, newpath, true);
        //    }
        //}

        Util.DeleteFolder(Application.dataPath + "/" + AppConst.AssetDirname + "/OutAssets/");
        Util.CopyDirectory(Application.dataPath + "/../../OutAssets/", Application.dataPath + "/" + AppConst.AssetDirname + "/OutAssets/");


        EditorUtility.ClearProgressBar();

        refreshFileList();
    }

    private static void BuildULuaRes(bool isWin)
    {
        string resPath = AppDataPath + "/" + AppConst.AssetDirname + "/";
        string luaPath = resPath + "/lua/";

        //----------复制Lua文件----------------
        if (Directory.Exists(luaPath))
        {
            Directory.Delete(luaPath, true);
        }
        Directory.CreateDirectory(luaPath);

        paths.Clear(); files.Clear();
        string luaDataPath = Application.dataPath + "/lua/".ToLower();
        Recursive(luaDataPath);
        int n = 0;
        foreach (string f in files)
        {
            if (f.EndsWith(".meta")) continue;
            string newfile = f.Replace(luaDataPath, "");
            string newpath = luaPath + newfile;
            string path = Path.GetDirectoryName(newpath);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            if (File.Exists(newpath))
            {
                File.Delete(newpath);
            }
            if (AppConst.LuaEncode)
            {
                UpdateProgress(n++, files.Count, newpath);
                EncodeLuaFile(f, newpath, isWin);
            }
            else
            {
                File.Copy(f, newpath, true);
            }
        }
    }

  



    [MenuItem("Game/生成文件列表")]
    public static void refreshFileList()
    {
        string resPath = AppDataPath + "/" + AppConst.AssetDirname + "/";
        ///----------------------创建文件列表-----------------------
        string newFilePath = resPath + "/files.txt";
        if (File.Exists(newFilePath)) File.Delete(newFilePath);

        paths.Clear(); files.Clear();
        Recursive(resPath);

        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < files.Count; i++)
        {
            string file = files[i];
            string ext = Path.GetExtension(file);
            if (file.EndsWith(".meta") || file.Contains(".DS_Store") || file.Contains(".manifest")) continue;
            long size = Util.sizeFile(file);
            string md5 = Util.md5file(file);
            string value = file.Replace(resPath, string.Empty);

            bool isinstream = !file.Contains("StreamingAssets.manifest") && (file.EndsWith(AppConst.ExtName) || file.EndsWith(".manifest"))|| file.Contains(".lua");
            sw.WriteLine((isinstream ? 1 : 2) + "|" + value + "|" + md5 + "|" + size);
        }//1为包内，2为包外，3为更新

        List<string> patchDirs = new List<string>();
        string asbegin = AppConst.AssetDirname + "/patch_v";
        string[] dirs = Directory.GetDirectories(resPath);
        foreach (string dir in dirs)
        {
            if (dir.IndexOf(asbegin) < 0)
                continue;

            patchDirs.Add(dir);
        }
        patchDirs.Sort();

        Dictionary<string, List<FileInfo>> dFile = new Dictionary<string, List<FileInfo>>();
        for (int i = patchDirs.Count - 1; i >= 0; i--)
        {
            string dir = patchDirs[i];
            files.Clear();
            Recursive(dir);

            string begin = "- " + PackAllAsset.AB_RELEASE_PATH;
            foreach (string file in files)
            {
                if (!file.EndsWith(".manifest")) continue;

                string[] lines = File.ReadAllLines(file);
                foreach(string line in lines)
                {
                    if (line.IndexOf(begin) != 0) continue;

                    int idx = line.IndexOf(PackAllAsset.PATCH_PATH) + PackAllAsset.PATCH_PATH.Length;
                    string temp = line.Substring(idx, line.Length - idx);
                    idx = temp.IndexOf("/") + 1;
                    temp = temp.Substring(idx, temp.Length - idx);
                    debug.Log(dFile.ContainsKey(temp) + " " + temp);
                    if (!dFile.ContainsKey(temp))
                    {

                        dFile[temp] = new List<FileInfo>() { new FileInfo(file), new FileInfo(line.Substring(2)) };
                    }
                }


              
            }
        }


        foreach (List<FileInfo> file in dFile.Values)
        {
            //  string ext = Path.GetExtension(file);

            FileInfo abFI = file[0];
            FileInfo fileFI = file[1];

            string patch = abFI.FullName.Substring(abFI.FullName.LastIndexOf("patch_v")).Replace('\\', '/');
            patch= patch.Substring(0, patch.IndexOf(".manifest"));
            string filename = fileFI.Name.Substring(0, fileFI.Name.LastIndexOf("."));

            string path = abFI.Name.Substring(0,abFI.Name.IndexOf(".manifest"));



            sw.WriteLine("3|"+patch + "|" + path  +"|"+ filename );
        }




        sw.Close(); fs.Close();
        string newFilePath1 = resPath + "/verMd5.txt";
        if (File.Exists(newFilePath1)) File.Delete(newFilePath1);
        fs = new FileStream(newFilePath1, FileMode.CreateNew);
        sw = new StreamWriter(fs);
        sw.WriteLine(Util.md5file(newFilePath));
        sw.Close(); fs.Close();

        AssetDatabase.Refresh();

    }


    /// <summary>
    /// 数据目录
    /// </summary>
    static string AppDataPath
    {
        get { return Application.dataPath.ToLower(); }
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta") ) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }

    static void UpdateProgress(int progress, int progressMax, string desc)
    {
        string title = "Processing...[" + progress + " - " + progressMax + "]";
        float value = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, value);
    }

    static void EncodeLuaFile(string srcFile, string outFile, bool isWin)
    {
        if (!srcFile.ToLower().EndsWith(".lua"))
        {
            File.Copy(srcFile, outFile, true);
            return;
        }
        string luaexe = string.Empty;
        string args = string.Empty;
        string exedir = string.Empty;
        string currDir = Directory.GetCurrentDirectory();
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            luaexe = "luajit.exe";
            args = "-b " + srcFile + " " + outFile;
            exedir = AppDataPath.Replace("assets", "") + "LuaEncoder/luajit/";
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            luaexe = "./luac";
            args = "-o " + outFile + " " + srcFile;
            exedir = AppDataPath.Replace("assets", "") + "LuaEncoder/luavm/";
        }
        Directory.SetCurrentDirectory(exedir);
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = luaexe;
        info.Arguments = args;
        info.WindowStyle = ProcessWindowStyle.Hidden;
        info.UseShellExecute = isWin;
        info.ErrorDialog = true;
        Util.Log(info.FileName + " " + info.Arguments);

        Process pro = Process.Start(info);
        pro.WaitForExit();
        Directory.SetCurrentDirectory(currDir);
    }

    [MenuItem("Game/Build Protobuf-lua-gen File")]
    public static void BuildProtobufFile()
    {
        if (!AppConst.ExampleMode)
        {
            Debugger.LogError("若使用编码Protobuf-lua-gen功能，需要自己配置外部环境！！");
            return;
        }
        string dir = AppDataPath + "/Lua/3rd/pblua";
        paths.Clear(); files.Clear(); Recursive(dir);

        string protoc = "d:/protobuf-2.4.1/src/protoc.exe";
        string protoc_gen_dir = "\"d:/protoc-gen-lua/plugin/protoc-gen-lua.bat\"";

        foreach (string f in files)
        {
            string name = Path.GetFileName(f);
            string ext = Path.GetExtension(f);
            if (!ext.Equals(".proto")) continue;

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = protoc;
            info.Arguments = " --lua_out=./ --plugin=protoc-gen-lua=" + protoc_gen_dir + " " + name;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.UseShellExecute = true;
            info.WorkingDirectory = dir;
            info.ErrorDialog = true;
            Util.Log(info.FileName + " " + info.Arguments);

            Process pro = Process.Start(info);
            pro.WaitForExit();
        }
        AssetDatabase.Refresh();
    }
}