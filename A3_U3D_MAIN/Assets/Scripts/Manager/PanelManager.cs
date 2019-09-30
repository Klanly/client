using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LuaInterface;
using GameFramework;
using MuGame;
using DG.Tweening;
using Cross;

namespace SimpleFramework.Manager
{
    public class PanelManager : View
    {
        private Transform m_winlayer_parent;

        Transform WinLayerParent
        {
            get
            {
                if (m_winlayer_parent == null)
                {
                    GameObject go = GameObject.Find("canvas_main/winLayer");
                    if (go != null) m_winlayer_parent = go.transform;
                }
                return m_winlayer_parent;
            }
        }

        private Transform m_floatui_parent;

        Transform FloatUIParent
        {
            get
            {
                if (m_floatui_parent == null)
                {
                    GameObject go = GameObject.Find("canvas_main/floatUI");
                    if (go != null) m_floatui_parent = go.transform;
                }
                return m_floatui_parent;
            }
        }

        private Transform m_loadingui_parent;
        Transform LoadingUIParent
        {
            get
            {
                if (m_loadingui_parent == null)
                {
                    GameObject go = GameObject.Find("Canvas_overlay/loadingLayer");
                    if (go != null) m_loadingui_parent = go.transform;
                }
                return m_loadingui_parent;
            }
        }


        //优化的3层UI
        private Transform m_canvas_low_parent;
        Transform CanvasLow_Parent
        {
            get
            {
                if (m_canvas_low_parent == null)
                {
                    GameObject go = GameObject.Find("canvas_low");
                    if (go != null) m_canvas_low_parent = go.transform;
                }
                return m_canvas_low_parent;
            }
        }

        private Transform m_canvas_mid_parent;
        Transform CanvasMid_Parent
        {
            get
            {
                if (m_canvas_mid_parent == null)
                {
                    GameObject go = GameObject.Find("canvas_mid");
                    if (go != null) m_canvas_mid_parent = go.transform;
                }
                return m_canvas_mid_parent;
            }
        }


        private Transform m_canvas_high_parent;
        Transform CanvasHigh_Parent
        {
            get
            {
                if (m_canvas_high_parent == null)
                {
                    GameObject go = GameObject.Find("canvas_high");
                    if (go != null) m_canvas_high_parent = go.transform;
                }
                return m_canvas_high_parent;
            }
        }


        public void ui_unshow()
        {
            if (GameObject.Find("Canvas_overlay/loadingLayer/fightingup") && GameObject.Find("Canvas_overlay/loadingLayer/fightingup").gameObject.activeSelf)
                GameObject.Find("Canvas_overlay/loadingLayer/fightingup").gameObject.SetActive(false);
            if (GameObject.Find("Canvas_overlay/loadingLayer/a3_attChange(Clone)") && GameObject.Find("Canvas_overlay/loadingLayer/a3_attChange(Clone)").gameObject.activeSelf)
                GameObject.Find("Canvas_overlay/loadingLayer/a3_attChange(Clone)").gameObject.SetActive(false);
        }

        public void open(string name)
        {

        }

        /// <summary>
        /// 创建面板，请求资源管理器
        /// </summary>
        /// <param name="type"></param>
        //public void CreatePanel(string name, LuaFunction func = null) {
        public void CreateUI_Layer(string name, int type, LuaFunction func = null)
        {
            string assetName = name;
            //GameObject prefab = ResManager.LoadAsset("ab_layer.assetbundle", "uilayer_" + name);

            //Debug.Log("同步加载Lua的UI=" + name);
            GameObject prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_" + name);

            Transform tf_parent = WinLayerParent;
            if (type == 2)
            {
                tf_parent = FloatUIParent;
            }
            else if (type == 3)
            {
                tf_parent = LoadingUIParent;
            }
            else if (type == 11)
            {
                tf_parent = CanvasLow_Parent;
            }
            else if (type == 12)
            {
                tf_parent = CanvasMid_Parent;
            }
            else if (type == 13)
            {
                tf_parent = CanvasHigh_Parent;
            }

            if (tf_parent.FindChild(name) != null || prefab == null)
            {
                return;
            }

            GameObject go = Instantiate(prefab) as GameObject;
            go.name = assetName;
            go.layer = LayerMask.NameToLayer("Default");
            go.transform.SetParent(tf_parent, false);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.AddComponent<LuaUI>();

            if (func != null) func.Call(go);
            debug.Log("CreateUI_Layer::>> " + name + " " + prefab);
        }

        public GameObject newGameobject(string name)
        {
            GameObject go = new GameObject();
            go.AddComponent<RectTransform>();
            go.name = name;
            return go;
        }
        public void onSound(string path)
        {
            MuGame.MediaClient.instance.PlaySoundUrl("audio_common_" + path, false, null);
        }
        public Image newImage(GameObject go)
        {
            Image img = go.AddComponent<Image>();
            return img;
        }
        public void GAME_CAMERA(bool tf)
        {
            if (GRMap.GAME_CAMERA != null)
            {
                GRMap.GAME_CAMERA.SetActive(tf);
            }

        }

        public ScrollControler newScrollControler(Transform trans)
        {
            ScrollControler scrollControler = new ScrollControler();
            ScrollRect scroll = trans.GetComponent<ScrollRect>();
            scrollControler.create(scroll);
            return scrollControler;
        }
        public void newcellSize(Transform trans, float x, float y)
        {
            if (trans == null) return;
            //Vector2 cellSize=trans.GetComponent<GridLayoutGroup>().cellSize;
            //cellSize = new Vector2(0, 0);
            //return cellSize;
            trans.GetComponent<GridLayoutGroup>().cellSize = new Vector2(x, y);
        }
        public string[] new_Split(string str)
        {
            //  if (str == null) return;
            string[] code = str.Split(',');
            return code;
        }


        public TabControl newTabControler(Transform trans, Transform main, LuaFunction onswitch)
        {
            TabControl ctl = new TabControl();
            if (onswitch != null)
            {
                ctl.onClickHanle = (TabControl tc) =>
                {
                    for (int i = 0; i < main.childCount; i++)
                    {
                        main.GetChild(i).gameObject.SetActive(false);
                    }
                    var mn = main.FindChild(trans.GetChild(tc.getSeletedIndex()).name);
                    if (mn != null)
                        mn.gameObject.SetActive(true);
                    onswitch.Call(tc);
                };
            }
            ctl.create(trans.gameObject, main.gameObject);
            return ctl;
        }


        public GameObject resLoad(string goname)
        {
            GameObject prefab = GAMEAPI.LoadNow_GameObject_OneAsset(goname + ".assetbundle", goname);
            GameObject go = Instantiate(prefab) as GameObject;
            return go;
        }

        public Sprite resPicLoad(string goname)
        {
            return GAMEAPI.LoadNow_Sprite_OneAsset(goname + ".assetbundle", goname);
        }


        public XMLMgr xmlMgr()
        {
            return XMLMgr.instance;
        }

        public KeyWord getKeyWord()
        {
            return KeyWord.instance;
        }

        public Tween domoveX(Transform trans, float value, float duration, object handle = null)
        {

            if (handle != null && handle is LuaFunction)
            {
                return trans.DOLocalMoveX(value, duration).OnComplete(() =>
                {
                    (handle as LuaFunction).Call();
                });

            }

            return trans.DOLocalMoveX(value, duration);

        }

        public Tween domoveY(Transform trans, float value, float duration, object handle = null)
        {
            if (handle != null && handle is LuaFunction)
            {
                return trans.DOLocalMoveY(value, duration).OnComplete(() =>
                {
                    (handle as LuaFunction).Call();
                });

            }

            return trans.DOLocalMoveY(value, duration);

        }

        public Tween doScaleX(Transform trans, float value, float duration, object handle = null)
        {
            if (handle != null && handle is LuaFunction)
            {
                return trans.DOScaleX(value, duration).OnComplete(() =>
                {
                    (handle as LuaFunction).Call();
                });

            }


            return trans.DOScaleX(value, duration);

        }

        public Tween doScaleY(Transform trans, float value, float duration, object handle = null)
        {
            if (handle != null && handle is LuaFunction)
            {
                return trans.DOScaleY(value, duration).OnComplete(() =>
                {
                    (handle as LuaFunction).Call();
                });

            }

            return trans.DOScaleY(value, duration);

        }

        public Tween doScale(Transform trans, Vector3 vec, float duration, object handle = null)
        {
            if (handle != null && handle is LuaFunction)
            {

                return trans.DOScale(vec, duration).OnComplete(() =>
                {
                    (handle as LuaFunction).Call();
                });

            }

            return trans.DOScale(vec, duration);
        }

        public Tween doRotate(Transform trans, Vector3 vec, float duration, object handle = null)
        {
            if (handle != null && handle is LuaFunction)
            {
                return trans.DOLocalRotate(vec, duration).OnComplete(() =>
                {
                    (handle as LuaFunction).Call();
                });

            }
            return trans.DOLocalRotate(vec, duration);
        }

        public void killTween(Transform trans)
        {
            trans.DOKill();
        }


        //public object dotween()
        //{

        //    return (object)DOTween;
        //}

        public string getCont(string id, LuaTable prams = null)
        {
            if (ContMgr.dText == null)
                ContMgr.init();

            if (!ContMgr.dText.ContainsKey(id))
                return id;

            string str = ContMgr.dText[id];

            if (prams != null)
            {
                int idx = 0;
                foreach (string p in prams.Values)
                {
                    string[] stringSeparators = new string[] { "<" + idx + ">" };
                    string[] arr = str.Split(stringSeparators, StringSplitOptions.None);
                    str = string.Join(p, arr);
                    idx++;
                }
            }

            return str;
        }

        public string getError(string id)
        {
            return ContMgr.getError(id);
        }



        public void openByC(string name)
        {
            MuGame.InterfaceMgr.getInstance().ui_async_open(name);
        }

        public void closeByC(string name)
        {
            MuGame.InterfaceMgr.getInstance().close(name);
        }
        public void changeStateByC(int state)
        {
            MuGame.InterfaceMgr.getInstance().changeState(state);
        }


        public GameObject addBehaviour(GameObject go, string behaviour)
        {
            Type TCls = ConfigUtil.getType("CollectRole");
            Type type = ConfigUtil.getType(behaviour);

            go.AddComponent(type);

            return go;
        }

        public EventTriggerListenerLua getEventTrigger(GameObject go)
        {
            return EventTriggerListenerLua.Get(go);
        }
        //public void Grild_cellSize(Transform trans, float x, float y)
        //{
        //    if (trans == null) return;
        //    trans.GetComponent<GridLayoutGroup>().cellSize = new Vector2(x, y);
        //}
        public void sliderOnValueChanged(Slider go, LuaFunction func)
        {
            if (go == null || func == null)
                return;
            go.onValueChanged.AddListener((float value) =>
            {
                if (func != null) func.Call(value);
            });
        }
        public void doByC(string name, params object[] args)
        {
            MuGame.InterfaceMgr.getInstance().doAction(name, args);
        }

        public void tween(Transform trans, string fun, LuaTable table)
        {


        }

        public void setGameJoy(bool open) {
            InterfaceMgr.getInstance().setGameJoy(open);
        }

        public void setGameSkill(bool open)
        {
            InterfaceMgr.getInstance().setGameSkill(open);
        }


        public FunctionOpenMgr functionOpenMgr()
        {
            return FunctionOpenMgr.instance;
        }
        public flytxt flytxts()
        {
            return MuGame.flytxt.instance;
        }
        public bool canlines()
        {
            return MuGame.a3_mapChangeLine.canline();
        }
    }
}