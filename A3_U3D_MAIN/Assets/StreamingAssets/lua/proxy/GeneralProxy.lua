require "proxy/BaseProxy"

GeneralProxy = BaseProxy:new();
local this = GeneralProxy;


function this:init()
    this:addProxyListener(PKG_NAME.S2C_GOD_LIGHT, this.onGodLightActive);
end;

active_open = false;
active_left_tm = 0;

function this.onGodLightActive(v)
	log("lualualua"..v:dump());
	active_open = v:getValue("open")._bool;
	active_left_tm = v:getValue("left_tm")._int;
	if (a1_low_fightgame.instance == nil) then
		return;
    end;
	a1_low_fightgame.instance.showActiveIcon(active_open ,active_left_tm);
end;


