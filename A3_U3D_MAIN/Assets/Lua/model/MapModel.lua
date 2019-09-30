
MapModel = BaseModel:new();
local this = MapModel;

function this:init()
end

this.getcurLevelId = function(v)
	if(v == nil)then
		return;
	end
	log("lualualialailaial"..v[0]);
	this.curLevelId = v[0];
end

this.getmapinfo = function(v)
	if(v == nil)then
		return;
	end
	this.curSvrConf = v[0];
	log("lua"..v[0]:dump());
end






