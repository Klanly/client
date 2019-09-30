using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFramework;
using MuGame;
public class PackAllAsset : Editor
{

    public const string MOTHER_PACKAGE_PATH = "Assets/AB_RELEASE/mother_package";
    public const string PATCH_PATH = "Assets/AB_RELEASE/patch_";
    public const string AB_RELEASE_PATH = "Assets/AB_RELEASE/";

    static public bool checkPlatform(RuntimePlatform rp, bool tip = true)
    {
        bool bright_platform = false;
        switch (rp)
        {
            case RuntimePlatform.Android:
                {
#if UNITY_ANDROID
                    bright_platform = true;
#endif
                }
                break;
            case RuntimePlatform.IPhonePlayer:
                {
#if UNITY_IPHONE
                    bright_platform = true;
#endif
                }
                break;
            case RuntimePlatform.WindowsPlayer:
                {
#if UNITY_STANDALONE_WIN
                    bright_platform = true;
#endif
                }
                break;
        }

        if (!bright_platform && tip)
        {
            switch (rp)
            {
                case RuntimePlatform.Android: EditorUtility.DisplayDialog("提示", "请手动切换到Android平台下进行导出", "确定"); break;
                case RuntimePlatform.IPhonePlayer: EditorUtility.DisplayDialog("提示", "请手动切换到IOS平台下进行导出", "确定"); break;
                case RuntimePlatform.WindowsPlayer: EditorUtility.DisplayDialog("提示", "请手动切换到Windows平台下进行导出", "确定"); break;
            }
        }

        return bright_platform;
    }

    [MenuItem("Assets/AssetBundle/清除标签")]
    public static void ClearAllAssetBundlesName()
    {

        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);

        foreach (Object obj in SelectedAsset)
        {
            string out_dir = AssetDatabase.GetAssetPath(obj);


            AssetImporter importer = AssetImporter.GetAtPath(out_dir);
            importer.assetBundleName = null;


            ClearAssetBundlesName(out_dir);
        }

    }

    [MenuItem("AssetBundle/Clear All Assetbundle Name")]
    public static void ClearAssetBundlesName()
    {
        string[] oldAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
        for (int j = 0; j < oldAssetBundleNames.Length; j++)
        {

            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
        }
    }


    public static void ClearAssetBundlesName(string path)
    {



        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;
            AssetImporter importer = AssetImporter.GetAtPath(filename);
            importer.assetBundleName = null;
        }
        foreach (string dir in dirs)
        {
            AssetImporter importer = AssetImporter.GetAtPath(dir);
            if (importer == null)
            {
                Debug.LogError("ClearAssetBundlesName:" + dir);
            }
            else
            {
                importer.assetBundleName = null;
                ClearAssetBundlesName(dir);
            }

        }
    }

    static public void autoexport()
    {
        ClearAllAssetBundlesName();
        AssetDatabase.Refresh();
        string[] aaa = Directory.GetDirectories(Application.dataPath + "/AB_RELEASE");
        foreach (string out_dir in aaa)
        {
            buildPatch("Assets" + out_dir.Substring(Application.dataPath.Length, out_dir.Length - Application.dataPath.Length).Replace('\\', '/'), true, "");
        }
    }



    

    //  [MenuItem("Assets/AssetBundle/标记所有资源")]



    static public void export_all()
    {




        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        string out_dir = null;

        foreach (Object obj in SelectedAsset)
        {
            out_dir = AssetDatabase.GetAssetPath(obj);

            if (out_dir.IndexOf(PATCH_PATH) == -1 || out_dir.IndexOf(MOTHER_PACKAGE_PATH) == -1 || !Directory.Exists(out_dir))
            {
                Debug.LogError("目录错误：" + out_dir);
                //EditorUtility.DisplayDialog("错误", "目录错误：" + out_dir + "]导出完成！", "确认");
            }

            string temp = out_dir.Substring(out_dir.LastIndexOf("/") + 1);
            if (temp.IndexOf("patch_") == -1 && temp.IndexOf("mother_package") == -1)
            {
                Debug.LogError("目录错误：" + out_dir);
                continue;
            }

            ClearAssetBundlesName(out_dir);
            buildPatch(out_dir, true, "");
        }
    }

    [MenuItem("标签管理/添加全部标签")]
    public static void buildAll()
    {
        ClearAssetBundlesName();

        build("ab_font", false, false);
        build("ab_ui", false, false);
        build("ab_fight", false, false);
        build("ab_layer", true, false);
        build("ab_audio", true, false);
        build("ab_model", true, false);
        build("ab_sprite", true, false);
        build("ab_scene", true, false);
    }

    public static void buildAllByLang(string lang)
    {
        ClearAssetBundlesName();

        build("ab_font", false, false, lang);
        build("ab_ui", false, false, lang);
        build("ab_fight", false, false, lang);
        build("ab_layer", true, false, lang);
        build("ab_audio", true, false, lang);
        build("ab_model", true, false, lang);
        build("ab_sprite", true, false, lang);
        build("ab_scene", true, false, lang);
    }


    static void build(string flag, bool single, bool clearAbName = true,string lang="")
    {
        if (clearAbName)
        {
            if(single)
            {
                ClearAssetBundlesName("Assets/AB_RELEASE/mother_package/" + flag+"/");
            }
            else
            {
                AssetImporter importer = AssetImporter.GetAtPath("Assets/AB_RELEASE/mother_package/" + flag);
                importer.assetBundleName = null;
            }
        }

        if (single)
        {


           

            buildABPath("Assets/AB_RELEASE/mother_package/" + flag, "mother_package/" + flag + AppConst.ExtName, AppConst.ExtName, true, "mother_package/",lang);
        }
        else
        {
            buildABPath("Assets/AB_RELEASE/mother_package/" + flag, "mother_package/" + flag + AppConst.ExtName, AppConst.ExtName,false,"",lang);
        }
    }


    [MenuItem("标签管理/添加/添加font标签")]
    public static void buildfont()
    {
        build("ab_font", false);
        //    buildABPath("Assets/AB_RELEASE/mother_package/ab_font", "mother_package/ab_font" + AppConst.ExtName, AppConst.ExtName);
    }

    [MenuItem("标签管理/添加/添加ui部件标签")]
    public static void buildui()
    {
        build("ab_ui", false);
        //  buildABPath("Assets/AB_RELEASE/mother_package/ab_ui", "mother_package/ab_ui" + AppConst.ExtName, AppConst.ExtName);
    }
    [MenuItem("标签管理/添加/添加fight标签")]
    public static void buildFight()
    {
        build("ab_fight", false);
        //    buildABPath("Assets/AB_RELEASE/mother_package/ab_fight", "mother_package/ab_fight" + AppConst.ExtName, AppConst.ExtName);
    }



    [MenuItem("标签管理/添加/添加layer标签(ui)")]
    public static void buildLayer()
    {
        build("ab_layer", true);
        //buildABPath("Assets/AB_RELEASE/mother_package/ab_layer", "mother_package/ab_layer" + AppConst.ExtName, AppConst.ExtName, true, "mother_package/");
    }
    [MenuItem("标签管理/添加/添加audio标签")]
    public static void buildAudio()
    {
        build("ab_audio", true);
        //  buildABPath("Assets/AB_RELEASE/mother_package/ab_audio", "mother_package/ab_audio" + AppConst.ExtName, AppConst.ExtName, true, "mother_package/");
    }

    [MenuItem("标签管理/添加/添加Model标签")]
    public static void buildModel()
    {
        build("ab_model", true);
        //    buildABPath("Assets/AB_RELEASE/mother_package/ab_model", "mother_package/ab_model" + AppConst.ExtName, AppConst.ExtName, true, "mother_package/");
    }

    [MenuItem("标签管理/添加/添加sprite标签")]
    public static void buildSprite()
    {
        build("ab_sprite", true);
        //  buildABPath("Assets/AB_RELEASE/mother_package/ab_sprite", "mother_package/ab_sprite" + AppConst.ExtName, AppConst.ExtName, true, "mother_package/");
    }


    [MenuItem("标签管理/添加/添加scene标签")]
    public static void buildScene()
    {
        build("ab_scene", true);
        //  buildABPath("Assets/AB_RELEASE/mother_package/ab_scene", "mother_package/ab_scene" + AppConst.ExtName, AppConst.ExtName, true, "mother_package/");
    }


    // [MenuItem("Assets/AssetBundle/标记除场景外的所有资源")]
    static private void export_no_scene()
    {
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        string out_dir = null;

        foreach (Object obj in SelectedAsset)
        {
            out_dir = AssetDatabase.GetAssetPath(obj);

            if (out_dir.IndexOf(PATCH_PATH) == -1 || out_dir.IndexOf(MOTHER_PACKAGE_PATH) == -1 || !Directory.Exists(out_dir))
            {
                Debug.LogError("目录错误：" + out_dir);
                //EditorUtility.DisplayDialog("错误", "目录错误：" + out_dir + "]导出完成！", "确认");
            }

            string temp = out_dir.Substring(out_dir.LastIndexOf("/") + 1);
            if (temp.IndexOf("patch_") == -1 && temp.IndexOf("mother_package") == -1)
            {
                Debug.LogError("目录错误：" + out_dir);
                continue;
            }

            ClearAssetBundlesName(out_dir);
            buildPatch(out_dir, false, "");
        }
    }



    public static void buildABPath(string path, string ab, string end_name, bool singlePack = false, string before = "",string lang="")
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }
        if (singlePack)
        {
            SetAssetBundleName(path, end_name, true, before,lang);
        }
        else
        {

            AssetImporter importer = AssetImporter.GetAtPath(path);
            importer.assetBundleName = ab;
        }

    }



    static void buildPatch(string path, bool has_scene, string lan)
    {
        int idx = path.IndexOf(AB_RELEASE_PATH) + AB_RELEASE_PATH.Length;
        string ver = path.Substring(idx, path.Length - idx);
        if (!path.EndsWith("/"))
            path = path + "/";

        //buildABPath(path + "ab_ui_shared", ver + "/ab_ui_shared" + AppConst.ExtName, AppConst.ExtName);


        buildABPath(path + "ab_font", ver + "/ab_font" + AppConst.ExtName, AppConst.ExtName);
        buildABPath(path + "ab_ui", ver + "/ab_ui" + AppConst.ExtName, AppConst.ExtName);
        buildABPath(path + "ab_fight", ver + "/ab_fight" + AppConst.ExtName, AppConst.ExtName);

        buildABPath(path + "ab_layer", ver + "/ab_layer" + AppConst.ExtName, AppConst.ExtName, true, ver + "/");
        buildABPath(path + "ab_audio", ver + "/ab_audio" + AppConst.ExtName, AppConst.ExtName, true, ver + "/");
        buildABPath(path + "ab_model", ver + "/ab_model" + AppConst.ExtName, AppConst.ExtName, true, ver + "/");
        buildABPath(path + "ab_sprite", ver + "/ab_sprite" + AppConst.ExtName, AppConst.ExtName, true, ver + "/");

        if (has_scene)
        {
            buildABPath(path + "ab_scene", ver + "/ab_scene" + AppConst.ExtName, AppConst.ExtName, true, ver + "/");
        }
    }


    public static void SetAssetBundleName(string path, string end_name, bool single = false, string before = "", string lang = "")
    {

        if (Directory.Exists(path))
        {
            string[] str = Directory.GetFileSystemEntries(path);
            foreach (string d in str)
            {
                SetAssetBundleName(d, end_name, single, before, lang);
            }
            return;
        }

        if (!File.Exists(path))
            return;

        FileInfo fileInfo = new FileInfo(path);

        if (fileInfo.Name.EndsWith(".meta") || fileInfo.Name.EndsWith(".manifest"))   //判断去除掉扩展名为“.meta”的文件
            return;


        if(lang=="")
        {
#if zh_tw
            string last = path.Replace("Assets/AB_RELEASE/mother_package/", "");
            string tempstr = "Assets/AB_RELEASE/zh_tw/" + last;
            if (File.Exists(tempstr))
            {
                fileInfo = new FileInfo(tempstr);
            }
#endif
        }
        else
        {
            string last = path.Replace("Assets/AB_RELEASE/mother_package/", "");
            string tempstr = "Assets/AB_RELEASE/"+ lang + "/" + last;
            if (File.Exists(tempstr))
            {
                fileInfo = new FileInfo(tempstr);
            }
        }
        // EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 1f * i / files.Length);


        string basePath = "Assets" + fileInfo.FullName.Substring(Application.dataPath.Length);  //编辑器下路径Assets/..
                                                                                                //string assetName = fileInfo.FullName.Replace('\\', '/');   //注意此处的斜线一定要改成反斜线，否则不能设置名称
        string assetName = "";
        if (single)
        {
         



            assetName = fileInfo.Name;
            int lastidx1 = assetName.LastIndexOf('.');
            assetName = before + assetName.Substring(0, lastidx1) + end_name;
        }
        else
        {
            assetName = path.Substring(AB_RELEASE_PATH.Length);  //预设的Assetbundle名字，带上一级目录名称
            int lastidx1 = assetName.LastIndexOf('.');
            int lastidx2 = assetName.LastIndexOf('/');

            int lastidx = lastidx1 > lastidx2 ? lastidx1 : lastidx2;
            if (lastidx == -1)
                return;


            assetName = before + assetName.Substring(2, lastidx1 - 2);
        }






        debug.Log("::" + assetName);
        AssetImporter importer = AssetImporter.GetAtPath(basePath);

        if (importer && importer.assetBundleName != assetName)
        {
            //importer.SaveAndReimport();
            importer.assetBundleName = assetName;
        }

    }


}
