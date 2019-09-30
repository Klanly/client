using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MuGame;
using System.Collections.Generic;
public class login_loacal : MonoBehaviour
{

    InputField uid;
    InputField tkn;
    Button bt;
    Button btn_serverSelect;
    ToggleGroup tg;
    Text textServer;
    GameObject serverInfo;
    Transform servers;
    Button zhanghao;

    List<serverData> serverList;
    

    void Start()
    {
        uid = this.transform.FindChild("uid").GetComponent<InputField>();
        tkn = this.transform.FindChild("tkn").GetComponent<InputField>();
        bt = this.transform.FindChild("bt").GetComponent<Button>();
        serverInfo = this.transform.FindChild("serverPanel").gameObject;
        servers = this.transform.FindChild("serverPanel/servers");
        btn_serverSelect = this.transform.FindChild("serverInfo/btn_selectServer").GetComponent<Button>();
        textServer = this.transform.FindChild("serverInfo/txt_server").GetComponent<Text>();
        tg = this.transform.FindChild("s").GetComponent<ToggleGroup>();
        bt.onClick.AddListener(onCLick);
        btn_serverSelect.onClick.AddListener(onServerSlectClick);
        zhanghao = this.transform.FindChild("idbtn").GetComponent<Button>();
        zhanghao.onClick.AddListener(onZhanghao);

        serverList = new List<serverData>();
        serverData td;

        //内网测试服务器
        td = new serverData();
        td.ip = "10.1.8.60";
        td.port = 63999;
        td.sid = 2;
        td.clnt = 0;
        serverList.Add(td);

        td = new serverData();
        td.ip = "10.1.8.76";
        td.port = 63999;
        td.sid = 2;
        td.clnt = 0;
        serverList.Add(td);

        td = new serverData();
        td.ip = "10.1.8.76";
        td.port = 62999;
        td.sid = 3;
        td.clnt = 0;
        serverList.Add(td);

        td = new serverData();
        td.ip = "10.1.8.76";
        td.port = 61999;
        td.sid = 4;
        td.clnt = 0;
        serverList.Add(td);

        //压力测试服
        td = new serverData();
        td.ip = "120.132.13.141";
        td.port = 63999;
        td.sid = 1;
        td.clnt = 0;
        serverList.Add(td);

        td = new serverData();
        td.ip = "10.1.6.181";
        td.port = 54999;
        td.sid = 1;
        td.clnt = 0;
        serverList.Add(td);


        //外网服务器
        td = new serverData();
        //td.ip = "qsmy-mobile.8090mt.com";
		td.ip = "a3.test.utogame.com";
        //td.port = 64999;
		td.port = 65019;
        td.sid = 1;
        td.clnt = 0;
        serverList.Add(td);

        td = new serverData();
        td.ip = "10.1.6.240";
        td.port = 64999;
        td.sid = 1;
        td.clnt = 0;
        serverList.Add(td);

        td = new serverData();
        td.ip = "10.1.8.7";
        td.port = 64999;
        td.sid = 1;
        td.clnt = 0;
        serverList.Add(td);


        td = new serverData();
        td.ip = "123.206.222.192";
        td.port = 65069;
        td.sid = 1;
        td.clnt = 0;
        serverList.Add(td);

        initInfo();
        setToggleEvent();
    }

    void initInfo()
    {

     
        if (PlayeLocalInfo.checkKey(PlayeLocalInfo.DEBUG_TKN))
        {
            tkn.text = PlayeLocalInfo.loadString(PlayeLocalInfo.DEBUG_TKN);
        }
        else
        {
           
        }

        if (PlayeLocalInfo.checkKey(PlayeLocalInfo.DEBUG_UID))
        {
            uid.text = PlayeLocalInfo.loadString(PlayeLocalInfo.DEBUG_UID);
        }
        else
        {
           
        }

        if (tkn.text == "" && uid.text == "")
        {
            tkn.gameObject.SetActive(true);
            uid.gameObject.SetActive(true);
        }
        else if (tkn.text != "" && uid.text != "")
        {
            tkn.gameObject.SetActive(false);
            uid.gameObject.SetActive(false);
        }


        if (PlayeLocalInfo.checkKey(PlayeLocalInfo.DEBUG_SELECTED))
        {
            Toggle t = this.transform.FindChild("serverPanel/servers/s" + PlayeLocalInfo.loadInt(PlayeLocalInfo.DEBUG_SELECTED)).GetComponent<Toggle>();
            t.isOn = true;
            textServer.text = t.transform.FindChild("Label").GetComponent<Text>().text;
        }
        else
        {

            Toggle t = this.transform.FindChild("serverPanel/servers/s7").GetComponent<Toggle>();
            t.isOn = true;
            textServer.text = t.transform.FindChild("Label").GetComponent<Text>().text;
        }
  
    }
   
    void setToggleEvent()
    {
        int serverCount = servers.childCount;
        for (int i = 0; i < serverCount; i++)
        {

            servers.transform.GetChild(i).gameObject.GetComponent<Toggle>().onValueChanged.AddListener(onToggleClick);
        }
    
    
    }
   public void onToggleClick(bool isON)
   {
       foreach (var item in tg.ActiveToggles())
       {
           if (item.isOn)
           {
               textServer.text = item.transform.FindChild("Label").GetComponent<Text>().text;
               if (serverInfo.activeSelf) serverInfo.SetActive(false);
               break;
           }
       }
    }
    void onCLick()
    {
        PlayeLocalInfo.saveString(PlayeLocalInfo.DEBUG_TKN,tkn.text);
        PlayeLocalInfo.saveString(PlayeLocalInfo.DEBUG_UID,uid.text);

        int idx=0;
        for (int i = 0; i < serverList.Count; i++)
        {
            Toggle t = this.transform.FindChild("serverPanel/servers/s" + (i + 1)).GetComponent<Toggle>();
            if (t.isOn)
            {
                idx = i;
                PlayeLocalInfo.saveInt(PlayeLocalInfo.DEBUG_SELECTED, i+1);
                break;
            }
        }

        Main.instance.initParam(uint.Parse(uid.text),tkn.text,serverList[idx]);
        Destroy(this.gameObject);
    }
    void onZhanghao()
    {
        if (tkn.gameObject.activeSelf == false || uid.gameObject.activeSelf == false)
        {
            tkn.gameObject.SetActive(true);
            uid.gameObject.SetActive(true);
        }
    }
    void onServerSlectClick()
    {
        if (!serverInfo.activeSelf) serverInfo.SetActive(true); 
    }
	// Update is called once per frame
	void Update () {
	
	}
}


