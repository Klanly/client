require "proxy/BaseProxy"

BagProxy = BaseProxy:new();
local this = BagProxy;


function this:init()
    this:addProxyListener(PKG_NAME.S2C_GET_ITEMS_RES, this.onLoadItems);
	this:addProxyListener(PKG_NAME.S2C_SELL_ITEM_RES, this.onSellItems);
	this:addProxyListener(PKG_NAME.S2C_USE_UITEM_RES, this.onUseItems);
	this:addProxyListener(PKG_NAME.S2C_ITEM_CHANGE, this.onItemChange);
	this:addProxyListener(PKG_NAME.S2C_BAGITEM_CDTIME, this.onItemCd);
end;

function this:sendLoadItems(val)
	v = variant();
	v:setValue("option",val);
	this:sendRPC(PKG_NAME.S2C_GET_ITEMS_RES, v);
end;

function this:sendMark(id)
	v = variant();
	v:setValue("option",6);
	v:setValue("id",id);
	this:sendRPC(PKG_NAME.S2C_GET_ITEMS_RES, v);
end;

function this:sendOpenLock(type, num,use_yb)
	v = variant();
	v:setValue("option",type);
	v:setValue("unlock_num",num);
	v:setValue("use_yb",use_yb);
	this:sendRPC(PKG_NAME.S2C_GET_ITEMS_RES, v);
end;


function this:sendRoomItems(pack_to_cangku, id, num)
	v = variant();
	v:setValue("option",4);
	v:setValue("pack_to_cangku",pack_to_cangku);
	v:setValue("item_id",id);
	v:setValue("item_num",num);
	this:sendRPC(PKG_NAME.S2C_GET_ITEMS_RES, v);
end;

function this:sendSellItems(id, num)
	v = variant();
	v:setValue("id",id);
	v:setValue("num",num);
	this:sendRPC(PKG_NAME.S2C_SELL_ITEM_RES, v);
end;

function this:sendUseItems(id, num)
	v = variant();
	v:setValue("id",id);
	v:setValue("num",num);
	this:sendRPC(PKG_NAME.S2C_USE_UITEM_RES, v);
end;

function this:onSellItems(data)
end

function this:onUseItems(data)
end

function this:onItemChange(data)
end

function this:onItemCd(data)
end


function this.onLoadItems(data)
	log("LUA°ü¹ü"..data:dump());
	local res =data:getValue("res")._int;
	if(res < 0)then
		--ÇëÇó³ö´í
	end

	if(res == 0)then
		a3_BagModel:getInstance().Items = nil;
		a3_BagModel:getInstance().Items = {};
		a3_BagModel:getInstance().curi = data:getValue("curi")._int;
		local info = data:getValue("items")._arr;
		local len = info.Count;
		for i= 0, len-1 do
			local itemData = a3_BagItemData:new();
			itemData.id = info[i]:getValue("id")._int;
			itemData.tpid = info[i]:getValue("tpid")._int;
			itemData.num = info[i]:getValue("cnt")._int;
			itemData.bnd = info[i]:getValue("bnd")._int;
			itemData.ismark = info[i]:getValue("mark")._int;
			itemData.isEquip = false;
			itemData.isNew  = false;
			if(info[i]:ContainsKey("intensify_lv"))then
				--itemData = a3_EquipModel.getInstance().equipData_read(itemData, item);
			end
			a3_BagModel:getInstance().addItem(itemData);
		end
	--elseif(res == 1)then
		--
	--elseif(res == 2)then
	--elseif(res == 3)then
	--elseif(res == 4)then
	--elseif(res == 5)then
	--elseif(res == 6)then
	end
end