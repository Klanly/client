using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using SimpleFramework.Manager;
using MuGame;
using GameFramework;

namespace SimpleFramework {
    public class LuaBehaviour : View {
        private string data = null;
        protected static bool initialize = false;
        private Dictionary<GameObject, LuaFunction> buttons = new Dictionary<GameObject, LuaFunction>();

        void Awake() {
            CallMethod("Awake", gameObject);
        }

        protected void Start() {
            if (LuaManager != null && initialize) {
                LuaState l = LuaManager.lua;
                l[name + ".transform"] = transform;
                l[name + ".gameObject"] = gameObject;
            }
            CallMethod("Start");
        }

        protected void OnClick() {
            CallMethod("OnClick");
        }

        protected void OnClickEvent(GameObject go) {
            CallMethod("OnClick", go);
        }

        /// <summary>
        /// 添加单击事件
        /// </summary>
        public void AddClick(GameObject go, LuaFunction luafunc) {
            if (go == null || luafunc == null) return;

            Button bt = go.GetComponent<Button>();
            if (bt == null)
                bt = go.AddComponent<Button>();

            buttons.Add(go, luafunc);
            bt.onClick.AddListener(
                delegate() {
                    luafunc.Call(go);
                }
            );
        }

        /// <summary>
        /// 删除单击事件
        /// </summary>
        /// <param name="go"></param>
        public void RemoveClick(GameObject go) {
            if (go == null) return;
            LuaFunction luafunc = null;
            if (buttons.TryGetValue(go, out luafunc)) {
                luafunc.Dispose();
                luafunc = null;
                buttons.Remove(go);
            }
        }

        /// <summary>
        /// 清除单击事件
        /// </summary>
        public void ClearClick() {
            foreach (var de in buttons) {
                if (de.Value != null) {
                    de.Value.Dispose();
                }
            }
            buttons.Clear();
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        protected object[] CallMethod(string func, params object[] args) {
            if (!initialize) return null;
            return Util.CallMethod(name, func, args);
        }

        //-----------------------------------------------------------------
     void OnDestroy() {
            ClearClick();
            LuaManager = null;
#if ASYNC_MODE
            string abName = name.ToLower().Replace("panel", "");
            ResourceManager.UnloadAssetBundle(abName + AppConst.ExtName);
#endif
            Util.ClearMemory();
            debug.Log("~" + name + " was destroy!");
        }
    }
}