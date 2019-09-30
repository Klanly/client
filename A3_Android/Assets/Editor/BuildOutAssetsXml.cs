using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Cross;

public class BuildOutAssetsXml : Editor
{
    static void ProcessAllXML()
    {
        string[] curProcessFile = Directory.GetFiles(Application.dataPath + "/../../xml_StaticData/", "*.*", SearchOption.AllDirectories);

        //const int all_xml_maxsize = 8 * 1024 * 1024;
        //byte[] all_xml_data = new byte[all_xml_maxsize];
        //int ncur_pos = 0;

        ByteArray sd_data = new ByteArray();


        float fcount = 0.0f;
        float fallcount = curProcessFile.Length;
        foreach (string file in curProcessFile)
        {
            //过滤后缀
            if (file.EndsWith(".xml"))
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                int len = (int)fs.Length;

                //需要将名字和大小加入
                //staticdata
                string file_name = file.Substring(file.IndexOf("/../../xml_StaticData/") + 22);
                file_name = file_name.Substring(0, file_name.Length - 4);
                sd_data.writeInt(file_name.Length);
                sd_data.writeUTF8Bytes(file_name);

                byte[] data = new byte[len];
                fs.Read(data, 0, len);
                fs.Close();

                sd_data.writeInt(len);
                sd_data.writeBytes(data, len);

                fcount++;
                EditorUtility.DisplayProgressBar("打包中...", file, fcount / fallcount);
            }
            else
            {
                Debug.Log(file);
            }
        }

        EditorUtility.ClearProgressBar();

        //保存打包数据
        int src_len = sd_data.length;

        sd_data.compress();
        FileStream fd_stream = new FileStream(Application.dataPath + "/../../OutAssets/staticdata/staticdata.dat", FileMode.Create);
        fd_stream.Write(sd_data.data, 0, sd_data.length);
        fd_stream.Flush();
        fd_stream.Close();

        //size = size / 1024 + 1;
        EditorUtility.DisplayDialog("打包成功", "原始大小 " + src_len / 1024 + "k， 打包后大小" + sd_data.length/1024 + "k", "确定");
    }

    //[MenuItem("Assets/[QSMY]打包Xml配置")]
    [MenuItem("Qinsmy/打包xml_StaticData目录中的所有Xml配置")]
    static void Build_OutAssetsXml()
    {
        ProcessAllXML();
    }

}
