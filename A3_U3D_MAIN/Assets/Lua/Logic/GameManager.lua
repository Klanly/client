require "Common/define"
require "Common/functions"
--Event = require 'events'


--require "3rd/pblua/login_pb"
--require "3rd/pbc/protobuf"

--local lpeg = require "lpeg"

--local json = require "cjson"
--local util = require "3rd/cjson.util"

--local sproto = require "3rd/sproto/sproto"
--local core = require "sproto.core"
--local print_r = require "3rd/sproto/print_r"

--require "Logic/LuaClass"
--require "Logic/CtrlManager"
--require "Common/functions"

require "ui/InterfaceMgr"

--管理器--
GameManager = {};
local this = GameManager;

local game; 
local transform;
local gameObject;
--local WWW = UnityEngine.WWW;

function GameManager.LuaScriptPanel()
	return  'Message';
end

function GameManager.Awake()
    --warn('Awake--->>>');
end

--启动事件--
function GameManager.Start()
	--warn('Start--->>>');
end

--初始化完成，发送链接服务器信息--
function GameManager.OnInitOK()
--    AppConst.SocketPort = 2012;
--    AppConst.SocketAddress = "127.0.0.1";
--    NetManager:SendConnect();

--    this.test_class_func();
--    this.test_pblua_func();
--    this.test_cjson_func();
--    this.test_pbc_func();
--    this.test_lpeg_func();
--    this.test_sproto_func();
--    coroutine.start(this.test_coroutine);

--    CtrlManager.Init();
--    local ctrl = CtrlManager.GetCtrl(CtrlName.Prompt);
--    if ctrl ~= nil and AppConst.ExampleMode then
--        ctrl:Awake();
--    end

    --logWarn('SimpleFramework InitOK--->>>');
--    a = NetManager:newVariant();
--   v=  NetManager:newVariant();
-- v:pushValue("aaa");
--  v:pushValue("bbb");
-- a:setValue("aaaaaa",v);
-- a:setValue("bbbb",1111);
--  log("::::::aaaaaaa:::::::::"..a:dump());
end


----测试lpeg--
--function GameManager.test_lpeg_func()
--	--logWarn("test_lpeg_func-------->>");
--	-- matches a word followed by end-of-string
--	local p = lpeg.R"az"^1 * -1

--	print(p:match("hello"))        --> 6
--	print(lpeg.match(p, "hello"))  --> 6
--	print(p:match("1 hello"))      --> nil
--end


--销毁--
function GameManager.OnDestroy()
	--logWarn('OnDestroy--->>>');
end
