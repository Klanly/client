using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class BuildVerList : Editor
{
    public static string getFileMD5(string filePath, ref int size_kb)
    {
        try
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            int len = (int)fs.Length;
            byte[] data = new byte[len];
            fs.Read(data, 0, len);
            fs.Close();

            size_kb = len / 1025;
            size_kb++;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string fileMD5 = "";
            foreach (byte b in result)
            {
                fileMD5 += Convert.ToString(b, 16);
            }
            return fileMD5;
        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e.Message);
            return "";
        }
    }

    static int ProcessOneDir(ref string str_verfiledata, string[] filter)
    {
        string[] curProcessFile = Directory.GetFiles(Application.dataPath + "/../../OutAssets/", "*.*", SearchOption.AllDirectories);

        float fcount = 0.0f;
        float fallcount = curProcessFile.Length;
        foreach (string file in curProcessFile)
        {
            bool bskip = false;
            for (int i = 0; i < filter.Length; i++)
            {
                if (file.IndexOf(filter[i]) > 0)
                {
                    bskip = true;
                    break;
                }
            }


            if (false == bskip)
            {
                ////过滤后缀
                //if (file.EndsWith(".res") || file.EndsWith(".lmp") || file.EndsWith(".lpb") || file.EndsWith(".anim") ||
                //    file.EndsWith(".pic") || file.EndsWith(".snd") || file.EndsWith(".xml") || file.EndsWith(".msk") || file.EndsWith(".crhm"))

                //过滤后缀
                if (file.EndsWith(".dat") || file.EndsWith(".xml"))
                {
                    int size_kb = 0;
                    string file_md5 = getFileMD5(file, ref size_kb);
                    string file_name = file.Substring(file.IndexOf("/../../OutAssets/") + 17);
                    file_name = file_name.Replace('\\', '/');
                    str_verfiledata += file_name + "," + file_md5 + "," + size_kb + "\n";

                    fcount++;
                    EditorUtility.DisplayProgressBar("打包中...", file, fcount / fallcount);

                    //if (fcount > 300.0f) break;
                }
                else
                {
                    Debug.Log(file);
                }
            }
        }

        EditorUtility.ClearProgressBar();

        return (int)fcount;
    }

    static void SaveVerFile(int filecout, ref string str_verfiledata, BuildTarget build_target)
    {
        string ver_filename = "null";
        if (BuildTarget.Android == build_target)
            ver_filename = "android_version.ver";
        else if (BuildTarget.iPhone== build_target)
            ver_filename = "ios_version.ver";
        else
            ver_filename = "win_version.ver";

        string str_verfilename = Application.dataPath + "/../../OutAssets/" + ver_filename;
        byte[] file_data = System.Text.Encoding.UTF8.GetBytes(str_verfiledata);
        FileStream stream = new FileStream(str_verfilename, FileMode.Create);
        stream.Write(file_data, 0, file_data.Length);
        stream.Flush();
        stream.Close();

        int size_kb = 0;
        string str_verfilemd5 = getFileMD5(str_verfilename, ref size_kb);
        string str_ververfilename = Application.dataPath + "/../../OutAssets/" + ver_filename + ".ver";
        file_data = System.Text.Encoding.UTF8.GetBytes(str_verfilemd5);
        stream = new FileStream(str_ververfilename, FileMode.Create);
        stream.Write(file_data, 0, file_data.Length);
        stream.Flush();
        stream.Close();

        EditorUtility.DisplayDialog("打包成功", "共处理文件数 " + filecout, "确定");
    } 


    [MenuItem("BuildVerVer/[CrossMono]导出（Windows）OutAssets文件更新表")]
    static void BuildVer_Win()
    {
        string str_verfiledata = "";
        int filecout = ProcessOneDir(ref str_verfiledata, 
            "/OutAssets/android,/OutAssets/ios,/OutAssets/win_version".Split(','));

        SaveVerFile(filecout, ref str_verfiledata, BuildTarget.StandaloneWindows);

        str_verfiledata = null;
    }

    [MenuItem("BuildVerVer/[CrossMono]导出（Android）OutAssets文件更新表")]
    static void BuildVer_Android()
    {
        string str_verfiledata = "";
        int filecout = ProcessOneDir(ref str_verfiledata, 
            "/OutAssets/win,/OutAssets/ios,/OutAssets/android_version".Split(','));

        SaveVerFile(filecout, ref str_verfiledata, BuildTarget.Android);

        str_verfiledata = null;
    }

    [MenuItem("BuildVerVer/[CrossMono]导出（IOS）OutAssets文件更新表")]
    static void BuildVer_IOS()
    {
        string str_verfiledata = "";
        int filecout = ProcessOneDir(ref str_verfiledata, 
            "/OutAssets/win,/OutAssets/android,/OutAssets/ios_version".Split(','));

        SaveVerFile(filecout, ref str_verfiledata, BuildTarget.iPhone);

        str_verfiledata = null;
    }
}
