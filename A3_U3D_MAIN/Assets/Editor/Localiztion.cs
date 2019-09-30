using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Cross;
using System.Xml;
using System.Text.RegularExpressions;
using SimpleFramework;

public class Localiztion : Editor
{

    public static int state = 0;


    //在这里找出你当前工程所有的场景文件，假设你只想把部分的scene文件打包 那么这里可以写你的条件判断 总之返回一个字符串数组。
    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
                names.Add(e.path);
        }
        return names.ToArray();
    }
    [MenuItem("荣耀军团/查看当前语言版本")]
    static void checkLang()
    {

#if zh_tw

EditorUtility.DisplayDialog("","当前语言版本为:zh_tw", "确定");
        return;
#endif

        EditorUtility.DisplayDialog("", "当前语言版本为:zh_cn", "确定");
    }
    [MenuItem("荣耀军团/切换/切换到zh_cn")]
    static void changeZh_CN()
    {
        if (!EditorUtility.DisplayDialog("", "切换到zh_cn?", "确定", "取消"))
            return;

        state = 0;
        changeToAndroid("zh_cn");
    }
    [MenuItem("荣耀军团/切换/切换到zh_tw")]
    static void changeZh_tw()
    {
        if (!EditorUtility.DisplayDialog("", "切换到zh_tw?", "确定", "取消"))
            return;

        state = 0;
        changeToAndroid("zh_tw");
    }


    [MenuItem("荣耀军团/导出/导出zh_cn")]
    static void buildZh_CN()
    {
        if (!EditorUtility.DisplayDialog("", "导出zh_cn?", "确定", "取消"))
            return;

        state = 1;
        BuildForAndroid("zh_cn");
    }


    [MenuItem("荣耀军团/导出/导出zh_tw")]
    static void buildzh_tw()
    {
        if (!EditorUtility.DisplayDialog("", "导出zh_tw?", "确定", "取消"))
            return;

        state = 1;
        BuildForAndroid("zh_tw");
    }

    static public string curlan = "";
    static void BuildForAndroid(string projectName)
    {
        Main m = GameObject.Find("sdkmain").GetComponent<Main>();
        if (m.debugMode != Main.ENUM_DEBUG_STATE.none_debug)
        {
            if (!EditorUtility.DisplayDialog("提示", "目前是debug状态，点击确认继续", "确认", "取消"))
            {
                return;
            }
        }

        if (m.CurPlatform != ENUM_QSMY_PLATFORM.QSPF_LINKSDK)
        {
            if (!EditorUtility.DisplayDialog("提示", "目前导出版本不含sdk登录框，点击确认继续", "确认", "取消"))
            {
                return;
            }
        }


        changeToAndroid(projectName);
        string path = Application.dataPath + "/../../" + projectName + ".apk";
        BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.Android, BuildOptions.None);
        unbackup();
    }

    public static void changeToAndroid(string projectName)
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, projectName);
        if (state == 1)
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, projectName);
        else if (state == 2)
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, projectName);
        else
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, projectName);

        AssetDatabase.Refresh();



        

        string filename = projectName;
        curlan = filename;


      
        Function.CopyDirectory(Application.dataPath + "/../../local/" + filename + "/OutAssets/", Application.dataPath + "/../../OutAssets/");

       


        Function.DeleteFolder(Application.dataPath + "/Plugins/Android/");
        Function.CopyDirectory(Application.dataPath + "/../../local/all/Plugins/", Application.dataPath + "/Plugins/");



        if (state == 1)
        {
            if (projectName != "debug")
                Function.CopyDirectory(Application.dataPath + "/../../local/" + filename + "/plugin/", Application.dataPath + "/Plugins/");
            Function.CopyDirectory(Application.dataPath + "/../../local/" + filename + "/Libs/", Application.dataPath + "/Libs/");

        }

        AssetDatabase.Refresh();

        FileStream fs = new FileStream(Application.dataPath + "/../../local/" + filename + "/config.xml", FileMode.Open);
        if (fs == null)
            return;

        int len = (int)fs.Length;
        byte[] data = new byte[len];
        fs.Read(data, 0, len);
        fs.Close();
        ByteArray ba = new ByteArray(data);
        XmlDocument xmlDoc = new XmlDocument();
        string xmlstr = ba.readUTF8Bytes(len);
        try
        {
            xmlDoc.LoadXml(xmlstr);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("表格式错误");
            return;
        }
        XmlNode node = xmlDoc.DocumentElement.ChildNodes[0];
        foreach (XmlAttribute attr in node.Attributes)
        {
            if (attr.Name == "productname")
            {
                PlayerSettings.productName = attr.Value;
            }
            else if (attr.Name == "companyname")
            {
                PlayerSettings.companyName = attr.Value;
            }
        }



        //unbackup();
        //backup();

        //if (Directory.Exists(Application.dataPath + "/temp/local/" + filename + "/"))
        //    Function.CopyDirectory(Application.dataPath + "/temp/local/" + filename + "/", Application.dataPath + "/QSMY/interface/uiResource/local/" + filename + "/");

        PackAllAsset.buildAllByLang(projectName);


        BuildOutAssetsXml.ProcessAllXML(false, projectName);
        AssetDatabase.Refresh();

        if (state == 1)
            Packager.BuildAssetResource(BuildTarget.Android, false);
        else
            Packager.buildAndroidAsset();


        //if (state == 1)
        //{
        //    if (projectName == "zh_cn")
        //        PlayerSettings.bundleIdentifier = "com.zijie.demo";
        //}



    }




    public static void backup()
    {
        if (Directory.Exists(Application.dataPath + "/temp/"))
            Function.DeleteFolder(Application.dataPath + "/temp/");

        //Function.MoveDirectory(Application.dataPath + "/StreamingAssets/OutAssets/svrconfig/", Application.dataPath + "/temp/svrconfig/");
        //Function.MoveDirectory(Application.dataPath + "/StreamingAssets/OutAssets/gconf/", Application.dataPath + "/temp/gconf/");


        Function.MoveDirectory(Application.dataPath + "/QSMY/interface/uiResource/local/", Application.dataPath + "/temp/local/");
        //  Function.MoveDirectory(Application.dataPath + "/QSMY/interface/resources/prefab/local/", Application.dataPath + "/temp/prefab/local/");

    }


    public static void unbackup()
    {
        if (Directory.Exists(Application.dataPath + "/temp/"))
        {

            //Function.MoveDirectory(Application.dataPath + "/temp/svrconfig/", Application.dataPath + "/StreamingAssets/OutAssets/svrconfig/");
            //Function.MoveDirectory(Application.dataPath + "/temp/gconf/", Application.dataPath + "/StreamingAssets/OutAssets/gconf/");

            Function.MoveDirectory(Application.dataPath + "/temp/local/", Application.dataPath + "/QSMY/interface/uiResource/local/");
            //  Function.MoveDirectory(Application.dataPath + "/temp/prefab/local/", Application.dataPath + "/QSMY/interface/resources/prefab/local/");

            // Directory.Delete(Application.dataPath + "/temp/prefab/local/");
            Function.DeleteFolder(Application.dataPath + "/temp/");
            Directory.Delete(Application.dataPath + "/temp/");
        }


    }





}
