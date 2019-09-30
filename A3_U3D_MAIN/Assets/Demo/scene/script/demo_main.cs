using UnityEngine;
using System.Collections;
using Cross;
using MuGame;
using GameFramework;

public class demo_main : MonoBehaviour {

    static osImpl m_os;
    // Use this for initialization
    void Start()
    {
        debug.Log("开始Demo");
        //m_os = new osImpl();
        //m_os.init(this.gameObject, 1280, 720);

        debug.Log("开始....加载xml数据...............");
        //加载xml
        URLReqImpl urlreq_staticdata = new URLReqImpl();
        urlreq_staticdata.dataFormat = "binary";
        urlreq_staticdata.url = "staticdata/staticdata.dat";
        urlreq_staticdata.load((IURLReq url_req, object ret) =>
        {
            debug.Log("demo 加载数据xml...............");

            byte[] data = ret as byte[];
            ByteArray sd_data = new ByteArray(data);
            sd_data.uncompress();
            XMLMgr.instance.init(sd_data);
        },
        null,
        (IURLReq url_req, string err) =>
        {
            debug.Log("加载staticdata 失败。。。。。。。。。。。。" + url_req.url);
        });

        
        //Globle.Init_DEFAULT();

        Globle.A3_DEMO = true;
    
        Application.targetFrameRate = 15;

		new CrossApp(true);
		CrossApp.singleton.regPlugin(new gameEventDelegate());
		CrossApp.singleton.regPlugin(new processManager());
        //loadXml();

        //这里一定要设置下，不然小辣椒等手机，都会对齐出错
        Screen.SetResolution(Screen.width/4, Screen.height/4, true);

        //InterfaceMgr.getInstance().open("joystick");
        //InterfaceMgr.getInstance().open("skillbar");
        //InterfaceMgr.getInstance().open("a3_minimap");
        //InterfaceMgr.getInstance().open("a3_herohead");
        //InterfaceMgr.getInstance().open("a3_expbar");

        //GameRoomMgr.getInstance().onChangeLevel(new Variant());

        //SelfRole._inst.Init("profession/warrior_inst", EnumLayer.LM_SELFROLE);
		SceneCamera.Init();
		SelfRole.Init();

       // MonsterMgr._inst.AddMonster();

        Variant svrconf = new Variant();
        svrconf["id"] = 1;

        GRMap.changeMapTimeSt = 7;
		GRMap.grmap_loading = false;
        GameRoomMgr.getInstance().onChangeLevel(svrconf, null);
    }

    // Update is called once per frame
    void Update()
    {
        float fdt = Time.deltaTime;

        SceneCamera.FrameMove();

        MonsterMgr._inst.FrameMove(fdt);

        SelfRole.FrameMove(fdt);
        TickMgr.instance.update(fdt);

		(CrossApp.singleton.getPlugin("gameEventDelegate") as gameEventDelegate).onProcess(fdt);
        //gameEventDelegate.singleton.onProcess(fdt);
    }

    public void loadXml()
    {     
            //将从OutAssets/staticdata/staticdata.dat中解包出需要的xml
            URLReqImpl urlreq_staticdata = new URLReqImpl();
            urlreq_staticdata.dataFormat = "binary";
            urlreq_staticdata.url = "staticdata/staticdata.dat";
            urlreq_staticdata.load((IURLReq url_req, object ret) =>
            {
                byte[] data = ret as byte[];
                //debug.Log("加载staticdata 成功。。。。 len " + data.Length);

                ByteArray sd_data = new ByteArray(data);
                sd_data.uncompress();

                while (sd_data.bytesAvailable > 4)
                {
                    int name_len = sd_data.readInt();
                    string name_str = sd_data.readUTF8Bytes(name_len);
                    int data_len = sd_data.readInt();
                    string data_str = sd_data.readUTF8Bytes(data_len);

                    debug.Log("处理表 " + name_str + " 大小=" + data_len);

                    XMLMgr.instance.AddXmlData(name_str, ref data_str);

                    name_str = null;
                    data_str = null;
                }
            },
            null,
            (IURLReq url_req, string err) =>
            {
                debug.Log("加载staticdata 失败。。。。。。。。。。。。" + url_req.url);
            });

          
	
    }
}
