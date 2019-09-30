require "model/BaseModel"
a3_BagModel = BaseModel:new();
local this = a3_BagModel;

local EVENT_EQUIP_ADD = 0;
local itemsXMl = XmlMgr:GetSXML("item");
local house_curi = 0;
local m_all_curi = 150;
this.curi = 77;
this.Items = {};
this.UnEquips ={};
this.hh = {1,2,3};
this.HouseItems = {};
local process_cd;
this.item_cds = {};
this.item_remove_cds = {};
this.item_reduce_cds = {};
local isFirstMark =true;
a3_BagModel.instance = this;

--function iter(a,i)
	--i = i+1;
	--local v = a[i];
	--if(v)then
	--return i,v;
	--end
--end
--function ilist(a)
	--return iter , a ,0;
--end
--
function this:init()
end;
a3_BagModel.getItemXml = function(tpid)
	return itemsXMl.GetNode("item", "id==" + tpid);
end

function this:GetCuri()
	return this.curi;
end

a3_BagModel.initItemList = function(arr)
	if(Items == nil)then
	Items = {};
	end
	local len = arr.Count;
	local ii =0;
	for i=0,len-1 do
		a3_BagModel.initItemOne(arr[i]);
		ii=ii+1;
	end
		log("this:::::"..len.."that:::::"..ii)
end
this.aa=function()
	log("查到xml:ppppppppppppppppppp");
end

a3_BagModel.initItemOne = function(data)
	local itemData = a3_BagItemData:new();
	itemData.id = data["id"];
	itemData.tpid = data["tpid"];
	itemData.num = data["cnt"];
    itemData.bnd = data["bnd"];
	--log("这里"..itemData.id .. itemData.tpid..itemData.num..itemData.bnd);
	itemData.isEquip = false;
    itemData.isNew = false;
	--if(data:ContainsKey("intensify_lv"))

	
end--

a3_BagModel.addItemCd  = function(type , time)
	
end

a3_BagModel.getItemCds =function()
	return item_cds;
end

a3_BagModel.getItems = function()
	return this.Items;
end

a3_BagModel.getHouseItems = function()
	return HouseItems;
end

a3_BagModel.getUnEquips = function()
	return UnEquips;
end

a3_BagModel.addItem = function(data)
	data.confdata = a3_BagModel.getItemDataById(data.tpid);
	local has =false;
	for i,v in pairs(Items)  do
		if(i == data.id)then
			has = true;
		end
	end
	if(has)then
		if(data.num == 0)then
			this.Items.Remove(data.id);
		else
			this.Items[data.id] = data;
		end
	else
		this.Items[data.id] = data;
end
end--

a3_BagModel.addHouseItem= function(data)
	data.confdata = a3_BagModel.getItemDataById (data.tpid);
	HouseItems[data.tpid] = data;
end

a3_BagModel.getItemCds = function()
	return item_cds;
end--
----
a3_BagModel.getItemDataById = function(tpid)
	local item = a3_ItemData:new();
	item.tpid= tpid;
	local s_xml = XmlMgr:GetSXML("item.item", "id=="..tpid);
	if(s_xml ~= nil) then
		item.file = s_xml:getString("icon_file");
		item.borderfile = s_xml:getString("quality");
		item.item_name = s_xml:getString("item_name");
		item.quality = s_xml:getInt("quality");
		item.desc = s_xml:getString("desc");
		item.desc2 = s_xml:getString("desc2");
		item.value = s_xml:getInt("value");
		item.use_lv = s_xml:getInt("use_lv");
		item.use_limit = s_xml:getInt("use_limit");
		item.use_type = s_xml:getInt("use_type");
		local score = s_xml:getInt("intensify_score");
		item.intensify_score = score;
		item.item_type = s_xml:getInt("item_type");
		item.equip_type = s_xml:getInt("equip_type");
		item.equip_level = s_xml:getInt("equip_level");
		item.job_limit = s_xml:getInt("job_limit");
		item.modelId = s_xml:getInt("model_id");
		item.on_sale = s_xml:getInt("on_sale");
		item.cd_type = s_xml:getInt("cd_type");
		item.cd_time = s_xml:getFloat("cd");
		item.main_effect = s_xml:getInt("main_effect");
		item.add_basiclevel = s_xml:getInt("add_basiclevel");
	end
	return item;

end
----
a3_BagModel.getItemDataByName = function(name)
	local item = a3_ItemData:new();
	local s_xml = itemsxml:getnode("item", "item_name=="..name);
	if(s_xml ~= nil) then
		item.file = s_xml:getstring("icon_file");
		item.borderfile = s_xml:getstring("quality");
		item.item_name = s_xml:getstring("item_name");
		item.quality = s_xml:getint("quality");
		item.desc = s_xml:getstring("desc");
		flytxt.fly(item.desc);
		item.desc2 = s_xml:getstring("desc2");
		item.value = s_xml:getint("value");
		item.use_lv = s_xml:getint("use_lv");
		item.use_limit = s_xml:getint("use_limit");
		item.use_type = s_xml:getint("use_type");
		local score = s_xml:getint("intensify_score");
		item.intensify_score = score;
		item.item_type = s_xml:getint("item_type");
		item.equip_type = s_xml:getint("equip_type");
		item.equip_level = s_xml:getint("equip_level");
		item.job_limit = s_xml:getint("job_limit");
		item.modelid = s_xml:getint("model_id");
		item.on_sale = s_xml:getint("on_sale");
		item.cd_type = s_xml:getint("cd_type");
		item.cd_time = s_xml:getfloat("cd");
		item.main_effect = s_xml:getint("main_effect");
		item.add_basiclevel = s_xml:getint("add_basiclevel");
	end
	return item;
end

a3_BagModel.isWorked = function(data)
	local a =false;
	for i,v in pairs(data.equipdata.gem_att)do
		local b = v==0;
		if(b == true)then
			a = true;
		elseif(b==false)then
			a=false
			break;
		end
	end
	return data.equipdata.intensify_lv == 0 and data.equipdata.stage == 0 and data.equipdata.add_level == 0 and a;

end
--
a3_BagModel.getItemNumByTpid = function(tpid)
	local num = 0;
	for i,v in pairs(items)do
		if(v.tpid == tpid )then
			num = v.num + num;
		end
	end
end
--
a3_BagModel.useItemByTpid = function(tpid,num)
	local data = a3_BagModel.getItemDataById(tpid);
	if(data.use_type < 0) then
		flytxt.fly("该物品不能使用！");
		return
	end
	local have_num = a3_BagModel.getItemNumByTpid(tpid);
	if(have_num < num)then
		flytxt.fly("物品数量不足！！");
		return
	end
	ids = {};
	local low_num = 999999;
	for i,v in ipairs(Items)do
		if(v.num == tpid)then
			if(v.num < low_num)then
				low_num = v.num;
				--ids.Insert(0, one);
			else
				--ids.Add(one);
			end
		end
	end
	local curneed_num = num;
	for i,v in ipairs()do
		if(v.num > curneed_num)then 
			BagProxy:getInstacne():sendUseItems(v.id,curneed_num);
			break;
		else
		BagProxy:getInstacne():sendUseItems(v.id,v.num);
			curneed_num = num - v.num;
		end
	end
end--


a3_BagItemData = BaseLayer:new();
local id;
local tpid;
local num;
local bnd;
local isEquip;
local isNew;
local isSummon;
local isLabeled;
local ismark;
local equipdata;
local confdata;
local summondata;
local auctiondata;

a3_EquipData = BaseLayer:new();
local color;
local intensify_lv;
local add_level;
local add_exp;
local stage;
local blessing_lv;
local combpt;
local subjoin_att;
local gem_att;

a3_ItemData = BaseLayer:new();
local tpid;
local file;
local borderfile;
local item_name;
local desc;
local desc2;
local quality;
local value;
local use_type;
local use_lv;
local use_limit;
local intensify_score;
local item_type;
local equip_type;
local equip_level;
local job_limit;
local modelId;
local on_sale;
local cd_type;
local cd_time;
local main_effect;
local add_basiclevel;

a3_SummonData = BaseLayer:new();
local id;
local tpid;
local name;
local currentexp;
local currenthp;
local maxhp; 
local grade; 
local naturaltype;
local level;   
local lifespan;
local blood;
local luck; 
local power;
local talent_type;
local attNatural;
local defNatural;
local agiNatural;
local conNatural;
local star;
local max_attack;
local min_attack;
local physics_def;
local magic_def;
local physics_dmg_red;
local magic_dmg_red; 
local double_damage_rate; 
local reflect_crit_rate;
local skillNum;
local skills;
local equips;
local isSpecial;
local objid;
local status;
