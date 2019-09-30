using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MuGame;

public class ExportBitmapBates : Editor
{
    static List<byte> m_garymax = new List<byte>();
    static List<byte> m_gary = new List<byte>();
    static short[] m_Collision;
    static short[] m_local;
    static byte[] m_byt;
    static short m_pixelWidth;
    static short m_pixelHeight;
    static int x_count;                                         //宽的格子总数
    static int y_count;                                         //高的格子总数
    static int m_sideLength = 32;                               //要求碰撞图格子的像素宽高
    static int m_hdtlength = 5;                                 //灰度图格子信息
    [MenuItem("Assets/[CrossMono]/map导出碰撞信息和灰度图(暂时不用)")]
    static public void export()
    {
        m_garymax.Clear();
        m_gary.Clear();
        string path = EditorUtility.OpenFilePanel("", "", "");
        if (path != "")
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader br = new BinaryReader(fs);
            char cha1 = br.ReadChar();
            char cha2 = br.ReadChar();
            char cha3 = br.ReadChar();
            char cha4 = br.ReadChar();
            short num1 = br.ReadInt16();
            m_pixelWidth = br.ReadInt16();
            m_pixelHeight = br.ReadInt16();
            float m_proportion = br.ReadSingle();
            float m_heightMin = br.ReadSingle();
            float m_heightMax = br.ReadSingle();
            Debug.Log(cha1 + cha2 + cha3 + cha4 + num1.ToString() + m_proportion.ToString() + m_heightMin.ToString() + m_heightMax.ToString());
            m_byt = new byte[m_pixelWidth * m_pixelHeight];
            m_byt = br.ReadBytes(m_pixelWidth * m_pixelHeight);

            for (int i = 0; i < m_pixelHeight; i += m_sideLength)
            {
                for (int j = 0; j < m_pixelWidth; j += m_sideLength)
                {
                    List<byte> m_all = new List<byte>();            // 每一个格子包含的灰度值数组
                    for (int a = i; a < i + m_sideLength; a++)
                    {
                        for (int b = j; b < j + m_sideLength; b++)
                        {
                            if (a * m_pixelWidth + b <= m_byt.Length - 1)
                                m_all.Add(m_byt[a * m_pixelWidth + b]);
                        }
                    }
                    byte temp = 0;
                    for (int c = 0; c < m_all.Count; c++)
                    {
                        temp = m_all[c];
                        if (temp == 0)
                            break;
                    }
                    m_garymax.Add(temp);
                }
            }
            for (int i = 0; i < m_pixelHeight; i += m_hdtlength)
            {
                for (int j = 0; j < m_pixelWidth; j += m_hdtlength)
                {
                    List<byte> m_all = new List<byte>();            // 每一个格子包含的灰度值数组
                    for (int a = i; a < i + m_hdtlength; a++)
                    {
                        for (int b = j; b < j + m_hdtlength; b++)
                        {
                            if (a * m_pixelWidth + b <= m_byt.Length - 1)
                                m_all.Add(m_byt[a * m_pixelWidth + b]);
                        }
                    }
                    byte temp = 0;
                    int sum = 0;
                    for (int c = 0; c < m_all.Count; c++)
                    {
                        temp = m_all[c];
                        sum += temp;
                        if (temp == 0)
                        {
                            sum = 0;
                            break;
                        }
                    }
                    m_gary.Add((byte)(sum / m_all.Count));
                }
            }
            m_Collision = new short[m_garymax.Count];
            m_local = new short[m_garymax.Count];


            if (m_pixelWidth % m_sideLength != 0)
                x_count = m_pixelWidth / m_sideLength + 1;
            else
                x_count = m_pixelWidth / m_sideLength;
            if (m_pixelHeight % m_sideLength != 0)
                y_count = m_pixelHeight / m_sideLength + 1;
            else
                y_count = m_pixelHeight / m_sideLength;

            //客户端,服务器碰撞信息赋值
            for (int i = 0; i < m_garymax.Count; i++)
            {
                if (m_garymax[i] == 0)
                {
                    m_local[i] = 0;
                    m_Collision[i] = 0;
                }
                else
                {
                    m_local[i] = 0;
                    m_Collision[i] = 0;
                }
            }
            ////对不能整除的边角格子进行处理
            //if (m_pixelWidth % m_sideLength != 0)
            //{
            //    for (int l = m_pixelWidth / m_sideLength; l < m_Collision.Length; l += x_count)
            //    {
            //        m_Collision[l] = 1;
            //        m_local[l] = 1;
            //    }
            //}
            //if (m_pixelHeight % m_sideLength != 0)
            //{
            //    for (int m = m_Collision.Length - x_count - 1; m < m_Collision.Length; m++)
            //    {
            //        m_Collision[m] = 1;
            //        m_local[m] = 1;
            //    }
            //}
            //匹配服务器，需要翻转数据
            for (int o = 0; o < m_Collision.Length / 2; o++)
            {
                short temp = m_Collision[o];
                int index = (y_count - 1 - o / x_count) * x_count + (o - o / x_count * x_count);
                m_Collision[o] = m_Collision[index];
                m_Collision[index] = temp;
            }
            debug.Log("碰撞信息格子边长=" + m_sideLength + "\n" + "碰撞信息宽的格子数=" + x_count + "\n" + "碰撞信息高的格子数=" + y_count + "\n" 
                + "灰度图像素宽=" + m_pixelWidth + "\n" + "灰度图像素高=" + m_pixelHeight);
                
            //导出grd
            string path_grd = EditorUtility.SaveFilePanel("", "", "", "grd");
            if (path_grd != "")
            {
                FileStream fsgrd = new FileStream(path_grd, FileMode.Create, FileAccess.Write);
                BinaryWriter bwgrd = new BinaryWriter(fsgrd);
                foreach (short value in m_Collision)
                {
                    bwgrd.Write(value);
                }
            }
            //导出localgrd
            string path_localgrd = EditorUtility.SaveFilePanel("", "", "", "localgrd");
            if (path_localgrd != "")
            {
                FileStream fslocal = new FileStream(path_localgrd, FileMode.Create, FileAccess.Write);
                BinaryWriter bwloacal = new BinaryWriter(fslocal);
                foreach (short value in m_local)
                {
                    bwloacal.Write(value);
                }
            }
            //导出hdt
            string path_hdt = EditorUtility.SaveFilePanel("", "", "", "hdt");
            if (path_hdt != "")
            {
                FileStream fshdt = new FileStream(path_hdt, FileMode.Create, FileAccess.Write);
                BinaryWriter bwhdt = new BinaryWriter(fshdt);
                bwhdt.Write(m_pixelWidth);
                bwhdt.Write(m_pixelHeight);
                foreach (byte value in m_gary)
                {
                    bwhdt.Write(value);
                }
            }
        }
    }
}
