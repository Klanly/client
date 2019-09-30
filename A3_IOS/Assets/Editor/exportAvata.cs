using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class exportAvata : Editor
{
    static XmlDocument xmlDoc;
    static XmlElement maps;
    static XmlElement n;
    static XmlElement m;
    static XmlElement l;
    static XmlElement mapid;
    static XmlElement g;
    static XmlElement born;
    static XmlElement pk_zone;
    static int m_const1 = 100;
    static int m_const2 = 60;
    static float m_const3 = 53.3f;
	

    static void ExportXml(object[] objs)
    {
        if (objs.Length == 0)
        {
            EditorUtility.DisplayDialog("Export SceneObj", "Please select one or more target objects", "OK");
            return;
        }
        string path_xml = EditorUtility.SaveFilePanel("Export Sceneobj Config", "", "", "xml");
        if (path_xml == "")
            return;
        xmlDoc = new XmlDocument();
        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", ""));
        maps = xmlDoc.CreateElement("maps");
        xmlDoc.AppendChild(maps);
        for (int a = 0; a < objs.Length; a++)
        {
            GameObject obj_p = objs[a] as GameObject;
            if (obj_p.transform.parent == null)
            {
                mapid = xmlDoc.CreateElement("m");
                mapid.SetAttribute("id", obj_p.name);
                mapid.SetAttribute("name", "{map_xml##mapname_"+obj_p.name+"}");
                mapid.SetAttribute("tile_size", "32");
                mapid.SetAttribute("width", "xx");
                mapid.SetAttribute("height", "xx");
                mapid.SetAttribute("tile_set", "1");
                mapid.SetAttribute("pk", "2");

                int i_m = 0;
                int i_n = 0;
                List<GameObject> m_objs = new List<GameObject>();
                for (int h = 0; h < obj_p.transform.childCount; h++)
                {
                    GameObject obj = obj_p.transform.GetChild(h).gameObject;
                    Animation obj_animation = obj.GetComponent<Animation>();
                    if (obj_animation != null)
                        m_objs.Add(obj);
                }
                for (int k = 0; k < m_objs.Count; k++)
                {
                    GameObject objl = m_objs[k] as GameObject;
                    if (objl == null)
                        return;
                    if (objl.transform.parent.transform.parent == null)
                    {
                        string namel = objl.name;
                        if (namel[0] == 'l')
                        {
                            l = xmlDoc.CreateElement("l");
                            l.SetAttribute("gto", "xx");
                            l.SetAttribute("x", (objl.transform.position.x > 0) ? (int)(objl.transform.position.x * m_const1 / m_const2) + "" : "0");
                            l.SetAttribute("y", (objl.transform.position.z > 0) ? (int)(objl.transform.position.z * m_const1 / m_const2) + "" : "0");
                            l.SetAttribute("to_x", "xx");
                            l.SetAttribute("to_y", "xx");
                            mapid.AppendChild(l);
                        }
                    }
                    i_n++;
                    if (i_n >= m_objs.Count)
                    {
                        for (int i = 0; i < m_objs.Count; i++)
                        {
                            GameObject objn = m_objs[i] as GameObject;
                            if (objn == null)
                                return;
                            if (objn.transform.parent.transform.parent == null)
                            {
                                string namen = objn.name;
                                if (namen[0] == 'n')
                                {
                                    n = xmlDoc.CreateElement("n");
                                    n.SetAttribute("nid", namen.Substring(1));
                                    n.SetAttribute("x", (objn.transform.position.x > 0) ? (int)(objn.transform.position.x * m_const1 / m_const2) + "" : "0");
                                    n.SetAttribute("y", (objn.transform.position.z > 0) ? (int)(objn.transform.position.z * m_const1 / m_const2) + "" : "0");
                                    n.SetAttribute("r", "6");
                                    mapid.AppendChild(n);
                                }
                            }
                            i_m++;
                            if (i_m >= m_objs.Count)
                            {
                                g = xmlDoc.CreateElement("g");
                                g.SetAttribute("file", "data/maps/maps/" + obj_p.name + ".grd");
                                mapid.AppendChild(g);
                                for (int j = 0; j < m_objs.Count; j++)
                                {
                                    GameObject objm = m_objs[j] as GameObject;
                                    if (objm == null)
                                        return;
                                    if (objm.transform.parent.transform.parent == null)
                                    {
                                        string namem = objm.name;
                                        if (namem[0] == 'm')
                                        {
                                            m = xmlDoc.CreateElement("m");
                                            m.SetAttribute("mid", namem.Substring(1));
                                            m.SetAttribute("x", (objm.transform.position.x > 0) ? (int)(objm.transform.position.x * m_const1 / m_const2) + "" : "0");
                                            m.SetAttribute("y", (objm.transform.position.x > 0) ? (int)(objm.transform.position.z * m_const1 / m_const2) + "" : "0");
                                            m.SetAttribute("r_x", "1");
                                            m.SetAttribute("r_y", "1");
                                            m.SetAttribute("spwan_time", "3000");
                                            mapid.AppendChild(m);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                for (int h = 0; h < obj_p.transform.childCount; h++)
                {
                    GameObject obj = obj_p.transform.GetChild(h).gameObject;
                    if (obj.name.Contains("born"))
                    {
                        born = xmlDoc.CreateElement("born");
                        born.SetAttribute("mpid", obj_p.name);
						born.SetAttribute("x", ((obj.transform.position.x - (obj.transform.lossyScale.x / 2)) > 0) ? ((int)((obj.transform.position.x - (obj.transform.lossyScale.x / 2)) * m_const3)).ToString() : "0");
						born.SetAttribute("y", ((obj.transform.position.z - (obj.transform.lossyScale.z / 2)) > 0) ? ((int)((obj.transform.position.z - (obj.transform.lossyScale.z / 2)) * m_const3)).ToString() : "0");
                        born.SetAttribute("width", ((int)(obj.transform.lossyScale.x  * m_const3)).ToString());
                        born.SetAttribute("height", ((int)(obj.transform.lossyScale.z  * m_const3)).ToString());
                        mapid.AppendChild(born);
                    }
                    if (obj.name.Contains("pk_zone"))
                    {
                        pk_zone = xmlDoc.CreateElement("pk_zone");
                        pk_zone.SetAttribute("mpid", obj_p.name);
						pk_zone.SetAttribute("x", ((obj.transform.position.x - (obj.transform.lossyScale.x / 2)) > 0) ? ((int)((obj.transform.position.x - (obj.transform.lossyScale.x / 2)) * m_const3)).ToString() : "0");
						pk_zone.SetAttribute("y", ((obj.transform.position.z - (obj.transform.lossyScale.z / 2)) > 0) ? ((int)((obj.transform.position.z - (obj.transform.lossyScale.z / 2)) * m_const3)).ToString() : "0");
                        pk_zone.SetAttribute("width", ((int)(obj.transform.lossyScale.x  * m_const3)).ToString());
                        pk_zone.SetAttribute("height", ((int)(obj.transform.lossyScale.z  * m_const3)).ToString());
                        mapid.AppendChild(pk_zone);
                    }
                }
                maps.AppendChild(mapid);
            }
        }
        xmlDoc.Save(path_xml);
        AssetDatabase.Refresh();
    }
    [MenuItem("Assets/[CrossMono]/map导出场景所有map配置")]
    static void all()
    {
        object[] objs = GameObject.FindObjectsOfType(typeof(GameObject));
        ExportXml(objs);
    }
    [MenuItem("Assets/[CrossMono]/map导出选择的map配置")]
    static void one()
    {
        object[] objs = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);
        ExportXml(objs);
    }
}
