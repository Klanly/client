
Globle = { };
local this = Globle;

Globle.getAttrAddById = function(id, value, add)
	local add = true;
	local str =this.getAttrNameById (id);
	-- this.getAttrNameById (id);
	if id == 19 or id == 20 or id == 17 then
		add = false;
	end
	if id == 16 then
		str = str .. ":" .. value;
	elseif (id == 17 or id == 19 or id == 20 or id == 24 or id == 25 or id == 29 or id == 30 or id == 31 or id == 32
		or id == 33 or id == 35 or id == 36 or id == 37 or id == 39 or id == 40 or id == 17 or id == 41) then
		if add then
			str = str .. "+" ..(value / 10) .. "%";
		else
			str = str .. "-" ..(value / 10) .. "%";
		end
	else
		if add then
			str = str .. "+" .. value;
		else
			str = str .. "-" .. value;
		end

	end
	return str;
end
Globle.getAttrNameById = function(id)
	local str;
	if id == 1 then
		str = "力量";
	elseif id == 2 then
		str = "敏捷";
	elseif id == 3 then
		str = "体力";
	elseif id == 4 then
		str = "魔力";
	elseif id == 5 then
		str = "攻击上限";
	elseif id == 6 then
		str = "物理防御";
	elseif id == 7 then
		str = "魔法防御";
	elseif id == 8 then
		str = "火攻";
	elseif id == 9 then
		str = "冰攻";
	elseif id == 10 then
		str = "光攻";
	elseif id == 11 then
		str = "火防";
	elseif id == 12 then
		str = "冰防";
	elseif id == 13 then
		str = "光防";
	elseif id == 14 then
		str = "生命";
	elseif id == 15 then
		str = "法力";
	elseif id == 16 then
		str = "罪恶值";
	elseif id == 17 then
		str = "法力消耗";
	elseif id == 18 then
		str = "生命击回";
	elseif id == 19 then
		str = "物理伤害减免";
	elseif id == 20 then
		str = "魔法伤害减免";
	elseif id == 21 then
		str = "技能伤害";
	elseif id == 22 then
		str = "致命攻击";
	elseif id == 23 then
		str = "致命闪避";
	elseif id == 24 then
		str = "生命上限";
	elseif id == 25 then
		str = "法力上限";
	elseif id == 26 then
		str = "生命秒回";
	elseif id == 27 then
		str = "法力秒回";
	elseif id == 28 then
		str = "法力击回";
	elseif id == 29 then
		str = "魔法护盾";
	elseif id == 30 then
		str = "经验值";
	elseif id == 31 then
		str = "祝福";
	elseif id == 32 then
		str = "知识值";
	elseif id == 33 then
		str = "致命伤害";
	elseif id == 34 then
		str = "智慧";
	elseif id == 35 then
		str = "火防增加";
	elseif id == 36 then
		str = "冰防增加";
	elseif id == 37 then
		str = "光防增加";
	elseif id == 38 then
		str = "攻击下限";
	elseif id == 39 then
		str = "双倍伤害概率";
	elseif id == 40 then
		str = "反射受到的致命伤害概率";
	elseif id == 41 then
		str = "忽略一次受到的致命伤害概率";
	elseif id == 42 then
		str = "每次暴击恢复多少点生命";
	elseif id == 43 then
		str = "命中";
	elseif id == 44 then
		str = "闪避";
	elseif id == 45 then
		str = "无视防御伤害";
	elseif id == 46 then
		str = "硬直";
	else
		str = "";
	end
	return str;
end