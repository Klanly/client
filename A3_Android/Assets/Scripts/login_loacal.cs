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
    ToggleGroup tg;

    List<serverData> serverList;


    void Start()
    {
        uid = this.transform.FindChild("uid").GetComponent<InputField>();
        tkn = this.transform.FindChild("tkn").GetComponent<InputField>();
        bt = this.transform.FindChild("bt").GetComponent<Button>();
        tg = this.transform.FindChild("s").GetComponent<ToggleGroup>();
        bt.onClick.AddListener(onCLick);

        serverList = new List<serverData>();
        serverData td;

        //内网测试服务器
        td = new serverData();
        td.ip = "10.1.8.60";
        td.port = 64999;
        td.sid = 1;
        td.clnt = 0;
        serverList.Add(td);

        td = new serverData();
        td.ip = "10.1.8.60";
        td.port = 63999;
        td.sid = 2;
        td.clnt = 0;
        serverList.Add(td);

        td = new serverData();
        td.ip = "10.1.8.60";
        td.port = 62999;
        td.sid = 3;
        td.clnt = 0;
        serverList.Add(td);

        td = new serverData();
        td.ip = "10.1.8.60";
        td.port = 61999;
        td.sid = 4;
        td.clnt = 0;
        serverList.Add(td);

        td = new serverData();
        td.ip = "10.1.6.162";
        td.port = 64999;
        td.sid = 1;
        td.clnt = 0;
        serverList.Add(td);

        td = new serverData();
        td.ip = "10.1.6.181";
        td.port = 64999;
        td.sid = 1;
        td.clnt = 0;
        serverList.Add(td);


        //外网服务器
        td = new serverData();
        td.ip = "qsmy-mobile.8090mt.com";
        td.port = 64999;
        td.sid = 1;
        td.clnt = 0;
        serverList.Add(td);

        td = new serverData();
        td.ip = "10.1.6.210";
        td.port = 64999;
        td.sid = 1;
        td.clnt = 0;
        serverList.Add(td);

        td = new serverData();
        td.ip = "10.1.6.217";
        td.port = 64999;
        td.sid = 1;
        td.clnt = 0;
        serverList.Add(td);

        initInfo();
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


         if (PlayeLocalInfo.checkKey(PlayeLocalInfo.DEBUG_SELECTED))
        {
            Toggle t = this.transform.FindChild("s" + PlayeLocalInfo.loadInt(PlayeLocalInfo.DEBUG_SELECTED)).GetComponent<Toggle>();
            t.isOn = true;
        }
        else
        {
            Toggle t = this.transform.FindChild("s1").GetComponent<Toggle>();
            t.isOn = true;
        }
    }

    void onCLick()
    {
        PlayeLocalInfo.saveString(PlayeLocalInfo.DEBUG_TKN,tkn.text);
        PlayeLocalInfo.saveString(PlayeLocalInfo.DEBUG_UID,uid.text);
        int idx=0;
        for (int i = 0; i < serverList.Count; i++)
        {
            Toggle t = this.transform.FindChild("s" + (i+1)).GetComponent<Toggle>();
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
	
	// Update is called once per frame
	void Update () {
	
	}
}

public struct serverData
{
 public   string ip;
 public uint port;
 public uint sid;
 public string server_config_url;
 public uint clnt;
}

