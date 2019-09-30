require "model/BaseModel"
require "proxy/BagProxy"
PlayerModel = BaseModel:new();
local this = PlayerModel;

function this:init()
    --this.max_hp = 0;   --最大血量
    --this.hp = 0;       --当前血量
    --this.zhuan = 0;    --转生等级
    --PlayerModel.lvl = 0;       --等级
    this.attr_list = {};
	--BagProxy:getInstance();
	--BagProxy:getInstance():sendLoadItems(0);
end;

this.initInfo = function(args)
    local data = args[0];
    log("LUA:::::" .. data:dump());
    this.profession = data:getValue("carr")._uint;
    this.cid = data:getValue("cid")._uint;
    this.uid = data:getValue("uid")._uint;
    this.iid = data:getValue("iid")._uint;
    this.name = data:getValue("name")._str;

    this.zhuan = data:getValue("zhuan")._uint;
    this.mapid = data:getValue("mpid")._uint;
    this.lvl = data:getValue("lvl")._uint;
    this.exp = data:getValue("exp")._uint;
    this.hp = data:getValue("hp")._uint;
    this.mp = data:getValue("mp")._uint;
    this.combpt = data:getValue("combpt")._uint;

    this.pt_att = data:getValue("att_pt")._int;
    this.pt_strpt = data:getValue("strpt")._int;
    this.pt_conpt = data:getValue("conpt")._int;
    this.pt_intept = data:getValue("intept")._int;
    this.pt_wispt = data:getValue("wispt")._int;
    this.pt_agipt = data:getValue("agipt")._int;
    this.line = data:getValue("line")._int;
    
    if(data:ContainsKey("yb")) then
        this.gold = data:getValue("yb")._uint;
    end
    if(data:ContainsKey("money")) then
        this.money = data:getValue("money")._uint;
    end
	if(data:ContainsKey("bndyb"))then
		this.gift = data:getValue("bndyb")._uint;
	end
    if(data:ContainsKey("zhuan")) then
        this.up_lvl=data:getValue("zhuan")._uint;
    end
    if(data:ContainsKey("battleAttrs")) then
        local v = data:getValue("battleAttrs");
        this.attrChange(v);
    end
	if(data:ContainsKey("items"))then
		
		a3_BagModel:getInstance().initItemList(data:getValue("items")._arr);
		--local a = data:getValue("items")._arr;
	end
    if(data:ContainsKey("pk_state")) then
        this.now_pkState = data:getValue("pk_state")._uint
    end
end;

this.modPkState = function(args)
    this.now_pkState = args[0];
    local isShowAni = args[1];
    if(a1_low_fightgame.heroicon_head_low_inst ~= nil) then
        a1_low_fightgame.heroicon_head_low_inst.refreshPkImages(isShowAni);
    end
end;

this.modHp = function(args)
    this.hp = args[0];
	this.max_hp = args[1];
    if(a1_high_fightgame.herohead_inst  ~= nil) then
        a1_high_fightgame.herohead_inst.refreshHp();
    end
end;

this.modMp = function(args)
    this.mp = args[0];
	this.max_mp = args[1];
    if(a1_high_fightgame.herohead_inst  ~= nil) then
        a1_high_fightgame.herohead_inst.refreshMp();
    end
end;
this.modExp = function(args)
    this.exp = args[0];
    if(a1_high_fightgame.instance  ~= nil) then
        a1_high_fightgame.instance.refreshExp();
    end
end;

this.modInfo = function(args)
    local v = args[0];

    if(v:ContainsKey("zhuan")) then
        this.zhuan = v:getValue("zhuan")._int;
        if(a1_high_fightgame.herohead_inst  ~= nil) then
            a1_high_fightgame.herohead_inst.refreshZhuan();
        end
    end

    if(v:ContainsKey("lvl")) then
        this.lvl = v:getValue("lvl")._int;
        if(a1_high_fightgame.herohead_inst  ~= nil) then
            a1_high_fightgame.herohead_inst.refreshLv();
        end
    end
	
    if(v:ContainsKey("combpt")) then
        this.combpt = v:getValue("combpt")._int;
        if(a1_high_fightgame.herohead_inst  ~= nil) then
            a1_high_fightgame.herohead_inst.refreshCombpt();
        end
    end
	
    if(v:ContainsKey("pinfo")) then
        local v_pinfo = v:getValue("pinfo");
        this.exp = v_pinfo:getValue("exp")._int;
        this.hp = v_pinfo:getValue("hp")._int;
        this.combpt = v_pinfo:getValue("combpt")._int;
        if(a1_high_fightgame.herohead_inst  ~= nil) then
            a1_high_fightgame.herohead_inst.refreshCombpt();
            a1_high_fightgame.herohead_inst.refreshHp();
        end

        if(v_pinfo:ContainsKey("battleAttrs")) then
            this.attrChange(v_pinfo:getValue("battleAttrs"));
        end
    end
end;

this.mapinfo = function(v)
	if(v == nil)then
		return;
	end
	this.curSvrConf = v[0];
	log("lua"..v[0]:dump());
end

this.attrChange = function(v)
log ("LUA属性变化");
    this.strength = v:getValue("strength")._int
    this.agility = v:getValue("agility")._int;
    this.constitution = v:getValue("constitution")._int;
    this.intelligence = v:getValue("intelligence")._int;
    this.max_attack = v:getValue("max_attack")._int;
    this.physics_def = v:getValue("physics_def")._int;
    this.magic_def = v:getValue("magic_def")._int;
    this.fire_att = v:getValue("fire_att")._int;
    this.ice_att = v:getValue("ice_att")._int;
    this.light_att = v:getValue("light_att")._int;
    this.fire_def = v:getValue("fire_def")._int;
    this.ice_def = v:getValue("ice_def")._int;
    this.light_def = v:getValue("light_def")._int;
    this.max_hp = v:getValue("max_hp")._int;
    this.max_mp = v:getValue("max_mp")._int;


	log("LUA   max"..this.max_mp);
    this.crime = v:getValue("crime")._int;
    this.mp_abate = v:getValue("mp_abate")._int;
    this.hp_suck = v:getValue("hp_suck")._int;
    this.physics_dmg_red = v:getValue("physics_dmg_red")._int;
    this.magic_dmg_red = v:getValue("magic_dmg_red")._int;
    this.skill_damage = v:getValue("skill_damage")._int;
    this.fatal_att = v:getValue("fatal_att")._int;
    this.fatal_dodge = v:getValue("fatal_dodge")._int;
    this.max_hp_add = v:getValue("max_hp_add")._int;
    this.max_mp_add = v:getValue("max_mp_add")._int;
    this.hp_recovery = v:getValue("hp_recovery")._int;
    this.mp_recovery = v:getValue("mp_recovery")._int;
    this.mp_suck = v:getValue("mp_suck")._int;
    this.magic_shield = v:getValue("magic_shield")._int;
    this.exp_add = v:getValue("exp_add")._int;
    this.blessing = v:getValue("blessing")._int;
    this.knowledge_add = v:getValue("knowledge_add")._int;
    this.fatal_damage = v:getValue("fatal_damage")._int;
    this.fire_def_add = v:getValue("fire_def_add")._int;
    this.ice_def_add = v:getValue("ice_def_add")._int;
    this.light_def_add = v:getValue("light_def_add")._int;
    this.wisdom = v:getValue("wisdom")._int;
    this.min_attack = v:getValue("min_attack")._int;
    this.double_damage_rate = v:getValue("double_damage_rate")._int;
    this.reflect_crit_rate = v:getValue("reflect_crit_rate")._int;
    this.ignore_crit_rate = v:getValue("ignore_crit_rate")._int;
    this.crit_add_hp = v:getValue("crit_add_hp")._int;
    this.hit = v:getValue("hit")._int;
    this.dodge = v:getValue("dodge")._int;
    this.ignore_defense_damage = v:getValue("ignore_defense_damage")._int;
    this.stagger = v:getValue("stagger")._int;

    this.attr_list[1] = this.strength; 
	this.attr_list[2] = this.agility; 
	this.attr_list[3] = this.constitution; 
	this.attr_list[4] = this.intelligence; 
	this.attr_list[5] = this.max_attack; 
	this.attr_list[6] = this.physics_def; 
	this.attr_list[7] = this.magic_def; 
	this.attr_list[8] = this.fire_att; 
	this.attr_list[9] = this.ice_att; 
	this.attr_list[10] = this.light_att; 
	this.attr_list[11] = this.fire_def; 
	this.attr_list[12] = this.ice_def; 
	this.attr_list[13] = this.light_def;
	this.attr_list[14] = this.max_hp; 
	this.attr_list[15] = this.max_mp; 
	this.attr_list[16] = this.crime; 
	this.attr_list[17] = this.mp_abate; 
	this.attr_list[18] = this.hp_suck;
	this.attr_list[19] = this.physics_dmg_red; 
	this.attr_list[20] = this.magic_dmg_red; 
	this.attr_list[21] = this.skill_damage; 
	this.attr_list[22] = this.fatal_att; 
	this.attr_list[23] = this.fatal_dodge; 
	this.attr_list[24] = this.max_hp_add; 
	this.attr_list[25] = this.max_mp_add;
	this.attr_list[26] = this.hp_recovery; 
	this.attr_list[27] = this.mp_recovery; 
	this.attr_list[28] = this.mp_suck; 
	this.attr_list[29] = this.magic_shield;
	this.attr_list[30] = this.exp_add;
	this.attr_list[31] = this.blessing;
	this.attr_list[32] = this.knowledge_add;
	this.attr_list[33] = this.fatal_damage; 
	this.attr_list[34] = this.fire_def_add;
	this.attr_list[35] = this.ice_def_add; 
	this.attr_list[36] = this.light_def_add;
	this.attr_list[37] = this.wisdom;
    this.attr_list[38] = this.min_attack; 
    this.attr_list[39] = this.double_damage_rate;
    this.attr_list[40] = this.reflect_crit_rate;
    this.attr_list[41] = this.ignore_crit_rate;
    this.attr_list[42] = this.crit_add_hp;
    this.attr_list[43] = this.hit;
    this.attr_list[44] = this.dodge;
    this.attr_list[45] = this.ignore_defense_damage;
    this.attr_list[46] = this.stagger;
end;





