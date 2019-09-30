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
using MuGame;
public class BuildOutAssetsXml : Editor
{
   public static ByteArray ProcessAllXML(bool desc=true,string lang = "")
    {
        string[] curProcessFile = Directory.GetFiles(Application.dataPath + "/../../xml_StaticData/", "*.*", SearchOption.TopDirectoryOnly);


      
        //const int all_xml_maxsize = 8 * 1024 * 1024;
        //byte[] all_xml_data = new byte[all_xml_maxsize];
        //int ncur_pos = 0;


        List<XmlFileData> ld = new List<XmlFileData>();
        int src_len = 0;

        int lastLen = 0;
        float fcount = 0.0f;
        float fallcount = curProcessFile.Length;
        foreach (string file in curProcessFile)
        {
            string xmlFilePath = file;
            //过滤后缀
            if (file.EndsWith(".xml"))
            {
                FileInfo f = new FileInfo(file);
                if (lang!="")
                {
                  
                    string tempstr = Application.dataPath + "/../../xml_StaticData/" + lang + "/" + f.Name;
                    if (File.Exists(tempstr))
                    {
                        xmlFilePath = tempstr;
                        f = new FileInfo(xmlFilePath);
                    }

                }


             


                XmlFileData fileD = new XmlFileData();
                FileStream fs = new FileStream(xmlFilePath, FileMode.Open);
                fcount++;
                EditorUtility.DisplayProgressBar("打包xml中...", xmlFilePath, fcount / fallcount);
                string file_name = f.Name;
                file_name = file_name.Substring(0, file_name.Length - 4);
                fileD.idx = getStrIdx(file_name);
               
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
                    Debug.LogError("表格式错误：" + xmlFilePath);

                }

                src_len += len;

                ByteArray xmlBa = new ByteArray();
                addNode(xmlBa, xmlDoc.DocumentElement);
                fileD.ba = xmlBa;

                ld.Add(fileD);
            }
            //else
            //{
            //    debug.Log(file);
            //}
        }
        ByteArray mainBa = new ByteArray();
        fcount = 0;
        foreach (string str in lStr)
        {
            ByteArray strBa = new ByteArray();
            strBa.writeUTF8Bytes(str);
            mainBa.writeInt(strBa.length);
            mainBa.writeBytes(strBa.data, strBa.length);

            //  debug.Log("in::" + str + "::" + strBa.length);

            fcount++;
            if (fcount % 500 == 0)
                EditorUtility.DisplayProgressBar("打包文本...", str, fcount / lStr.Count);
        }
        mainBa.writeInt(-1);
        fcount = 0;
        foreach (XmlFileData d in ld)
        {
            mainBa.writeInt(d.idx);
            mainBa.writeInt(d.ba.length);
            mainBa.writeBytes(d.ba, 0, d.ba.length);
            fcount++;
            EditorUtility.DisplayProgressBar("打包头文件...", lStr[d.idx], fcount / ld.Count);
        }


        EditorUtility.ClearProgressBar();

        //保存打包数据
        string resPath = Application.dataPath + "/../../OutAssets/";



        mainBa.compress();
        FileStream fd_stream = new FileStream(resPath + "staticdata.dat", FileMode.Create);
        fd_stream.Write(mainBa.data, 0, mainBa.length);
        fd_stream.Flush();
        fd_stream.Close();



        debug.Log(Application.dataPath + "/../../OutAssets/");
        debug.Log(Application.dataPath + "/" + AppConst.AssetDirname + "/OutAssets/");

        Util.DeleteFolder(Application.dataPath + "/" + AppConst.AssetDirname + "/OutAssets/");
        Util.CopyDirectory(resPath, Application.dataPath + "/" + AppConst.AssetDirname + "/OutAssets/");

        Util.DeleteFolder(Util.DataPath + "/OutAssets/");
        Util.CopyDirectory(resPath, Util.DataPath + "/OutAssets/");


        Packager.refreshFileList();

        //size = size / 1024 + 1;
        if(desc)
        EditorUtility.DisplayDialog("打包成功", "原始大小 " + src_len / 1024 + "k， 打包后大小" + mainBa.length / 1024 + "k", "确定");

        return mainBa;
    }



    static List<XmlFileData> ld = new List<XmlFileData>();
    static List<string> lStr = new List<string>();
    static Dictionary<string, int> dStr = new Dictionary<string, int>();
    static int addNode(ByteArray ba, XmlNode xmlDoc)
    {
        if (xmlDoc.Name == "#text")
            return -1;

        int idx = getStrIdx(xmlDoc.Name);
        ba.writeInt(idx);
        ByteArray main = new ByteArray();

        foreach (XmlAttribute attr in xmlDoc.Attributes)
        {
            addAttr(main, attr);
        }
        main.writeInt(-1);

        int offset = main.length;

        foreach (XmlNode child in xmlDoc.ChildNodes)
        {
            ByteArray nodeBa = new ByteArray();
            if (child.NodeType == XmlNodeType.Comment)
                continue;
            int ididx = addNode(nodeBa, child);

            if (ididx == -1) continue;
            main.writeInt(ididx);
            main.writeInt(nodeBa.length);
            main.writeBytes(nodeBa.data, nodeBa.length);
            nodeBa.clear();
        }
        main.writeInt(-1);

        ba.writeBytes(main.data, main.length);
        return idx;
    }

    static int getStrIdx(string str)
    {
        if (dStr.ContainsKey(str))
            return dStr[str];
        int idx = lStr.Count;
        lStr.Add(str);
        dStr[str] = idx;
        return idx;
    }

    static int addAttr(ByteArray ba, XmlAttribute attr)
    {
        if (attr.Value == "")
            return -1;

        int idx = getStrIdx(attr.Name);



        if (IsUnsign(attr.Value))
        {
            if (attr.Value.Contains("."))
            {
                float b = float.Parse(attr.Value);
                ba.writeInt(idx);
                ba.writeByte(6);
                ba.writeFloat(b);
                return idx;
            }

            uint temp = uint.Parse(attr.Value);
            if (temp < 128)
            {

                sbyte b = sbyte.Parse(attr.Value);
                ba.writeInt(idx);
                ba.writeByte(1);
                ba.writeByte(b);
                return idx;


            }
            else if (temp < 32768)
            {
                short b = short.Parse(attr.Value);
                ba.writeInt(idx);
                ba.writeByte(2);
                ba.writeShort(b);
                return idx;
            }
            else if (temp < 2147483647)
            {
                int b = int.Parse(attr.Value);
                ba.writeInt(idx);
                ba.writeByte(3);
                ba.writeInt(b);
                return idx;
            }
            else
            {
                uint b = uint.Parse(attr.Value);
                ba.writeInt(idx);
                ba.writeByte(5);
                ba.writeUnsignedInt(b);
                return idx;
            }
        }
        if (IsInt(attr.Value))
        {

            int b = int.Parse(attr.Value);
            ba.writeInt(idx);
            ba.writeByte(3);
            ba.writeInt(b);
            return idx;
        }
        else
        {
            string s = attr.Value.ToString();
            ba.writeInt(idx);
            ba.writeByte(4);
            ba.writeInt(getStrIdx(s));
            return idx;
        }


        return -1;
    }

    public static bool IsInt(string value)
    {
        return Regex.IsMatch(value, @"^[+-]?\d*$");
    }
    public static bool IsUnsign(string value)
    {
        return Regex.IsMatch(value, @"^\d*[.]?\d*$");
    }


    //[MenuItem("Assets/[QSMY]打包Xml配置")]

    [MenuItem("Qinsmy/导出xml/自动导出")]
    public static void BuildXMl_auto()
    {
        string lang = "";
#if zh_tw
        lang = "zh_tw";
#endif 
        Build_OutAssetsXml(lang);
    }


    [MenuItem("Qinsmy/导出xml/zh_cn")]
    public static void BuildXMl_zh_cn()
    {
        Build_OutAssetsXml("");
    }
    [MenuItem("Qinsmy/导出xml/zh_tw")]
    public static void BuildXMl_zh_tw()
    {
        Build_OutAssetsXml("zh_tw");
    }

    public static void Build_OutAssetsXml(string lang)
    {
        ld.Clear();
        lStr.Clear();
        dStr.Clear();
        ProcessAllXML(true, lang);


        //XMLMgr.instance.init(ba);
        //SXML xml = XMLMgr.instance.GetSXML("item");
        //debug.Log(xml.GetNode("item", "id==101").getString("item_name"));
        //  debug.Log(xml.GetNode("item.decompose").getInt("item"));
        //xml.forEach((SXML x) =>
        //{
        //    debug.Log(x.getString("id"));
        //});


    }

    public static void Build_OutAssetsXmlnodesc()
    {
        ld.Clear();
        lStr.Clear();
        dStr.Clear();
        ProcessAllXML(false);
    }

}

class XmlFileData
{
    public int idx;
    public ByteArray ba;
}



