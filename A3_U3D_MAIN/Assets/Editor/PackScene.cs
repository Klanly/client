using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PackScene : Editor
{
//    [MenuItem("Assets/[CrossMono]打包场景资源（Android）")]
//    static public void exportAndroid()
//    {
//        if (!checkPlatform(RuntimePlatform.Android))
//            return;

//        string rootPath = Application.dataPath + "/Resources/scene";
//        if (Directory.Exists(rootPath))
//        {
//            string[] files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
//            foreach (string file in files) 
//            {
//                if (file.EndsWith(".prefab") || file.EndsWith(".exr") || file.EndsWith(".asset"))
//                {
//                    //获取u3d资源引用地址
//                    string assetPath = file.Substring(0, file.LastIndexOf("."));
//                    assetPath = assetPath.Substring(assetPath.IndexOf("Resources/") + "Resources/".Length);
//                    assetPath = assetPath.Replace('\\', '/');

//                    //生成AssetBundle存放地址
//                    //string dstPath = Application.streamingAssetsPath + "/android/" + assetPath + ".res";
//                    string dstPath = Application.dataPath + "/../../OutAssets/android/" + assetPath + ".res";
                    
//                    //从Resources中加载资源
//                    Object res = Resources.Load(assetPath);

//                    //Debug.Log("Build: " + assetPath);
//                    PackScene.preparePath(dstPath);
//                    BuildPipeline.BuildAssetBundle(res, new Object[] { res }, dstPath, 
//                                                        BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.Android);
//                }
//            }
//        }
//    }

//    [MenuItem("Assets/[CrossMono]打包场景资源（PC）")]
//    static public void exportPC()
//    {
//        if (!checkPlatform(RuntimePlatform.WindowsPlayer))
//            return;

//        string rootPath = Application.dataPath + "/Resources/scene";
//        if (Directory.Exists(rootPath))
//        {
//            string[] files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
//            foreach (string file in files)
//            {
//                if (file.EndsWith(".prefab") || file.EndsWith(".exr") || file.EndsWith(".asset"))
//                {
//                    //获取u3d资源引用地址
//                    string assetPath = file.Substring(0, file.LastIndexOf("."));
//                    assetPath = assetPath.Substring(assetPath.IndexOf("Resources/") + "Resources/".Length);
//                    assetPath = assetPath.Replace('\\', '/');

//                    //生成AssetBundle存放地址
//                    //string dstPath = Application.streamingAssetsPath + "/win/" + assetPath + ".res";
//                    string dstPath = Application.dataPath + "/../../OutAssets/win/" + assetPath + ".res";

//                    //从Resources中加载资源
//                    Object res = Resources.Load(assetPath);

//                    //Debug.Log("Build: " + assetPath);
//                    PackScene.preparePath(dstPath);
//                    BuildPipeline.BuildAssetBundle(res, new Object[] { res }, dstPath,
//                                                        BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.StandaloneWindows);
//                }
//            }
//        }
//    }


//    [MenuItem("Assets/[CrossMono]打包角色资源（PC）")]
//    static public void exportCharPC()
//    {
//        if (!checkPlatform(RuntimePlatform.WindowsPlayer))
//            return;

//        string rootPath = Application.dataPath + "/Resources/character";
//        int prefabfile_count = -1;
//        int animfile_count = -1;
//        if (Directory.Exists(rootPath))
//        {
//            prefabfile_count = 0;
//            string[] files = Directory.GetFiles(rootPath, "*.prefab", SearchOption.AllDirectories);
//            foreach (string file in files)
//            {
//                //if (file.EndsWith(".prefab") || file.EndsWith(".anim"))
//                {
//                    //获取u3d资源引用地址
//                    string assetPath = file.Substring(0, file.LastIndexOf(".prefab"));
//                    assetPath = assetPath.Substring(assetPath.IndexOf("Resources/") + "Resources/".Length);
//                    assetPath = assetPath.Replace('\\', '/');

//                    //生成AssetBundle存放地址
//                    //string dstPath = Application.streamingAssetsPath + "/win/" + assetPath + ".res";
//                    string dstPath = Application.dataPath + "/../../OutAssets/win/" + assetPath + ".res";

//                    //从Resources中加载资源
//                    Object res = Resources.Load(assetPath);

//                    //Debug.Log("Build: " + assetPath);
//                    prefabfile_count++;
//                    PackScene.preparePath(dstPath);
//                    BuildPipeline.BuildAssetBundle(res, new Object[] { res }, dstPath,
//                                                        BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.StandaloneWindows);
//                }
//            }

//            animfile_count = 0;
//            files = Directory.GetFiles(rootPath, "*.anim", SearchOption.AllDirectories);
//            foreach (string file in files)
//            {
//                //if (file.EndsWith(".prefab") || file.EndsWith(".anim"))
//                {
//                    //获取u3d资源引用地址
//                    string assetPath = file.Substring(0, file.LastIndexOf(".anim"));
//                    assetPath = assetPath.Substring(assetPath.IndexOf("Resources/") + "Resources/".Length);
//                    assetPath = assetPath.Replace('\\', '/');

//                    //生成AssetBundle存放地址
//                    //string dstPath = Application.streamingAssetsPath + "/win/" + assetPath + ".anim";
//                    string dstPath = Application.dataPath + "/../../OutAssets/win/" + assetPath + ".anim";

//                    //从Resources中加载资源
//                    Object res = Resources.Load(assetPath);

//                    //Debug.Log("Build: " + assetPath);
//                    animfile_count++;
//                    PackScene.preparePath(dstPath);
//                    BuildPipeline.BuildAssetBundle(res, new Object[] { res }, dstPath,
//                                                        BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.StandaloneWindows);
//                }
//            }

//            //allfile_count = files.Length;
//        }

//        Debug.Log("prefab file = " + prefabfile_count + "  anim file = " + animfile_count);
//    }


//    [MenuItem("Assets/[CrossMono]打包角色资源（Android）")]
//    static public void exportCharAndroid()
//    {
//        if (!checkPlatform(RuntimePlatform.Android))
//            return;

//        string rootPath = Application.dataPath + "/Resources/character";
//        int prefabfile_count = -1;
//        int animfile_count = -1;
//        if (Directory.Exists(rootPath))
//        {
//            prefabfile_count = 0;
//            string[] files = Directory.GetFiles(rootPath, "*.prefab", SearchOption.AllDirectories);
//            foreach (string file in files)
//            {
//                //if (file.EndsWith(".prefab") || file.EndsWith(".anim"))
//                {
//                    //获取u3d资源引用地址
//                    string assetPath = file.Substring(0, file.LastIndexOf(".prefab"));
//                    assetPath = assetPath.Substring(assetPath.IndexOf("Resources/") + "Resources/".Length);
//                    assetPath = assetPath.Replace('\\', '/');

//                    //生成AssetBundle存放地址
//                    string dstPath = Application.dataPath + "/../../OutAssets/android/" + assetPath + ".res";

//                    //从Resources中加载资源
//                    Object res = Resources.Load(assetPath);

//                    //Debug.Log("Build: " + assetPath);
//                    prefabfile_count++;
//                    PackScene.preparePath(dstPath);
//                    BuildPipeline.BuildAssetBundle(res, new Object[] { res }, dstPath,
//                                                        BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.Android);
//                }
//            }

//            animfile_count = 0;
//            files = Directory.GetFiles(rootPath, "*.anim", SearchOption.AllDirectories);
//            foreach (string file in files)
//            {
//                //if (file.EndsWith(".prefab") || file.EndsWith(".anim"))
//                {
//                    //获取u3d资源引用地址
//                    string assetPath = file.Substring(0, file.LastIndexOf(".anim"));
//                    assetPath = assetPath.Substring(assetPath.IndexOf("Resources/") + "Resources/".Length);
//                    assetPath = assetPath.Replace('\\', '/');

//                    //生成AssetBundle存放地址
//                    string dstPath = Application.dataPath + "/../../OutAssets/android/" + assetPath + ".anim";

//                    //从Resources中加载资源
//                    Object res = Resources.Load(assetPath);

//                    //Debug.Log("Build: " + assetPath);
//                    animfile_count++;
//                    PackScene.preparePath(dstPath);
//                    BuildPipeline.BuildAssetBundle(res, new Object[] { res }, dstPath,
//                                                        BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.Android);
//                }
//            }

//            //allfile_count = files.Length;
//        }

//        Debug.Log("prefab file = " + prefabfile_count + "  anim file = " + animfile_count);
//    }

//    static private bool checkPlatform(RuntimePlatform rp)
//    {
//        bool bright_platform = false;
//        switch (rp)
//        {
//            case RuntimePlatform.Android:
//                {
//#if UNITY_ANDROID
//                    bright_platform = true;
//#endif
//                }break;
//            case RuntimePlatform.IPhonePlayer:
//                {
//#if UNITY_IPHONE
//                    bright_platform = true;
//#endif
//                } break;
//            case RuntimePlatform.WindowsPlayer:
//                {
//#if UNITY_STANDALONE_WIN
//                    bright_platform = true;
//#endif
//                } break;
//        }

//        if (!bright_platform)
//        {
//            switch (rp)
//            {
//                case RuntimePlatform.Android: EditorUtility.DisplayDialog("提示", "请手动切换到Android平台下进行导出", "确定"); break;
//                case RuntimePlatform.IPhonePlayer: EditorUtility.DisplayDialog("提示", "请手动切换到IOS平台下进行导出", "确定"); break;
//                case RuntimePlatform.WindowsPlayer: EditorUtility.DisplayDialog("提示", "请手动切换到Windows平台下进行导出", "确定"); break;
//            }
//        }

//        return bright_platform;
//    }


//    [MenuItem("Assets/[CrossMono]打包UI资源（PC）")]
//    static public void exportUIResPC()
//    {
//        if (!checkPlatform(RuntimePlatform.WindowsPlayer))
//            return;

//        int file_count = -1;
//        string rootPath = Application.dataPath + "/Resources/uiimgs";
//        if (Directory.Exists(rootPath))
//        {
//            file_count = 0;
//            string[] files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
//            foreach (string file in files)
//            {
//                if (file.EndsWith(".png") || file.EndsWith(".jpg"))
//                {
//                    //获取u3d资源引用地址
//                    string assetPath = file.Substring(0, file.Length - 4);
//                    assetPath = assetPath.Substring(assetPath.IndexOf("Resources/") + "Resources/".Length);
//                    assetPath = assetPath.Replace('\\', '/');

//                    //生成AssetBundle存放地址
//                    string dstPath = Application.dataPath + "/../../OutAssets/win/" + assetPath + ".pic";

//                    //从Resources中加载资源
//                    Object res = Resources.Load(assetPath);

//                    //Debug.Log("Build: " + assetPath);
//                    file_count++;
//                    PackScene.preparePath(dstPath);
//                    BuildPipeline.BuildAssetBundle(res, new Object[] { res }, dstPath,
//                                                        BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.StandaloneWindows);
//                }
//            }
//        }

//        rootPath = Application.dataPath + "/Resources/uires";
//        if (Directory.Exists(rootPath))
//        {
//            file_count = 0;
//            string[] files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
//            foreach (string file in files)
//            {
//                if (file.EndsWith(".png") || file.EndsWith(".jpg"))
//                {
//                    //获取u3d资源引用地址
//                    string assetPath = file.Substring(0, file.Length - 4);
//                    assetPath = assetPath.Substring(assetPath.IndexOf("Resources/") + "Resources/".Length);
//                    assetPath = assetPath.Replace('\\', '/');

//                    //生成AssetBundle存放地址
//                    string dstPath = Application.dataPath + "/../../OutAssets/win/" + assetPath + ".pic";

//                    //从Resources中加载资源
//                    Object res = Resources.Load(assetPath);

//                    //Debug.Log("Build: " + assetPath);
//                    file_count++;
//                    PackScene.preparePath(dstPath);
//                    BuildPipeline.BuildAssetBundle(res, new Object[] { res }, dstPath,
//                                                        BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.StandaloneWindows);
//                }
//            }
//        }

//        Debug.Log("build file count = " + file_count);
//    }


//    [MenuItem("Assets/[CrossMono]打包UI资源（Android）")]
//    static public void exportUIResAndroid()
//    {
//        if (!checkPlatform(RuntimePlatform.Android))
//            return;

//        int file_count = -1;
//        string rootPath = Application.dataPath + "/Resources/uiimgs";
//        if (Directory.Exists(rootPath))
//        {
//            file_count = 0;
//            string[] files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
//            foreach (string file in files)
//            {
//                if (file.EndsWith(".png") || file.EndsWith(".jpg"))
//                {
//                    //获取u3d资源引用地址
//                    string assetPath = file.Substring(0, file.Length - 4);
//                    assetPath = assetPath.Substring(assetPath.IndexOf("Resources/") + "Resources/".Length);
//                    assetPath = assetPath.Replace('\\', '/');

//                    //生成AssetBundle存放地址
//                    string dstPath = Application.dataPath + "/../../OutAssets/android/" + assetPath + ".pic";

//                    //从Resources中加载资源
//                    Object res = Resources.Load(assetPath);

//                    //Debug.Log("Build: " + assetPath);
//                    file_count++;
//                    PackScene.preparePath(dstPath);
//                    BuildPipeline.BuildAssetBundle(res, new Object[] { res }, dstPath,
//                                                        BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.Android);
//                }
//            }
//        }

//        rootPath = Application.dataPath + "/Resources/uires";
//        if (Directory.Exists(rootPath))
//        {
//            file_count = 0;
//            string[] files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
//            foreach (string file in files)
//            {
//                if (file.EndsWith(".png") || file.EndsWith(".jpg"))
//                {
//                    //获取u3d资源引用地址
//                    string assetPath = file.Substring(0, file.Length - 4);
//                    assetPath = assetPath.Substring(assetPath.IndexOf("Resources/") + "Resources/".Length);
//                    assetPath = assetPath.Replace('\\', '/');

//                    //生成AssetBundle存放地址
//                    string dstPath = Application.dataPath + "/../../OutAssets/android/" + assetPath + ".pic";

//                    //从Resources中加载资源
//                    Object res = Resources.Load(assetPath);

//                    //Debug.Log("Build: " + assetPath);
//                    file_count++;
//                    PackScene.preparePath(dstPath);
//                    BuildPipeline.BuildAssetBundle(res, new Object[] { res }, dstPath,
//                                                        BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.Android);
//                }
//            }
//        }

//        Debug.Log("build file count = " + file_count);
//    }

//    static public void preparePath(string path)
//    {
//        //删除文件
//        if (File.Exists(path))
//        {
//            File.Delete(path);
//        }

//        if (File.Exists(path + ".meta"))
//        {
//            File.Delete(path + ".meta");
//        }

//        //创建目录
//        path = path.Substring(0, path.LastIndexOf('/'));
//        string[] dirs = path.Split('/');
//        for (int i = 2; i <= dirs.Length; ++i)
//        {
//            string subdir = "";
//            for (int j = 0; j < i; ++j)
//            {
//                subdir += dirs[j];
//                subdir += '/';
//            }

//            if (!Directory.Exists(subdir))
//            {
//                Directory.CreateDirectory(subdir);
//            }
//        }
//    }
}
