using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using MuGame;
using Cross;

public class Export_grd_crhm : Editor
{
    [MenuItem("Qinsmy/导出阻挡图和高度图")]
    static void Build_qsmy_grd()
    {
        //处理选择地图配置
        GameObject go_map_id = GameObject.Find("#map_id");
        if (go_map_id == null)
        {
            EditorUtility.DisplayDialog("错误", "无法找到地图配置 物件 #map_id", "确定");
            return;
        }

        string str_mapid = "";
        if ( 1 == go_map_id.transform.childCount )
        {
            str_mapid = go_map_id.transform.GetChild(0).name;
        }

        //地图的宽和高
        string server_grdfile = null;
        string client_mskfile = null;
        string client_hdtfile = null;
        int w = 400;
        int h = 400;

        //读取游戏内的配置表
        {
            FileStream gconf_map = new FileStream(Application.dataPath + "/../../OutAssets/gconf/map.xml", FileMode.Open);
            int len = (int)gconf_map.Length;
            byte[] data = new byte[len];
            gconf_map.Read(data, 0, len);
            gconf_map.Close();

            string data_str = System.Text.Encoding.UTF8.GetString(data, 0, len);
            XMLMgr.instance.AddXmlData("gmap", ref data_str);
        }

        //读取服务器表数据
        {
            FileStream svr_map = new FileStream(Application.dataPath + "/../../OutAssets/svrconfig/map.xml", FileMode.Open);
            int len = (int)svr_map.Length;
            byte[] data = new byte[len];
            svr_map.Read(data, 0, len);
            svr_map.Close();

            string data_str = System.Text.Encoding.UTF8.GetString(data, 0, len);

            //debug.Log(data_str + "  len = " + len);

            XMLMgr.instance.AddXmlData("smap", ref data_str);
        }

        {
            SXML sxml = XMLMgr.instance.GetSXML("gmap.map", "id==" + str_mapid);
            if (sxml != null)
            {
                client_mskfile = sxml.getString("mskfile");
                client_hdtfile = sxml.getString("hdtfile");

                debug.Log("msk = " + client_mskfile + "  |||||| hdt = " + client_hdtfile);
            }
            else
            {
                EditorUtility.DisplayDialog("错误", "无法/gconf/map.xml中获取ID为" + str_mapid + "的地图的信息", "确定");
                return;
            }
        }

        {
            SXML sxml = XMLMgr.instance.GetSXML("smap.m", "id==" + str_mapid);
            if (sxml != null)
            {
                w = sxml.getInt("width");
                h = sxml.getInt("height");
                debug.Log("mapsize    w = " + w + "  ||||||  h = " + h);

                SXML sn = sxml.GetNode("g", null);
                if (sn != null)
                {
                    server_grdfile = sn.getString("file");
                    debug.Log("server_grdfile = " + server_grdfile);
                }
            }
            else
            {
                EditorUtility.DisplayDialog("错误", "无法/svrconfig/map.xml中获取ID为" + str_mapid + "的地图的信息", "确定");
                return;
            }
        }

        
        short[] map_grd = new short[w*h];
        float[] map_hdt = new float[w * h];

        ////射线碰撞，找自己要的目标
        int n_canpass_cell = 0;
        RaycastHit rchit;
        Vector3 head_pos = new Vector3(0.0f, 65535.0f / 2.0f, 0.0f);
        Vector3 head_dir = new Vector3(0.0f, -1.0f, 0.0f);

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                float fhit_hdt = 0.0f;
                int hit_count = 0;
                int index = j * w + i;

                head_pos.x = i * 0.6f + 0.3f;
                head_pos.z = j * 0.6f + 0.3f;
                if (Physics.Raycast(head_pos, head_dir, out rchit, 65535.0f))
                {
                    fhit_hdt += rchit.point.y;
                    hit_count++;
                }

                head_pos.x = i * 0.6f;
                head_pos.z = j * 0.6f;
                if (Physics.Raycast(head_pos, head_dir, out rchit, 65535.0f))
                {
                    fhit_hdt += rchit.point.y;
                    hit_count++;
                }

                head_pos.x = i * 0.6f + 0.6f;
                head_pos.z = j * 0.6f + 0.6f;
                if (Physics.Raycast(head_pos, head_dir, out rchit, 65535.0f))
                {
                    fhit_hdt += rchit.point.y;
                    hit_count++;
                }

                head_pos.x = i * 0.6f + 0.6f;
                head_pos.z = j * 0.6f;
                if (Physics.Raycast(head_pos, head_dir, out rchit, 65535.0f))
                {
                    fhit_hdt += rchit.point.y;
                    hit_count++;
                }

                head_pos.x = i * 0.6f;
                head_pos.z = j * 0.6f + 0.6f;
                if (Physics.Raycast(head_pos, head_dir, out rchit, 65535.0f))
                {
                    fhit_hdt += rchit.point.y;
                    hit_count++;
                }

                //只要有碰到就认为是可以行走的
                if (hit_count > 0){
                    fhit_hdt = fhit_hdt / hit_count;
                    map_hdt[index] = fhit_hdt;
                    map_grd[index] = 0;
                    n_canpass_cell++;
                }else{
                    map_hdt[index] = 0.0f;
                    map_grd[index] = 4096;
                }
            }

            float percent = (float)i/h;
            EditorUtility.DisplayProgressBar("生成阻挡中...", i.ToString(), percent);
        }

        EditorUtility.ClearProgressBar();

        //导出阻挡信息
        {
            {
                //服务端用的阻挡信息
                string output_filename_server = Application.dataPath + "/../../xml_ServerData/" + server_grdfile;

                FileStream msk_stream = new FileStream(output_filename_server, FileMode.Create);
                byte[] file_data = new byte[map_grd.Length * sizeof(short)];
                Buffer.BlockCopy(map_grd, 0, file_data, 0, file_data.Length);
                msk_stream.Write(file_data, 0, file_data.Length);
                msk_stream.Flush();
                msk_stream.Close();
            }

            {
                //客户端用的阻挡信息
                string output_filename_client = Application.dataPath + "/../../OutAssets/" + client_mskfile;

                ByteArray msk_data = new ByteArray();
                byte[] file_data = new byte[map_grd.Length * sizeof(short)];
                Buffer.BlockCopy(map_grd, 0, file_data, 0, file_data.Length);
                msk_data.writeBytes(file_data, file_data.Length);
                msk_data.compress();

                FileStream msk_stream = new FileStream(output_filename_client, FileMode.Create);
                msk_stream.Write(msk_data.data, 0, msk_data.length);
                msk_stream.Flush();
                msk_stream.Close();
            }
        }


        //导出高度信息
        float max = -99999999.0f;
        float min = 99999999.0f;
        float average_h = 0.0f;
        //整合数据
        for (int i = 0; i < map_hdt.Length; i++)
        {
            if (map_grd[i] == 0)
            {
                if (map_hdt[i] > max) max = map_hdt[i];
                if (map_hdt[i] < min) min = map_hdt[i];

                average_h += map_hdt[i];
            }
        }

        if( client_hdtfile != null )
        {
            string output_filename_client = Application.dataPath + "/../../OutAssets/" + client_hdtfile;


            if( n_canpass_cell > 0 ){
                average_h = average_h / n_canpass_cell;
            }

            ByteArray hdt_data = new ByteArray();
            byte[] file_data = new byte[map_hdt.Length * sizeof(float)];
            Buffer.BlockCopy(map_hdt, 0, file_data, 0, file_data.Length);
            hdt_data.writeBytes(file_data, file_data.Length);
            hdt_data.compress();

            FileStream hdt_stream = new FileStream(output_filename_client, FileMode.Create);
            hdt_stream.Write(hdt_data.data, 0, hdt_data.length);
            hdt_stream.Flush();
            hdt_stream.Close();
        }

        string res_msg = "地图 宽w = " + w.ToString() + " 高 h =" + h.ToString();
        res_msg += "\n共有" + n_canpass_cell + "格可行走";
        res_msg += "\n最大高度=" + max.ToString("0.00") + " 最小高度=" + min.ToString("0.00") + " 平均高度=" + average_h.ToString("0.00");

        if( client_hdtfile != null ){
            //string str_min = String.Format("{0:N2}", Convert.ToDecimal("0.333333").ToString());
            //string str_min = String.Format("{0:N2}", Convert.ToDecimal(min.ToString()).ToString());

            //result = Convert.ToDouble(dValue).ToString("0.00");//保留小数点后两位,结果为95.12
            //result = Convert.ToDouble(iValue).ToString("0.00");//10000.00 
            //result = Convert.ToDouble(strValue).ToString("0.00");//95.12
            //result = Convert.ToDouble(dValue).ToString("P");//得到小数点后2位的百分比,自动 加上%号;//9512.35%
            //result = Convert.ToDouble(strValue).ToString("f4");//保留小数点后4位;  //95.1235

            res_msg += "\n高度图文件：" + client_hdtfile;
        }

        EditorUtility.DisplayDialog("成功", res_msg, "确定");
    }
}
