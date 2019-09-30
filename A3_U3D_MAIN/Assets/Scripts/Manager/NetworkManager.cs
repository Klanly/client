using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace SimpleFramework.Manager
{
    public class NetworkManager : View
    {
        protected SessionFuncMgr sessionFuncMgr;
        protected NetClient netClient;
        public Variant a;
        protected Dictionary<uint, LuaFunction> poolRpc = new Dictionary<uint, LuaFunction>();

        void init()
        {
            sessionFuncMgr = SessionFuncMgr.instance;
            netClient = NetClient.instance;
            sessionFuncMgr._luaFunc = proxyHandle;
        }

        public void addProxyListener(uint id, LuaFunction handle)
        {
            if (netClient == null)
                init();

            if (sessionFuncMgr == null)
            {
                return;
            }

            poolRpc[id] = handle;
        }

        bool proxyHandle(uint id, Variant v)
        {

            if (!poolRpc.ContainsKey(id))
                return false;

            poolRpc[id].CallParams(v);

            return true;
        }


        public void sendRPC(uint cmd, Variant v = null)
        {
            if (v == null)
                v = new Variant();
            netClient.sendRpc(cmd, v);
        }

        public Variant newVariant()
        {
            return new Variant();
        }

        public Variant writeInt(Variant v, string name, int num)
        {

            v[name] = num;
            return v;
        }

        public int getInt(Variant v, string name)
        {
            return v[name]._int;
        }

    }
}