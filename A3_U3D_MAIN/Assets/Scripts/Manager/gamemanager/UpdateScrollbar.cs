
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System.Reflection;
using System.IO;
//using Junfine.Debuger;
using UnityEngine.UI;
using DG.Tweening;
using SimpleFramework.Manager;
using MuGame;

namespace SimpleFramework
{
    public class UpdateScrollbar : MonoBehaviour
    {
        private static UpdateScrollbar _inst;
        public static void SetShow_Txt(string txt)
        {
            if( _inst != null && _inst.lineTxt != null)
            {
                _inst.lineTxt.text = txt;
            }
        }

        private Image m_scroll_bar;
        private GameObject line;
        private Action m_funcCallBack;
        private bool m_fOver = false;
        private bool m_bStartEnterLogin = false;
        public Action clickcomfirmHandle;
        public Action clickHandle;
        public GameObject comfirmBox;
        public GameObject msgBox1;
        public Text msgTxt;
        private Text txt;

        public Text lineTxt;

        GameManager gameMgr;

        public void init(GameManager mgr)
        {
            _inst = this;

            gameMgr = mgr;

            gameMgr.onBeginHelperHandle = onBeginHelp;
            gameMgr.onEndHelperHandle = onEndHelp;
            gameMgr.onCheckingVerHandle = onCheckingVer;
            gameMgr.onCheckingUpdateHandle = onCheckUpdate;
            gameMgr.onEndCheckingHandle = onEndCheck;

            gameMgr.onResourceLoadedOverhandle = dispose;
            gameMgr.onNeedupdateclientver = onNeedUpdateVer;
            gameMgr.onInitingHandle = onInit;
            gameMgr.onInitOverHandle = onInitOver;
            gameMgr.onComfirmUpdate = oncomfirmUpdate;
            gameMgr.onComFirmWifihandle = onComFirmWifihandle;
            gameMgr.onUpdateFailhanlde = onfail;
            gameMgr.onAllDone = loadCompleted;


            line = transform.FindChild("line").gameObject;

            m_scroll_bar = transform.FindChild("line/Image").gameObject.GetComponent<Image>();
            m_scroll_bar.fillAmount = 0;

            lineTxt = transform.FindChild("line/Text").GetComponent<Text>();
            lineTxt.text = "Loading";

            msgBox1 = transform.Find("msgbox").gameObject;
            msgTxt = transform.Find("msgbox/Text").GetComponent<Text>();
            msgBox1.SetActive(false);

            comfirmBox = transform.Find("confirm").gameObject;
            comfirmBox.SetActive(false);
            transform.FindChild("confirm/bt").GetComponent<Button>().onClick.AddListener(oncomfirmClick);

            txt = transform.FindChild("confirm/Text").GetComponent<Text>();
        }

        void onBeginHelp()
        {
            showMsg(OutGameContMgr.getOutGameCont("helper"));
        }

        void onEndHelp()
        {
            hideMsg();
        }

        void onCheckingVer()
        {
            showMsg(OutGameContMgr.getOutGameCont("checkver"));
        }
        void onCheckUpdate()
        {
            showMsg(OutGameContMgr.getOutGameCont("checkupdate"));
        }
        void onEndCheck()
        {
            hideMsg();
        }


        void onComFirmWifihandle(Action onWifiClick)
        {
            showComfirm(OutGameContMgr.getOutGameCont("wifi"), onWifiClick);
        }

        void oncomfirmUpdate(long updatesize, int num,Action onUpdateClick)
        {
            if (updatesize < 1024)
            {

                showComfirm(OutGameContMgr.getOutGameCont("update", num.ToString(),"1K"), onUpdateClick);
            }
            else if (updatesize < 1048576)
            {
                showComfirm(OutGameContMgr.getOutGameCont("update", num.ToString(), (updatesize / 1024) + "K"), onUpdateClick);
            }
            else
            {
                showComfirm(OutGameContMgr.getOutGameCont("update", num.ToString(), (updatesize / 1000000) + "M"), onUpdateClick);
            }

        }


        void onInitOver()
        {
            hideMsg();
        }

        void onNeedUpdateVer()
        {
            //showMsg(OutGameContMgr.getOutGameCont("needupdateclientver"));
            showComfirm(OutGameContMgr.getOutGameCont("needupdateclientver"), on_update_Click);
        }
        void on_update_Click()
        {
            debug.Log("跳转到地址" + Globle.CLIENT_URL + "进行下载");
            Application.OpenURL(Globle.CLIENT_URL);
        }

        void onInit()
        {
            showMsg(OutGameContMgr.getOutGameCont("initing"));
        }




        void onClick()
        {
            comfirmBox.SetActive(false);
            clickHandle();
        }


        public void showMsg(string s)
        {
            msgTxt.text = s;
            msgBox1.SetActive(true);
        }

        public void hideMsg()
        {
            msgBox1.SetActive(false);
        }


        public void showComfirm(string desc, Action comfirhandle = null)
        {
            debug.Log("显示更新框！！！");
            hideMsg();
            txt.text = desc;
            if (comfirhandle == null)
            {
                transform.FindChild("confirm/bt").gameObject.SetActive(false);
            }
            else
            {
                transform.FindChild("confirm/bt").gameObject.SetActive(true);
                clickcomfirmHandle = comfirhandle;
                comfirmBox.SetActive(true);

            }
           
        }


        void oncomfirmClick()
        {
            if (clickcomfirmHandle != null)
                clickcomfirmHandle();
             comfirmBox.SetActive(false);
        }



        void Update()
        {
            if (gameMgr.curupdate_downProcess < 0)
            {
                line.transform.localScale = Vector3.zero;
                return;
            }

            line.transform.localScale = Vector3.one;
            if (m_fOver)
            {
                if(m_bStartEnterLogin == false)
                {
                    m_bStartEnterLogin = true;
                    ResourceManager._inst.LoadGameFightNeedRes();
                }

                if (m_scroll_bar.fillAmount >= 1f)
                {
                    m_funcCallBack();
                    dispose();
                }

                //m_scroll_bar.fillAmount = m_scroll_bar.fillAmount + Time.deltaTime * 2;
                m_scroll_bar.fillAmount = m_scroll_bar.fillAmount + 0.25f;
                if (m_scroll_bar.fillAmount >= 1f)
                    m_scroll_bar.fillAmount = 1f;
            }
            else if (gameMgr.realT)
            {
                //Debug.Log("gameMgr.realT");

                float p = (float)gameMgr.curupdate_downProcess / (float)(1.1f * (gameMgr.maxProcess == 0 ? 1 : gameMgr.maxProcess));  //*1.2的目的是为了留一点时间用于解压assetbundle
                if (!float.IsNaN(m_scroll_bar.fillAmount))
                    m_scroll_bar.fillAmount = p;
            }
            else
            {
                //Debug.Log("gameMgr.down_new_file");

                float p = (float)gameMgr.curupdate_downProcess / (float)(1.1f * (gameMgr.maxProcess == 0 ? 1 : gameMgr.maxProcess));
                if (m_scroll_bar.fillAmount < p)
                {
                    m_scroll_bar.fillAmount += Time.deltaTime * 0.02f;
                }

                if (m_scroll_bar.fillAmount > p)
                {
                    if (!float.IsNaN(m_scroll_bar.fillAmount))
                        m_scroll_bar.fillAmount = p;
                }

                if (m_scroll_bar.fillAmount >= 0.9f)
                    m_scroll_bar.fillAmount = 0.9f;
            }
           
        
            tick++;
            if (tick > 3)
            {
                tick = 0;
                if (gameMgr.stateProcess == 0)
                {
                    //  lineTxt.text = "";
                }
                else if (gameMgr.stateProcess == 1)
                {

                    if (tempTick > 3)
                    {
                        tempTick = 1;
                    }
                    lineTxt.text = OutGameContMgr.getOutGameCont("init" + tempTick, (int)(m_scroll_bar.fillAmount * 100f) + "",gameMgr.curUpdateFile);
                    tempTick++;
                }
                else if (gameMgr.stateProcess == 2)
                {

                    if (tempTick > 3)
                    {
                        tempTick = 1;
                    }
                    lineTxt.text = OutGameContMgr.getOutGameCont("loading" + tempTick, (int)(m_scroll_bar.fillAmount * 100f) + "", gameMgr.curUpdateFile);

                   
                    tempTick++;
                }
            }

        }

        public int tick = 0;
        private int tempTick = 1;

        public void loadCompleted(Action cb)
        {
            
            m_fOver = true;
            m_funcCallBack = cb;
        }

        public void dispose()
        {
            _inst = null;

            gameMgr.onBeginHelperHandle = null;
            gameMgr.onEndHelperHandle = null;
            gameMgr.onCheckingVerHandle = null;
            gameMgr.onCheckingUpdateHandle = null;
            gameMgr.onEndCheckingHandle = null;
            gameMgr.onResourceLoadedOverhandle = null;
            gameMgr.onNeedupdateclientver = null;
            gameMgr.onInitingHandle = null;
            gameMgr.onInitOverHandle = null;
            gameMgr.onComfirmUpdate = null;
            gameMgr.onComFirmWifihandle = null;
            gameMgr.onUpdateFailhanlde = null;
            gameMgr.onAllDone = null;


            Destroy(gameObject);

            Main.instance.m_bUpdateGameOver = true;
        }

        public void onfail(string error)
        {
            m_scroll_bar.fillAmount = 0f;

            showComfirm(error, onClick);


            //msgBox.SetActive(true);
            ////  msgBox.transform.Find("Text").GetComponent<Text>().text  = ;
            //msgBox.transform.localScale = Vector3.zero;
            //msgBox.transform.DOScale(1f, 0.3f);

        }
    }
}