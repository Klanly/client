using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class testconfig : Editor
{
    static XmlElement gameobj;
    static GameObject obj;
    static XmlElement position;
    static XmlElement rotation;
    static XmlElement scale;
    static XmlDocument xmlDoc;
    static XmlElement map;
    static List<GameObject> m_objs = new List<GameObject>();

    static void objtransform(GameObject obj)
    {

        gameobj.SetAttribute("id", obj.name);
        // position 
        position = xmlDoc.CreateElement("pos");
        position.SetAttribute("x", obj.transform.position.x + "");
        position.SetAttribute("y", obj.transform.position.y + "");
        position.SetAttribute("z", obj.transform.position.z + "");

        // rotation
        rotation = xmlDoc.CreateElement("rot");
        rotation.SetAttribute("x", obj.transform.eulerAngles.x + "");
        rotation.SetAttribute("y", obj.transform.eulerAngles.y + "");
        rotation.SetAttribute("z", obj.transform.eulerAngles.z + "");

        // scale
        scale = xmlDoc.CreateElement("scale");
        scale.SetAttribute("x", obj.transform.lossyScale.x + "");
        scale.SetAttribute("y", obj.transform.lossyScale.y + "");
        scale.SetAttribute("z", obj.transform.lossyScale.z + "");

        gameobj.AppendChild(position);
        gameobj.AppendChild(rotation);
        gameobj.AppendChild(scale);
    }
    static void main(GameObject obj1)
    {
        m_objs.Clear();
        m_objs.Add(obj1);
        
            //for(int i=0;i<obj.transform.childCount;i++)
            //{
            //    GameObject a=obj.transform.GetChild(i).gameObject;
            //    MeshRenderer obj_mesh=a.GetComponent<MeshRenderer>();
            //    MeshFilter obj_meshfilter = a.GetComponent<MeshFilter> ();
            //    ParticleSystem obj_particlesystem = a.GetComponent<ParticleSystem> ();
            //    if(obj_mesh!=null&&obj_meshfilter!=null)
            //    {
            //        gameobj = xmlDoc.CreateElement ("Mesh");
            //        XmlElement asset = xmlDoc.CreateElement ("asset");
            //        string mesh_path = AssetDatabase.GetAssetPath (PrefabUtility.GetPrefabParent (a));
            //        int mesh_index = mesh_path.LastIndexOf (".");
            //        string mesh_file = mesh_path.Substring (17, mesh_index - 17);
            //        asset.SetAttribute ("file", mesh_file);
            //        gameobj.AppendChild (asset);
            //        objtransform(obj);

            //        map.AppendChild(gameobj);
            //        return;
                //}else if(obj_particlesystem!=null)
                //{
                //    gameobj = xmlDoc.CreateElement ("Particles");

                //    XmlElement asset = xmlDoc.CreateElement ("asset");
                //    string par_path = AssetDatabase.GetAssetPath (PrefabUtility.GetPrefabParent (obj_particlesystem));
                //    int par_index = par_path.LastIndexOf (".");
                //    string par_file = par_path.Substring (17, par_index - 17);
                //    asset.SetAttribute ("file", par_file);
                //    gameobj.AppendChild (asset);
                //    objtransform(obj);
                //    gameobj.SetAttribute("loop",obj_particlesystem.loop+"");

                //    map.AppendChild(gameobj);
                //    return;
                //}
            //}
            while (m_objs.Count > 0)
            {
                GameObject obj = m_objs[0];
                m_objs.RemoveAt(0);
                MeshRenderer obj_mesh = obj.GetComponent<MeshRenderer>();
                MeshFilter obj_meshfilter = obj.GetComponent<MeshFilter>();
                ParticleSystem obj_particlesystem = obj.GetComponent<ParticleSystem>();

                if (obj_mesh != null && obj_meshfilter != null)
                {
                    gameobj = xmlDoc.CreateElement("Mesh");
                    XmlElement asset = xmlDoc.CreateElement("asset");
                    string mesh_path = AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(obj));
                    int mesh_index = mesh_path.LastIndexOf(".");
                    string mesh_file = mesh_path.Substring(17, mesh_index - 17);
                    asset.SetAttribute("file", mesh_file);
                    gameobj.AppendChild(asset);
                    objtransform(obj1);

                    map.AppendChild(gameobj);
                    return;
                }else if(obj_particlesystem!=null)
                {
                    gameobj = xmlDoc.CreateElement ("Particles");

                    XmlElement asset = xmlDoc.CreateElement ("asset");
                    string par_path = AssetDatabase.GetAssetPath (PrefabUtility.GetPrefabParent (obj_particlesystem));
                    int par_index = par_path.LastIndexOf (".");
                    string par_file = par_path.Substring (17, par_index - 17);
                    asset.SetAttribute ("file", par_file);
                    gameobj.AppendChild (asset);
                    objtransform(obj1);
                    gameobj.SetAttribute("loop",obj_particlesystem.loop.ToString());

                    map.AppendChild(gameobj);
                    return;
                }
                if (obj.transform.childCount > 0)
                {
                    for (int i = 0; i < obj.transform.childCount; i++)
                    {
                        m_objs.Add(obj.transform.GetChild(i).gameObject);
                    }
                }
            }
    }
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
        map = xmlDoc.CreateElement("map");
        map.SetAttribute("id", "");
        //env
        XmlElement env = xmlDoc.CreateElement("env");
        //fog
        XmlElement fog = xmlDoc.CreateElement("fog");
        XmlElement fogcor = xmlDoc.CreateElement("color");
        fogcor.SetAttribute("r", RenderSettings.fogColor.r + "");
        fogcor.SetAttribute("g", RenderSettings.fogColor.g + "");
        fogcor.SetAttribute("b", RenderSettings.fogColor.b + "");
        fogcor.SetAttribute("a", RenderSettings.fogColor.a + "");

        fog.SetAttribute("display", RenderSettings.fog + "");
        fog.SetAttribute("density", RenderSettings.fogDensity + "");
        fog.SetAttribute("mode", RenderSettings.fogMode + "");
        fog.SetAttribute("distBegin", RenderSettings.fogStartDistance + "");
        fog.SetAttribute("distEnd", RenderSettings.fogEndDistance + "");
        fog.AppendChild(fogcor);

        //ambientLight 
        XmlElement amblight = xmlDoc.CreateElement("amblight");
        XmlElement ambcor = xmlDoc.CreateElement("color");
        ambcor.SetAttribute("r", RenderSettings.ambientLight.r + "");
        ambcor.SetAttribute("g", RenderSettings.ambientLight.g + "");
        ambcor.SetAttribute("b", RenderSettings.ambientLight.b + "");
        ambcor.SetAttribute("a", RenderSettings.ambientLight.a + "");
        amblight.AppendChild(ambcor);


        env.AppendChild(fog);
        env.AppendChild(amblight);
        map.AppendChild(env);
        for (int i = 0; i < objs.Length; i++)
        {
            obj = objs[i] as GameObject;
            if (objs[i] == null)
                return;
            if (obj.transform.parent == null)
            {
                main(obj);
            }
            xmlDoc.AppendChild(map);
            xmlDoc.Save(path_xml);
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/[CrossMono]/map导出Hierarchy视图中所有的地图信息")]
    static void exportall()
    {
        object[] objs = GameObject.FindObjectsOfType(typeof(GameObject));
        ExportXml(objs);
    }
    [MenuItem("Assets/[CrossMono]/map导出选中地图的地图信息")]
    static void exportone()
    {
        object[] objs = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);
        ExportXml(objs);
    }
}
