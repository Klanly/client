using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using SimpleFramework;
using System.Diagnostics;
using System.Threading;
class ProjectBuild : Editor
{


   
    public static string getpram()
    {
        string[] l = System.Environment.GetCommandLineArgs();
        return l[l.Length-1];
    }





    [MenuItem("外部工具/【闲人勿用】同步资源到内网服务器上")]
    public static void testc()
    {
        if(getpram()!="1")
        if (!EditorUtility.DisplayDialog("提示", "该操作不可逆，是否要继续", "继续", "停止"))
            return;
        bool b = false;
#if UNITY_STANDALONE_WIN
                    b = true;
#endif
        if (!b)
        {
          
                EditorUtility.DisplayDialog("提示", "请切换到windows下使用", "确定");
            return;
        }



        string asset = Application.dataPath.Replace('/', '\\');
        string path = asset + "\\..\\..\\cmd\\bin\\";

        Run(path + "FtpToEdi.bat", asset + " " + path);


        EditorUtility.DisplayDialog("提示", "任务完成！", "确定");
    }


    [MenuItem("外部工具/【闲人勿用】导出资源并同步到内网服务器上")]
    static void BuildWindowsResource()
    {
        if (getpram() != "1")
            if (!EditorUtility.DisplayDialog("提示", "该操作不可逆，是否要继续", "继续", "停止"))
            return;

        bool b = false;
#if UNITY_STANDALONE_WIN
                    b = true;
#endif
        if (!b)
        {
            
                EditorUtility.DisplayDialog("提示", "请切换到windows下使用", "确定");
            return;
        }

        Packager.BuildWindowsResource();
        string asset = Application.dataPath.Replace('/', '\\');
        string path = asset + "\\..\\..\\cmd\\bin\\";

        Run(path + "FtpToEdi.bat", asset + " " + path);


        EditorUtility.DisplayDialog("提示", "任务完成！", "确定");
    }

    [MenuItem("外部工具/【闲人勿用】全自动同步到内网服务器上")]
    public static void SVNBuildWindowsResource()
    {
        if (getpram() != "1")
            if (!EditorUtility.DisplayDialog("提示", "该操作不可逆，是否要继续", "继续", "停止"))
            return;

        bool b = false;
#if UNITY_STANDALONE_WIN
                    b = true;
#endif
        if (!b)
        {
            EditorUtility.DisplayDialog("提示", "请切换到windows下使用", "确定");
            return;
        }
        string asset = Application.dataPath.Replace('/', '\\');

        Run(asset + "\\..\\..\\cmd\\bin\\svn.bat", asset);
        AssetDatabase.Refresh();

        PackAllAsset.autoexport();
        AssetDatabase.Refresh();
        Packager.BuildWindowsResource();

        string path = asset + "\\..\\..\\cmd\\bin\\";
        Run(path + "FtpToEdi.bat", asset + " " + path);
        EditorUtility.DisplayDialog("提示", "任务完成！", "确定");
    }



    static void Run(string path, string command = "")
    {
        Process pro = new Process();
        FileInfo file = new FileInfo(path);
        pro.StartInfo.WorkingDirectory = file.Directory.FullName;
        pro.StartInfo.FileName = path;
        if (command != "")
            pro.StartInfo.Arguments = command;
        pro.StartInfo.CreateNoWindow = false;
        pro.Start();
        pro.WaitForExit();
    }



    private static string RunCmd(string command)
    {
        //例Process
        Process p = new Process();
        p.StartInfo.FileName = "cmd.exe";           //确定程序名
        p.StartInfo.Arguments = "/c " + command;    //确定程式命令行
        p.StartInfo.UseShellExecute = false;        //Shell的使用
        p.StartInfo.RedirectStandardInput = true;   //重定向输入
        p.StartInfo.RedirectStandardOutput = true; //重定向输出
        p.StartInfo.RedirectStandardError = true;   //重定向输出错误
        p.StartInfo.CreateNoWindow = true;          //设置置不显示示窗口
        p.Start();
        return p.StandardOutput.ReadToEnd();        //输出出流取得命令行结果果
    }
}
