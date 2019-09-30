IconImageMgr = BaseLayer:new();
local this = IconImageMgr;
IconImageMgr.createA3ItemIcon1 = function(data,istouch,num,scale,tip)
	istouch =istouch or false;
	num = num or -1;
	scale =scale or 1;
	tip = tip or false;
	local isUpEquip =false;
	if(data.isEquip)then
	end
	return IconImageMgr.createA3ItemIcon2(data.confdata, istouch, num, scale, tip, data.equipdata.stage, data.equipdata.blessing_lv, data.isNew, isUpEquip, data.ismark );
end

--IconImageMgr.createA3ItemIcon3 = function(itemid,istouch,num,scale,tip,stage,blessing_lv,isNew,isUpEquip)
	--istouch = istouch or false;
	--num = num or -1;
	--scale = scale or 1;
	--tip =tip or false;
	--stage =stage or -1;
	--blessing_lv =blessing_lv or 0;
	--isNew = isNew or false;
	--isUpEquip =isUpEquip or false;
	--local item = a3_BagModel.getItemDataById(itemid);
	--return IconImageMgr.createA3ItemIcon2(item, istouch, num, scale, tip, stage, blessing_lv, isNew, isUpEquip);
--end
--
IconImageMgr.createA3ItemIcon2 = function(item,istouch,num,scale,tip,stage,blessing_lv,isNew,isUpEquip,isMark)
	istouch = istouch or false;
	num = num or -1;
	scale = scale or 1;
	tip =tip or false;
	stage =stage or -1;
	blessing_lv =blessing_lv or 0;
	isNew = isNew or false;
	isUpEquip =isUpEquip or false;
	isMark = isMark or false;

	local iconPrefab = this.loadGo("iconimage");
	local icon = iconPrefab.transform:FindChild("icon"):GetComponent("Image");
	icon.sprite = this.loadSprite(item.file);
	local iconborder = iconPrefab.transform:FindChild("iconborder"):GetComponent("Image");
	iconborder.sprite = this.loadSprite(item.borderfile);
	local numText = iconPrefab.transform:FindChild("num"):GetComponent("Text");
	if(istouch)then
	iconPrefab.transform:GetComponent("Button").enabled = true;
	else
	iconPrefab.transform:GetComponent("Button").enabled = false;
	end
	if(num ~=-1)then
	numText.text = num;
	numText.gameObject:SetActive(true);
	else
	numText.gameObject:SetActive(false);
	end
	if(item.item_type == 2)then
	--×°±¸
	end
	if(isNew)then
		iconPrefab.transform:FindChild("iconborder/is_new").gameObject:SetActive(true);
	end
	if(isMark)then
		iconPrefab.transform:FindChild("iconborder/ismark").gameObject:SetActive(true);
	else
		iconPrefab.transform:FindChild("iconborder/ismark").gameObject:SetActive(false);
	end
	iconPrefab.name = "icon";
	--±ÈÀý
	return iconPrefab;
end
--
--IconImageMgr.createA3EquipIcon1 = function(data,scale,istouch)
	--scale =scale or 1;
	--istouch = istouch or false;
	--return  createA3EquipIcon2(data.confdata, data.equipdata.stage, data.equipdata.blessing_lv, scale, istouch);
--end
--
--IconImageMgr.createA3EquipIcon2 = function(data,stage,blessing_lv,scale,istouch)
	--stage =stage or -1;
	--blessing_lv =blessing_lv or 0;
	--scale =scale or 1;
	--istouch =istouch or false;
	--local iconPrefab = this.loadGo("iconimage");
	--local icon = iconPrefab.transform:FindChild("icon"):GetComponent("Image");
	--icon.sprite = this.loadSprite(item.file);
	--local iconborder = iconPrefab.transform:FindChild("iconborder"):GetComponent("Image");
	--iconborder.sprite = this.loadSprite(item.borderfile);
	--local numText = iconPrefab.transform:FindChild("num"):GetComponent("Text");
	--numText.gameObject:SetActive(false);
	--if(istouch)then
	--iconPrefab.transform:GetComponent("Button").enabled = true;
	--else
	--iconPrefab.transform:GetComponent("Button").enabled = false;
	--end
--
--end