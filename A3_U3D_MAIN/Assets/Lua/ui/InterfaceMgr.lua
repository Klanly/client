require "ui/core/Base"
require "ui/core/BaseLayer"
require "proxy/PKG_NAME"
-- require "ui/interfaces/loadingui//IconImageMgr"
require "ui/interfaces/high/a1_high_fightgame"
--require "ui/interfaces/mid/a1_mid_fightgame"
require "ui/interfaces/low/a1_low_fightgame"
require "ui/interfaces/loadingui/flytxt"
require "ui/interfaces/loadingui/fightingup"
require "ui/interfaces/window/wintest"
require "ui/interfaces/window/a3_task"
require "ui/interfaces/window/a3_auction"
require "ui/interfaces/window/a3_summon"
require "ui/interfaces/window/a3_star_pic"
-- require "ui/interfaces/window/a3_bag"
-- require "ui/interfaces/window/a3_role"


local UI_TYPE_WINDOW = 1;
local UI_TYPE_FLOATUI = 2;
local UI_TYPE_LOADING = 3;
local UI_TYPE_LOW = 11;
local UI_TYPE_MID = 12;
local UI_TYPE_HIGH = 13;

InterfaceMgr = { };

local this = InterfaceMgr;
InterfaceMgr.m_lua = { };
InterfaceMgr.m_pool = { };
InterfaceMgr.data = Base:new( { winName = "", winType = 1, winItem = nil, ctrl = nil, prama = nil });
InterfaceMgr.len = 0;
InterfaceMgr.createData = function(name, uitype)
    this.len = this.len + 1;
    local item = InterfaceMgr.data:new( { winName = name, winType = uitype });
    this.m_pool[item.winName] = item;
    local luapathname = "ui/interfaces/window/";

    if uitype == UI_TYPE_WINDOW then
        luapathname = "ui/interfaces/window/" .. name;
    elseif uitype == UI_TYPE_FLOATUI then
        luapathname = "ui/interfaces/floatui/" .. name;
    elseif uitype == UI_TYPE_LOADING then
        luapathname = "ui/interfaces/loadingui/" .. name
    elseif uitype == UI_TYPE_LOW then
        luapathname = "ui/interfaces/low/" .. name
    elseif uitype == UI_TYPE_MID then
        luapathname = "ui/interfaces/mid/" .. name
    elseif uitype == UI_TYPE_HIGH then
        luapathname = "ui/interfaces/high/" .. name
    end
    item.luapathname = luapathname

    -- logWarn(item.winName)
    return name
end

InterfaceMgr.createData("a1_low_fightgame", UI_TYPE_LOW);
--InterfaceMgr.createData("a1_mid_fightgame", UI_TYPE_MID);
InterfaceMgr.createData("a1_high_fightgame", UI_TYPE_HIGH);

InterfaceMgr.WIN_FLYTXT = InterfaceMgr.createData("flytxt", UI_TYPE_LOADING);
InterfaceMgr.WIN_FIGHTINGUP = InterfaceMgr.createData("fightingup", UI_TYPE_LOADING);
InterfaceMgr.WIN_TEST = InterfaceMgr.createData("wintest", UI_TYPE_WINDOW);
InterfaceMgr.A3TASK = InterfaceMgr.createData("a3_task", UI_TYPE_WINDOW);
-- InterfaceMgr.A3_BAG = InterfaceMgr.createData("a3_bag", UI_TYPE_WINDOW);
InterfaceMgr.A3AUCTION = InterfaceMgr.createData("a3_auction", UI_TYPE_WINDOW);
InterfaceMgr.A3SUMMON = InterfaceMgr.createData("a3_summon", UI_TYPE_WINDOW);
InterfaceMgr.A3_STAR_PIC = InterfaceMgr.createData("a3_star_pic", UI_TYPE_WINDOW);
-- InterfaceMgr.A3_ROLE = InterfaceMgr.createData("a3_role", UI_TYPE_WINDOW);


InterfaceMgr.open = function(name, pram)
    local pram = pram or { };
    local windata = this.m_pool[name];
    -- logWarn("打开界面:" .. name);
    if windata == nil then
        -- logWarn("无法初始化界面:" .. name);
        return;
    end;
    windata.prama = pram;
    if windata.winItem == nil then

        if windata.ctrl == nil then
            require(windata.luapathname)
            windata.ctrl = _G[name]
        end
        PanelManager:CreateUI_Layer(windata.winName, windata.winType, this.onUICreate);
    else
        windata.winItem:SetActive(true);
        windata.ctrl.onShowed(pram);

    end;
end;

InterfaceMgr.del = function(name)
    local windata = this.m_pool[name];

    if windata == nil then
        return;
    end;

    if windata.winItem == nil then
        return;
    end;
    windata.ctrl.onClosed();
    destroy(windata.winItem.gameObject);
end;

InterfaceMgr.close = function(name)
    local windata = this.m_pool[name];

    if windata == nil then
        return;
    end;

    if windata.winItem == nil then
        return;
    end;
    windata.ctrl.onClosed();
    windata.winItem:SetActive(false);
end;

InterfaceMgr.STATE_NORMAL = 0;
InterfaceMgr.STATE_STORY = 1;
InterfaceMgr.STATE_FUNCTIONBAR = 2;
InterfaceMgr.STATE_FB_WIN = 3;
InterfaceMgr.STATE_FB_BATTLE = 4;
InterfaceMgr.STATE_3DUI = 5;
InterfaceMgr.STATE_HIDE_ALL = 6;
InterfaceMgr.STATE_SHOW_ONLYWIN = 7;
InterfaceMgr.changeState = function(state)
    -- 逐步将c#层的界面模拟控制转移到lua
    PanelManager:changeStateByC(state);
end;


InterfaceMgr.onUICreate = function(obj)
    local gameObject = obj;
    local item = this.m_pool[gameObject.name];
    item.winItem = obj;
    item.ctrl.onShowed(item.prama);


    if (item.winType == UI_TYPE_WINDOW and item.ctrl.showBg ~= false) then
        item.ctrl.addBg();
    end;


end;



-- ===================================================================================================
InterfaceMgr.resignCommand = function(commandid, act)
    if (this.m_luaCommand[commandid] ~= nil) then
        return
    end;



    this.m_luaCommand[commandid] = { str = act }
end;
InterfaceMgr.m_luaCommand = { };

InterfaceMgr.doCommand = function(commandid, paramArr)

    if (this.m_luaCommand[commandid] == nil) then
        return;
    end;
    local comm = this.m_luaCommand[commandid];

    InterfaceMgr.doLua(comm["str"], paramArr)
end;


local luacomm = { }
InterfaceMgr.doLua = function(command, paramArr, filepath)
    -- logWarn(command)
    if luacomm[command] == nil then
        if filepath ~= nil then
            -- logWarn("filepath  "..tostring(paramArr))
            require(filepath)
        end
        luacomm[command] = assert(loadstring("return " .. command))()
    end

    if luacomm[command] == nil then
        logError("错误接口：" .. command)
        return;
    end
    -- logWarn(command.."  "..tostring(paramArr))
    if (paramArr == nil) then
        return luacomm[command]()
    else
        return luacomm[command](paramArr)
    end;
end


InterfaceMgr.openWintest = function()

    InterfaceMgr.open(InterfaceMgr.WIN_TEST);
end;


-------------------不再新加内容，改用新接口---------------------------------------------------------
InterfaceMgr.resignCommand("interfacemgr_openwintest", "InterfaceMgr.openWintest");


