using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LuaInterface;
namespace SimpleFramework.Manager
{
    public class InterfaceLuaMgr : View
    {
        public static InterfaceLuaMgr instacne;
        void Awake()
        {
            instacne = this;

            MuGame.InterfaceMgr.handleOpenByLua = instacne.open;

            MuGame.InterfaceMgr.doCommandByLua = instacne.doCommand;
            MuGame.InterfaceMgr.doCommandByLua_discard = instacne.doCommand_discard;

            MuGame.InterfaceMgr.doGetAssert = instacne.getAssertGo;
            MuGame.InterfaceMgr.doGetAssert_sp = instacne.getAssertSp;
        }
        public object[] doCommand(string id, string path, params object[] args)
        {
            return Util.CallMethod("InterfaceMgr", "doLua", id, args, path);
        }
        public object[] doCommand_discard(string id, params object[] args)
        {
            return Util.CallMethod("InterfaceMgr", "doCommand", id, args);
        }

        public void open(string name, object pram = null)
        {
            CallMethod("open", name, pram);
        }
        public void close(string name)
        {
            CallMethod("close", name);
        }
        object[] CallMethod(string func, params object[] args)
        {
            return Util.CallMethod("InterfaceMgr", func, args);
        }

        public GameObject getAssertGo(string abname, string assetname)
        {
            return ResManager.LoadAsset(abname, assetname);
        }

        public Sprite getAssertSp(string abname, string assetname)
        {
            return ResManager.LoadAsset_Sprite(abname, assetname);
        }
    }
}