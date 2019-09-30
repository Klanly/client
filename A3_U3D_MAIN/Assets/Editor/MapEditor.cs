using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using MuGame;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Cross;
public class MapEditor : EditorWindow
{
    [MenuItem("Qinsmy/导出地图跳点")]
    static void parseMapPointXml()
    {
        string[] curProcessFile = Directory.GetFiles(Application.dataPath + "/../../OutAssets/svrconfig", "map.xml", SearchOption.AllDirectories);

        FileStream fs = new FileStream(curProcessFile[0], FileMode.Open);
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
            EditorUtility.DisplayDialog("", "表格式错误：" + curProcessFile[0],"");
        }
        Dictionary<string,int > dMapId = new  Dictionary<string,int >();

        foreach (XmlNode child in xmlDoc.DocumentElement.ChildNodes)
        {
            int id = 0;
            string mapname = "";

            if (child.Attributes==null)
                continue;
            foreach (XmlAttribute attr in child.Attributes)
            {
                if (attr.Name == "id")
                    id = int.Parse(attr.Value);
                else if (attr.Name == "name")
                    mapname = attr.Value;

                // debug.Log(attr.Name.ToString());
            }
            if (id == 0 || mapname == "")
            {
                EditorUtility.DisplayDialog("", "表参数错误：" + id + " " + mapname, "");
                return;
            }
            dMapId[mapname] =id ;
        }


        Dictionary<int, float> dMapscale = new Dictionary<int, float>();
        Dictionary<int, Vector3> dCanvasPosition = new Dictionary<int, Vector3>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string  assetPath= S.path;
                EditorApplication.OpenScene(assetPath);

                int begin = assetPath.LastIndexOf("/");
                int end = assetPath.LastIndexOf(".");
                string name = assetPath.Substring(begin + 1, end - begin - 1);

                foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
                {
                    if (obj.name == "MINIMAP_CANVAS")
                    {
                        if (!dMapId.ContainsKey(name))
                        {
                            EditorUtility.DisplayDialog("", "配置表找不到：" + name, "确定");
                            break;
                        }


                        if (name == "dcfx3_scene")
                        {
                            debug.Log("aaaaaaaaaaaa");
                        }

                        int idid = dMapId[name];
                        dMapscale[idid] = obj.transform.localScale.x;
                        dCanvasPosition[idid] = obj.transform.localPosition;
                        debug.Log(name + " " + obj.transform.localScale.x);
                        break;
                    }
                }
            }
        }
        EditorApplication.OpenScene("Assets/main.unity");

        

        XmlDocument xml = new XmlDocument();
        XmlElement root = xml.CreateElement("mappoint");

        string[] files = Directory.GetFiles(Application.dataPath + "/AB_RELEASE/mother_package/ab_ui/map/prefab", "*.prefab", SearchOption.TopDirectoryOnly);
        foreach (string file in files)
        {
            int begin = file.LastIndexOf("\\");
            int end = file.LastIndexOf(".");
            string assetPath = file.Substring(begin + 1, end - begin - 1);

            GameObject go = Resources.Load("map/prefab/" + assetPath) as GameObject;
            //GameObject go = GAMEAPI.ABUI_LoadPrefab(assetPath);

            float mapscale = go.transform.FindChild("map").localScale.x;
            int cost = int.Parse(go.transform.GetChild(0).gameObject.name);
            int mapid = int.Parse(go.name.Replace("map_prefab_map", ""));
          
            Transform con = go.transform.FindChild("npc");
            for (int i = 0; i < con.childCount; i++)
            {
               

                GameObject tempgo = con.GetChild(i).gameObject;

              //  float scale = dMapscale.ContainsKey(mapid) ? dMapscale[mapid] : 150;




                //Vector3 pos = SelfRole._inst.m_curModel.position;
                //scale = curMiniMapCanvas.transform.localScale.x / 1024f;
                //Vector3 tempos = curMiniMapCanvas.transform.localPosition;
                //// Vector2 picpos = new Vector2(512f-(pos.x / scale)-tempos.x,512f-( pos.z / scale)-tempos.z);
                //Vector2 picpos = new Vector2(-((pos.x - tempos.x) / scale), -((pos.z - tempos.z) / scale));
                //Vector2 vec = picpos * picscale;
                ////vec.x = vec.x - 512f * picscale;
                ////vec.y = vec.y - 512f * picscale;
                //return vec;




                //Vector3 pos = SelfRole._inst.m_curModel.position;
                //float curMiniMapCanvas = dMapscale.ContainsKey(mapid) ? dMapscale[mapid] : 150;
                //float scale = curMiniMapCanvas / 1024f;

                //Transform curMiniMapCanvasTrans = dMapTrans[mapid];
                //Vector3 tempos = curMiniMapCanvasTrans != null ? curMiniMapCanvasTrans.transform.localPosition : Vector3.zero;
                //Vector2 picpos = new Vector2(-((pos.x - tempos.x) / scale), -((pos.z - tempos.z) / scale));
                //Vector2 vec = picpos * mapscale;

                //return vec;

//-------------------------------------------------------
                Vector3 picpos = tempgo.transform.position / mapscale;
                float curMiniMapCanvas = dMapscale.ContainsKey(mapid) ? dMapscale[mapid] : 150;
                float scale = curMiniMapCanvas / 1024f;
            
                Vector3 vec;
                if (!dCanvasPosition.ContainsKey(mapid))
                {
                    vec = Vector3.zero;
                }
                else
                {
                    vec = new Vector3(dCanvasPosition[mapid].x - (picpos.x * scale), 0f, dCanvasPosition[mapid].z - (picpos.y * scale));
                }




                //-------------------------------------------------------
                // scale = dMapscale.ContainsKey(mapid) ? dMapscale[mapid] : 150;
                //scale = scale / 1024f;
                //Vector3 vec1 = tempgo.transform.position / mapscale;
                //vec1 = new Vector3(512f - vec1.x, 0f, 512f - vec1.y);
                // vec1 = vec1 * scale;

                // debug.Log("::::::::::" + vec1 + " " + vec + "  " + go.name);



                XmlElement element = xml.CreateElement("p");
                element.SetAttribute("id", (mapid * 100 + int.Parse(tempgo.name)).ToString());
                element.SetAttribute("mapid", mapid.ToString());
                element.SetAttribute("x", ((int)(vec.x * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());
                element.SetAttribute("y", ((int)(vec.z * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());
                element.SetAttribute("ux", vec.x.ToString());
                element.SetAttribute("uy", vec.z.ToString());
                element.SetAttribute("cost", cost.ToString());
                element.SetAttribute("type", "0");
                string str = tempgo.transform.FindChild("Text").GetComponent<Text>().text;
                element.SetAttribute("name", str);

                //if (tempgo.transform.GetChildCount() >= 3)
                //{
                //    string temp =tempgo.transform.GetChild(0).gameObject.name;
                //    string[] tempstr = temp.Split('_');
                //    element.SetAttribute("lv_up", tempstr[0]);
                //    element.SetAttribute("lv", tempstr[1]);
                //}

          
                root.AppendChild(element);

               
            }

             con = go.transform.FindChild("monster");
            for (int i = 0; i < con.childCount; i++)
            {
                //float scale = dMapscale.ContainsKey(mapid) ? dMapscale[mapid] : 150;
                //scale = scale / 1024f;

                GameObject tempgo = con.GetChild(i).gameObject;
                //Vector3 vec = new Vector3(512f - tempgo.transform.position.x, 0f, 512f - tempgo.transform.position.y);
                //vec = vec * scale;


                Vector3 picpos = tempgo.transform.position / mapscale;
                float curMiniMapCanvas = dMapscale.ContainsKey(mapid) ? dMapscale[mapid] : 150;
                float scale = curMiniMapCanvas / 1024f;

                Vector3 vec;
                if (!dCanvasPosition.ContainsKey(mapid))
                {
                    vec = Vector3.zero;
                }
                else
                {
                    vec = new Vector3(dCanvasPosition[mapid].x - (picpos.x * scale), 0f, dCanvasPosition[mapid].z - (picpos.y * scale));
                }





                XmlElement element = xml.CreateElement("p");
                element.SetAttribute("id", (mapid * 100 + int.Parse(tempgo.name)).ToString());
                element.SetAttribute("mapid", mapid.ToString());
                element.SetAttribute("x",((int)(vec.x * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());
                element.SetAttribute("y", ((int)(vec.z * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());
                element.SetAttribute("ux",  vec.x.ToString());
                element.SetAttribute("uy",  vec.z.ToString());
                element.SetAttribute("cost", cost.ToString());
                element.SetAttribute("type", "1");
                string str = tempgo.transform.FindChild("Text").GetComponent<Text>().text;
                element.SetAttribute("name", str);

                //if (tempgo.transform.GetChildCount() >= 3)
                //{
                //    string temp = tempgo.transform.GetChild(0).gameObject.name;
                //    string[] tempstr = temp.Split('_');
                //    element.SetAttribute("lv_up", tempstr[0]);
                //    element.SetAttribute("lv", tempstr[1]);
                //}


                root.AppendChild(element);

               
            }

            XmlElement xml1001=null;
            con = go.transform.FindChild("way");
            if (con != null)
            {
                for (int i = 0; i < con.childCount; i++)
                {
                    GameObject tempgo = con.GetChild(i).gameObject;

                    Vector3 picpos = tempgo.transform.position / mapscale;
                    float curMiniMapCanvas = dMapscale.ContainsKey(mapid) ? dMapscale[mapid] : 150;
                    float scale = curMiniMapCanvas / 1024f;

                    Vector3 vec;
                    if (!dCanvasPosition.ContainsKey(mapid))
                    {
                        vec = Vector3.zero;
                    }
                    else
                    {
                        vec = new Vector3(dCanvasPosition[mapid].x - (picpos.x * scale), 0f, dCanvasPosition[mapid].z - (picpos.y * scale));
                    }

                    if (mapid == 13)
                    {
                        debug.Log("aaaaaaaaaa");
                    }


                    string id = (mapid * 100 + int.Parse(tempgo.name)).ToString();

                    XmlElement element = xml.CreateElement("p");
                    element.SetAttribute("id", id);
                    element.SetAttribute("mapid", mapid.ToString());
                    element.SetAttribute("x", ((int)(vec.x * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());
                    element.SetAttribute("y", ((int)(vec.z * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());
                    element.SetAttribute("ux", vec.x.ToString());
                    element.SetAttribute("uy", vec.z.ToString());
                    element.SetAttribute("cost", cost.ToString());
                    element.SetAttribute("type", "2");
                    string str = tempgo.transform.FindChild("Text").GetComponent<Text>().text;
                    element.SetAttribute("name", str);

                    root.AppendChild(element);

                    if (id == "1001")
                    {
                        xml1001 = element.Clone() as XmlElement;
                        xml1001.SetAttribute("cost", "0");
                        xml1001.SetAttribute("id", "0");
                        xml1001.SetAttribute("type", "100");
                        root.AppendChild(xml1001);
                    }

                }
            }


            con = go.transform.FindChild("none");
            if (con != null)
            {
                for (int i = 0; i < con.childCount; i++)
                {
                    GameObject tempgo = con.GetChild(i).gameObject;

                    Vector3 picpos = tempgo.transform.position / mapscale;
                    float curMiniMapCanvas = dMapscale.ContainsKey(mapid) ? dMapscale[mapid] : 150;
                    float scale = curMiniMapCanvas / 1024f;

                    Vector3 vec;
                    if (!dCanvasPosition.ContainsKey(mapid))
                    {
                        vec = Vector3.zero;
                    }
                    else
                    {
                        vec = new Vector3(dCanvasPosition[mapid].x - (picpos.x * scale), 0f, dCanvasPosition[mapid].z - (picpos.y * scale));
                    }

                    string id = (mapid * 100 + int.Parse(tempgo.name)).ToString();

                    XmlElement element = xml.CreateElement("p");
                    element.SetAttribute("id", id);
                    element.SetAttribute("mapid", mapid.ToString());
                    element.SetAttribute("x", ((int)(vec.x * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());
                    element.SetAttribute("y", ((int)(vec.z * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());
                    element.SetAttribute("ux", vec.x.ToString());
                    element.SetAttribute("uy", vec.z.ToString());
                    element.SetAttribute("cost", cost.ToString());
                    element.SetAttribute("type", "3");
                    string str = tempgo.transform.FindChild("Text").GetComponent<Text>().text;
                    element.SetAttribute("name", str);

                    root.AppendChild(element);

                    if (id == "1001")
                    {
                        xml1001 = element.Clone() as XmlElement;
                        xml1001.SetAttribute("cost", "0");
                        xml1001.SetAttribute("id", "0");
                        xml1001.SetAttribute("type", "100");
                        root.AppendChild(xml1001);
                    }

                }
            }
            
        }


        xml.AppendChild(root);
        string path = Application.dataPath + "/../../xml_StaticData/mappoint.xml";
        //  string path = Application.dataPath + "/data2.xml";
        xml.Save(path);


        EditorUtility.DisplayDialog("", "导出完成", "");
    }






    [MenuItem("Qinsmy/导出地图")]
    static void open()
    {
        EditorWindow.GetWindow<MapEditor>(false, "地图信息导出编辑", true).Show();
    }

    public int mapid = 0;
    public string sceneName = "kqzd_scene";
    public string mapname = "";
    public bool pkstate;

    void OnGUI()
    {
        if (GUILayout.Button("导出地图信息")) build();

        mapid = EditorGUILayout.IntField("地图id", mapid);
        sceneName = EditorGUILayout.TextField("场景文件名", sceneName);
        EditorGUI.BeginDisabledGroup(mapid <= 0);
        {
            if (GUILayout.Button("创建地图信息")) creatMapInfo();
        }


        if (GUILayout.Button("创建怪物点")) creatMapPoint(0);
        if (GUILayout.Button("创建玩家点")) creatMapPoint(1);
        if (GUILayout.Button("创建安全区")) createZone(1);
        if (GUILayout.Button("创建pk区")) createZone(0);
        if (GUILayout.Button("创建地图跳转点")) createChangePoint();
    }

    void createZone(int type)
    {
        GameObject conGo = GameObject.Find("SVR_DATA");
        if (conGo == null)
            return;

        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);


        go.name = type == 1 ? "安全区" : "pk区";
        go.transform.SetParent(conGo.transform);
      //  SceneView.currentDrawingSceneView.MoveToView(go.transform);
        Zone zone = go.AddComponent<Zone>();
        zone.pkzone = type == 0;
        //if(type==1)
    }

    void creatMapInfo()
    {
        GameObject conGo = GameObject.Find("SVR_DATA");
        if (conGo != null)
            return;

        conGo = new GameObject();
        conGo.name = "SVR_DATA";
        MapInfo info = conGo.AddComponent<MapInfo>();
        info.地图id = mapid;
    }

    void createChangePoint()
    {
        GameObject conGo = GameObject.Find("SVR_DATA");
        if (conGo == null)
            return;

        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);



        go.name = "跳转点";
        go.transform.SetParent(conGo.transform);
     //   SceneView.currentDrawingSceneView.MoveToView(go.transform);

        SvrMapChangePoint mp = go.AddComponent<SvrMapChangePoint>();

    }

    void creatMapPoint(int type)
    {
        GameObject conGo = GameObject.Find("SVR_DATA");
        if (conGo == null)
            return;

        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

        go.name = type == 1 ? "玩家出生点" : "怪物点";
        go.transform.SetParent(conGo.transform);
       // SceneView.currentDrawingSceneView.MoveToView(go.transform);

        MapPoint mp = go.AddComponent<MapPoint>();
        mp.玩家出生点 = type == 1;
        //SceneView.lastActiveSceneView.pivot = position;
        //SceneView.lastActiveSceneView.Repaint();
    }

    void build()
    {
        GameObject conGo = GameObject.Find("SVR_DATA");
        if (conGo == null)
            return;
        XmlDocument xml = new XmlDocument();
        XmlElement root = xml.CreateElement("m");
        MapInfo mapinfo = conGo.GetComponent<MapInfo>();
        if (mapinfo == null)
            return;

        root.SetAttribute("id", mapinfo.地图id.ToString());
        root.SetAttribute("name", mapinfo.地图名.ToString());
        root.SetAttribute("tile_size", "32");
        root.SetAttribute("tile_set", "1");
        root.SetAttribute("pk", mapinfo.是否是pk地图 ? "1" : "0");

        GameObject mapacanvas = GameObject.Find("MINIMAP_CANVAS");
        if (mapacanvas == null)
            return;

        int w = (int)(mapacanvas.transform.localScale.x * 10 * GameConstant.GEZI_TRANS_UNITYPOS);
        int h = (int)(mapacanvas.transform.localScale.z * 10 * GameConstant.GEZI_TRANS_UNITYPOS);

        root.SetAttribute("width", w.ToString());
        root.SetAttribute("height", h.ToString());



        XmlElement element = xml.CreateElement("g");
        element.SetAttribute("file", "data/maps/maps/" + mapinfo.地图id + ".grd");
        getMapGrd(w, h, mapinfo.地图id);
        root.AppendChild(element);


        MapPoint[] mapPoints = conGo.GetComponentsInChildren<MapPoint>();
        foreach (MapPoint mp in mapPoints)
        {
            XmlElement ele = getXml(mp, xml);
            if (ele != null)
                root.AppendChild(ele);
        }
        xml.AppendChild(root);


        Zone[] mapZones = conGo.GetComponentsInChildren<Zone>();
        foreach (Zone z in mapZones)
        {
            XmlElement ele = getXml(z, xml);
            if (ele != null)
                root.AppendChild(ele);
        }
        xml.AppendChild(root);

        SvrMapChangePoint[] mapChangePoint = conGo.GetComponentsInChildren<SvrMapChangePoint>();
        foreach (SvrMapChangePoint z in mapChangePoint)
        {
            XmlElement ele = getXml(z, xml);
            if (ele != null)
                root.AppendChild(ele);
        }
        xml.AppendChild(root);

        string path = Application.dataPath + "/../../xml_ServerData/" + mapinfo.地图id + ".xml";
        //  string path = Application.dataPath + "/data2.xml";
        xml.Save(path);
        getMiniMapCanvas(mapinfo.地图id, 1024, 1024,0);
        getMiniMapCanvas(mapinfo.地图id, 1024, 1024, 1);
    }

    public XmlElement getXml(Zone z, XmlDocument xml)
    {
        XmlElement e;
        if (z.pkzone)
            e = xml.CreateElement("pk_zone");
        else
            e = xml.CreateElement("pc_zone");
        e.SetAttribute("x", ((int)(z.transform.position.x * GameConstant.PIXEL_TRANS_UNITYPOS)).ToString());
        e.SetAttribute("y", ((int)(z.transform.position.z * GameConstant.PIXEL_TRANS_UNITYPOS)).ToString());
        e.SetAttribute("width", ((int)(z.transform.localScale.x * 10 * GameConstant.PIXEL_TRANS_UNITYPOS)).ToString());
        e.SetAttribute("height", ((int)(z.transform.localScale.z * 10 * GameConstant.PIXEL_TRANS_UNITYPOS)).ToString());

        return e;
    }

    public XmlElement getXml(SvrMapChangePoint z, XmlDocument xml)
    {
        XmlElement e;

        e = xml.CreateElement("l");
        e.SetAttribute("id", z.id.ToString());
        e.SetAttribute("gto", z.目标地图id.ToString());
        e.SetAttribute("to_x", ((int)(z.目标x * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());
        e.SetAttribute("to_y", ((int)(z.目标y * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());
        e.SetAttribute("faceto", (z.目标点朝向 ).ToString());
        e.SetAttribute("x", ((int)(z.transform.position.x * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());
        e.SetAttribute("y", ((int)(z.transform.position.z * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());

        e.SetAttribute("uw", z.transform.localScale.x.ToString());
        e.SetAttribute("uh", z.transform.localScale.z.ToString());

        e.SetAttribute("ux", z.transform.position.x.ToString());
        e.SetAttribute("uy", z.transform.position.y.ToString());
        e.SetAttribute("uz", z.transform.position.z.ToString());

        return e;
    }

    public XmlElement getXml(MapPoint mp, XmlDocument xml)
    {
        if (mp.玩家出生点)
        {
            //   <born mpid="1" x="4681" y="3621" width="100" height="100" />

            XmlElement e = xml.CreateElement("born");
            e.SetAttribute("mpid", mp.玩家出生点地图id.ToString());

            e.SetAttribute("width", mp.范围W.ToString());
            e.SetAttribute("height", mp.范围H.ToString());

            if (mp.点x_非必填 == 0)
            {
                e.SetAttribute("x", ((int)(mp.transform.position.x * GameConstant.PIXEL_TRANS_UNITYPOS)).ToString());
                e.SetAttribute("y", ((int)(mp.transform.position.z * GameConstant.PIXEL_TRANS_UNITYPOS)).ToString());
            }
            else
            {
                e.SetAttribute("x", mp.点x_非必填.ToString());
                e.SetAttribute("y", mp.点y_非必填.ToString());
            }

            return e;
        }
        else if (mp.怪物id == 0)
            return null;
        XmlElement element = xml.CreateElement("m");
        element.SetAttribute("mid", mp.怪物id.ToString());

        element.SetAttribute("x", ((int)(mp.transform.position.x * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());
        element.SetAttribute("y", ((int)(mp.transform.position.z * GameConstant.GEZI_TRANS_UNITYPOS)).ToString());
        element.SetAttribute("r_x", "1");
        element.SetAttribute("r_y", "1");
        element.SetAttribute("spwan_time", mp.重生时间.ToString());
        element.SetAttribute("sideid", mp.阵营id.ToString());



        //XmlElement elementChild1 = xml.CreateElement("contents");

        //elementChild1.SetAttribute("name", "a");
        ////设置节点内面的内容
        //elementChild1.InnerText = "这就是你，你就是天狼";
        //XmlElement elementChild2 = xml.CreateElement("mission");
        //elementChild2.SetAttribute("map", "abc");
        //elementChild2.InnerText = "去吧，少年，去实现你的梦想";
        ////把节点一层一层的添加至xml中，注意他们之间的先后顺序，这是生成XML文件的顺序
        //element.AppendChild(elementChild1);
        //element.AppendChild(elementChild2);

        return element;
    }



    public void getMapGrd(int w, int h, int mapid)
    {
        short[] map_grd = new short[w * h];
        ////射线碰撞，找自己要的目标
        int n_canpass_cell = 0;
        RaycastHit rchit;
        Vector3 head_pos = new Vector3(0.0f, 65535.0f / 2.0f, 0.0f);
        Vector3 head_dir = new Vector3(0.0f, -1.0f, 0.0f);

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                //float fhit_hdt = 0.0f;
                //int hit_count = 0;
                int index = j * w + i;

                //head_pos.x = i * 0.6f + 0.3f;
                //head_pos.z = j * 0.6f + 0.3f;
                //if (Physics.Raycast(head_pos, head_dir, out rchit, 65535.0f))
                //{
                //    fhit_hdt += rchit.point.y;
                //    hit_count++;
                //}

                //head_pos.x = i * 0.6f;
                //head_pos.z = j * 0.6f;
                //if (Physics.Raycast(head_pos, head_dir, out rchit, 65535.0f))
                //{
                //    fhit_hdt += rchit.point.y;
                //    hit_count++;
                //}

                //head_pos.x = i * 0.6f + 0.6f;
                //head_pos.z = j * 0.6f + 0.6f;
                //if (Physics.Raycast(head_pos, head_dir, out rchit, 65535.0f))
                //{
                //    fhit_hdt += rchit.point.y;
                //    hit_count++;
                //}

                //head_pos.x = i * 0.6f + 0.6f;
                //head_pos.z = j * 0.6f;
                //if (Physics.Raycast(head_pos, head_dir, out rchit, 65535.0f))
                //{
                //    fhit_hdt += rchit.point.y;
                //    hit_count++;
                //}

                //head_pos.x = i * 0.6f;
                //head_pos.z = j * 0.6f + 0.6f;
                //if (Physics.Raycast(head_pos, head_dir, out rchit, 65535.0f))
                //{
                //    fhit_hdt += rchit.point.y;
                //    hit_count++;
                //}

                //只要有碰到就认为是可以行走的
                //  if (hit_count > 0)
                //     {
                //        fhit_hdt = fhit_hdt / hit_count;

                map_grd[index] = 0;
                //   n_canpass_cell++;
                //}
                //else
                //{
                //    map_grd[index] = 4096;
                //}
            }

            float percent = (float)i / h;
            EditorUtility.DisplayProgressBar("生成阻挡中...", i.ToString(), percent);
        }

        EditorUtility.ClearProgressBar();

        //服务端用的阻挡信息
        string output_filename_server = Application.dataPath + "/../../xml_ServerData/" + mapid + ".grd";
        //    string output_filename_server = Application.dataPath + "/../../xml_ServerData/1.grd";
        //  string output_filename_server = Application.dataPath + "/" + mapid + ".grd";

        FileStream msk_stream = new FileStream(output_filename_server, FileMode.Create);
        byte[] file_data = new byte[map_grd.Length * sizeof(short)];
        Buffer.BlockCopy(map_grd, 0, file_data, 0, file_data.Length);
        msk_stream.Write(file_data, 0, file_data.Length);
        msk_stream.Flush();

        //   EditorUtility.DisplayDialog("成功", res_msg, "确定");
    }


    private static UnityEngine.Rect CutRect = new UnityEngine.Rect(0, 0, 1, 1);
    public static void getMiniMapCanvas(int mapid, int width, int height,int type)
    {
        GameObject temogo = Resources.Load("camera/camera_minimap") as GameObject;
        GameObject minicam = GameObject.Instantiate(temogo) as GameObject;
        if (minicam == null)
            return;

        Transform transminiMap = minicam.transform.FindChild("camera");

        GameObject p = GameObject.Find("MINIMAP_CANVAS");
        if (p == null)
            return;

        GameObject go = new GameObject();
        Camera mCam = go.AddComponent<Camera>();

        //    mCam.rect = new Rect(.5f, .5f, .5f, .5f);

        mCam.backgroundColor = new Color(0f, 0f, 0f);
        mCam.orthographic = true;
        mCam.orthographicSize = p.transform.localScale.x * 0.5f;
        Vector3 vec = p.transform.position;
        vec.y = 50;
        go.transform.position = vec;

        go.transform.rotation = transminiMap.rotation;

        if (type==0)
        mCam.cullingMask = 1 << LayerMask.NameToLayer("scene_shadow");
        else
            mCam.cullingMask = 1 << LayerMask.NameToLayer("Default");


        RenderTexture rt = new RenderTexture(width, height, 2);
        //  mCam.pixelRect = new Rect(0, 0, Screen.width, Screen.height);
        mCam.pixelRect = new UnityEngine.Rect(0, 0, 1136, 640);
        //  mCam.pixelRect = new Rect(0, 0, 1, 1);

        mCam.targetTexture = rt;
        Texture2D screenShot = new Texture2D((int)(width * CutRect.width), (int)(height * CutRect.height),
                                                 TextureFormat.RGB24, false);



        RenderTexture.active = rt;
        mCam.Render();

        screenShot.ReadPixels(new UnityEngine.Rect(width * CutRect.x, height * CutRect.y, width * CutRect.width, height * CutRect.height), 0, 0);
        //screenShot.Apply();
        //p.renderer.material.SetTexture("_MainTex", screenShot);

        byte[] bytes = screenShot.EncodeToPNG();
        string filename;
        if(type==0)
       filename=Application.dataPath + "/../../A3_U3D_MAIN/Assets/A3/interface/resources/map/" + "map" + mapid + ".png";
        else
            filename = Application.dataPath + "/../../A3_U3D_MAIN/Assets/A3/interface/resources/map/" + "map" + mapid + "_w.png";
        System.IO.File.WriteAllBytes(filename, bytes);

        mCam.targetTexture = null;
        RenderTexture.active = null;
        UnityEngine.Object.DestroyImmediate(rt);

        GameObject.DestroyImmediate(go);
        GameObject.DestroyImmediate(minicam);

    }


   


}

