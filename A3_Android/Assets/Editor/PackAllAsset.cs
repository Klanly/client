using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PackAllAsset : Editor
{
    static private bool checkPlatform(RuntimePlatform rp)
    {
        bool bright_platform = false;
        switch (rp)
        {
            case RuntimePlatform.Android:
                {
#if UNITY_ANDROID
                    bright_platform = true;
#endif
                } break;
            case RuntimePlatform.IPhonePlayer:
                {
#if UNITY_IPHONE
                    bright_platform = true;
#endif
                } break;
            case RuntimePlatform.WindowsPlayer:
                {
#if UNITY_STANDALONE_WIN
                    bright_platform = true;
#endif
                } break;
        }

        if (!bright_platform)
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

    static private int Build_FileAssetBundle(string file, BuildTarget build_target)
    {
        //获取u3d资源引用地址
        string assetPath = file.Substring(0, file.LastIndexOf("."));
        assetPath = assetPath.Substring(assetPath.IndexOf("Resources") + "Resources/".Length);
        assetPath = assetPath.Replace('\\', '/');

        string outasset_dir = "/../../OutAssets/win/";
        if (BuildTarget.Android == build_target)
            outasset_dir = "/../../OutAssets/android/";

        if (BuildTarget.iPhone == build_target)
            outasset_dir = "/../../OutAssets/ios/";

        //生成AssetBundle存放地址
        string ext_name = null;
        bool copy_android_to_win = false; //将通用数据从android考份到win
        if (file.EndsWith(".prefab"))
        {
            ext_name = ".res";
        }
        else if (file.EndsWith(".exr"))
        {
            ext_name = ".lmp";
        }
        else if (file.EndsWith(".asset"))
        {
            ext_name = ".lpb";
        }
        else if (file.EndsWith(".anim"))
        {
            ext_name = ".anim";
            copy_android_to_win = true;
        }
        else if (file.EndsWith(".png") || file.EndsWith(".jpg"))
        {
            ext_name = ".pic";
        }
        else if (file.EndsWith(".mp3") || file.EndsWith(".ogg") || file.EndsWith(".wav"))
        {
            ext_name = ".snd";
            copy_android_to_win = true;
        }

        //win下的文件，如果是android靠过来的，就不生成了
        if (BuildTarget.StandaloneWindows == build_target && copy_android_to_win)
        {
            ext_name = null;
        }

        if (ext_name != null)
        {
            string dstPath = Application.dataPath + outasset_dir + assetPath + ext_name;
            //从Resources中加载资源
            Object res = Resources.Load(assetPath);
            PackAllAsset.preparePath(dstPath);
            BuildPipeline.BuildAssetBundle(res, new Object[] { res }, dstPath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, build_target);

            FileStream fs = new FileStream(dstPath, FileMode.Open);
            int len = (int)fs.Length;
            if (BuildTarget.Android == build_target && copy_android_to_win)
            {
                byte[] data = new byte[len];
                fs.Read(data, 0, len);

                string winfile = Application.dataPath + "/../../OutAssets/win/" + assetPath + ext_name;
				PackAllAsset.preparePath(winfile);
                //string str_verfilename = Application.dataPath + "/../../OutAssets/" + ver_filename;
                //byte[] file_data = System.Text.Encoding.UTF8.GetBytes(str_verfiledata);
                FileStream stream = new FileStream(winfile, FileMode.Create);
                stream.Write(data, 0, len);
                stream.Flush();
                stream.Close();
            }

            fs.Close();

            return len;
        }

        if (BuildTarget.StandaloneWindows != build_target && file.EndsWith(".unity"))
        {
            copy_android_to_win = true;

            string dstPath = Application.dataPath + outasset_dir + assetPath + ".unity";
            ////从Resources中加载资源
            //Object res = Resources.Load(assetPath);
            //PackAllAsset.preparePath(dstPath);
            //BuildPipeline.BuildAssetBundle(res, new Object[] { res }, dstPath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, build_target);

            //string[] scenes = { "Assets/scene1.unity" };
            string scenepath = "Assets/Resources/" + assetPath + ".unity";
            Debug.Log("sce path = " + scenepath);
            string[] scenes = { scenepath };

            BuildPipeline.BuildPlayer(scenes, dstPath, build_target, BuildOptions.BuildAdditionalStreamedScenes);

            FileStream fs = new FileStream(dstPath, FileMode.Open);
            int len = (int)fs.Length;
            if (BuildTarget.Android == build_target && copy_android_to_win)
            {
                byte[] data = new byte[len];
                fs.Read(data, 0, len);

                string winfile = Application.dataPath + "/../../OutAssets/win/" + assetPath + ".unity";
				PackAllAsset.preparePath(winfile);
                //string str_verfilename = Application.dataPath + "/../../OutAssets/" + ver_filename;
                //byte[] file_data = System.Text.Encoding.UTF8.GetBytes(str_verfiledata);
                FileStream stream = new FileStream(winfile, FileMode.Create);
                stream.Write(data, 0, len);
                stream.Flush();
                stream.Close();
            }

            fs.Close();

            return len;
        }

        return 0;
    }

    static private int Build_AssetBundle(string root_path, BuildTarget build_target)
    {
        int allfile_size = 0;
        if (Directory.Exists(root_path))
        {
            string[] files = Directory.GetFiles(root_path, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                allfile_size += Build_FileAssetBundle(file, build_target);
            }
        }
        else
        {
            allfile_size += Build_FileAssetBundle(root_path, build_target);
            //EditorUtility.DisplayDialog("提示", "无目录"+root_path, "确定");
        }

        return allfile_size;
    }

    [MenuItem("Assets/[CrossMono]/game打包（Android）平台下的所有文件")]
    static public void exportAndroid()
    {
        if (!checkPlatform(RuntimePlatform.Android))
            return;

        int nsize = 0;
        nsize += Build_AssetBundle(Application.dataPath + "/Resources/scene", BuildTarget.Android);
        nsize += Build_AssetBundle(Application.dataPath + "/Resources/character", BuildTarget.Android);

        nsize += Build_AssetBundle(Application.dataPath + "/Resources/uiimgs", BuildTarget.Android);
        nsize += Build_AssetBundle(Application.dataPath + "/Resources/uires", BuildTarget.Android);

        nsize += Build_AssetBundle(Application.dataPath + "/Resources/fx", BuildTarget.Android);
        nsize += Build_AssetBundle(Application.dataPath + "/Resources/media", BuildTarget.Android);

        nsize = nsize / 1024 + 1;
        EditorUtility.DisplayDialog("导出结束", "Android " + nsize + "k\n", "确定");
    }

    [MenuItem("Assets/[CrossMono]/game打包（IOS）平台下的所有文件")]
    static public void exportIOS()
    {
        if (!checkPlatform(RuntimePlatform.IPhonePlayer))
            return;

        int nsize = 0;
        nsize += Build_AssetBundle(Application.dataPath + "/Resources/scene", BuildTarget.iPhone);
        nsize += Build_AssetBundle(Application.dataPath + "/Resources/character", BuildTarget.iPhone);

        nsize += Build_AssetBundle(Application.dataPath + "/Resources/uiimgs", BuildTarget.iPhone);
        nsize += Build_AssetBundle(Application.dataPath + "/Resources/uires", BuildTarget.iPhone);

        nsize += Build_AssetBundle(Application.dataPath + "/Resources/fx", BuildTarget.iPhone);
        nsize += Build_AssetBundle(Application.dataPath + "/Resources/media", BuildTarget.iPhone);

        nsize = nsize / 1024 + 1;
        EditorUtility.DisplayDialog("导出结束", "IOS " + nsize + "k\n", "确定");
    }

    [MenuItem("Assets/[CrossMono]/game打包（Windows）平台下的所有文件")]
    static public void exportWindows()
    {
        if (!checkPlatform(RuntimePlatform.WindowsPlayer))
            return;

        int nsize = 0;
        nsize += Build_AssetBundle(Application.dataPath + "/Resources/scene", BuildTarget.StandaloneWindows);
        nsize += Build_AssetBundle(Application.dataPath + "/Resources/character", BuildTarget.StandaloneWindows);

        nsize += Build_AssetBundle(Application.dataPath + "/Resources/uiimgs", BuildTarget.StandaloneWindows);
        nsize += Build_AssetBundle(Application.dataPath + "/Resources/uires", BuildTarget.StandaloneWindows);

        nsize += Build_AssetBundle(Application.dataPath + "/Resources/fx", BuildTarget.StandaloneWindows);
        nsize += Build_AssetBundle(Application.dataPath + "/Resources/media", BuildTarget.StandaloneWindows);

        nsize = nsize / 1024 + 1;
        EditorUtility.DisplayDialog("导出结束", "Win " + nsize + "k\n", "确定");
    }


    [MenuItem("Assets/[CrossMono]当前目录导出（Windows）包")]
    static public void exportTargetWindows()
    {
        if (!checkPlatform(RuntimePlatform.WindowsPlayer))
            return;

        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        string out_dir = null;
        foreach (Object obj in SelectedAsset)
        {
            out_dir = AssetDatabase.GetAssetPath(obj);
        }

        if (out_dir != null)
        {
            out_dir = out_dir.Substring(out_dir.IndexOf("/"));

            int nsize = Build_AssetBundle(Application.dataPath + out_dir, BuildTarget.StandaloneWindows);
            nsize = nsize / 1024 + 1;
            EditorUtility.DisplayDialog("导出结束", "Win " + nsize + "k\n", "确定");
        }
    }

    [MenuItem("Assets/[CrossMono]当前目录导出（IOS）包")]
    static public void exportTargetIOS()
    {
        if (!checkPlatform(RuntimePlatform.IPhonePlayer))
            return;

        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        string out_dir = null;
        foreach (Object obj in SelectedAsset)
        {
            out_dir = AssetDatabase.GetAssetPath(obj);
        }

        if (out_dir != null)
        {
            out_dir = out_dir.Substring(out_dir.IndexOf("/"));

            int nsize = Build_AssetBundle(Application.dataPath + out_dir, BuildTarget.iPhone);
            nsize = nsize / 1024 + 1;
            EditorUtility.DisplayDialog("导出结束", "IOS " + nsize + "k\n", "确定");
        }
    }

    [MenuItem("Assets/[CrossMono]当前目录导出（Android）包")]
    static public void exportTargetAndroid()
    {
        if (!checkPlatform(RuntimePlatform.Android))
            return;

        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        string out_dir = null;
        foreach (Object obj in SelectedAsset)
        {
            out_dir = AssetDatabase.GetAssetPath(obj);
        }

        if (out_dir != null)
        {
            out_dir = out_dir.Substring(out_dir.IndexOf("/"));

            int nsize = Build_AssetBundle(Application.dataPath + out_dir, BuildTarget.Android);
            nsize = nsize / 1024 + 1;
            EditorUtility.DisplayDialog("导出结束", "Android " + nsize + "k\n", "确定");
        }
    }



    static public void preparePath(string path)
    {
        //删除文件
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        if (File.Exists(path + ".meta"))
        {
            File.Delete(path + ".meta");
        }

        //创建目录
        path = path.Substring(0, path.LastIndexOf('/'));
        string[] dirs = path.Split('/');
        for (int i = 2; i <= dirs.Length; ++i)
        {
            string subdir = "";
            for (int j = 0; j < i; ++j)
            {
                subdir += dirs[j];
                subdir += '/';
            }

            if (!Directory.Exists(subdir))
            {
                Directory.CreateDirectory(subdir);
            }
        }
    }
}
