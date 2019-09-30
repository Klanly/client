require "model/BaseModel"

AutoPlayModel = BaseModel:new();
local this = AutoPlayModel;

function this:init()
    
end;

this.onHpLowerChange = function(args)
    this.nHpLower = args[0];
    if(a1_high_fightgame.herohead_inst  ~= nil) then
        a1_high_fightgame.herohead_inst.onHpLowerChange();
    end
end

this.onMpLowerChange = function(args)
    this.nMpLower = args[0];
    if(a1_high_fightgame.herohead_inst  ~= nil) then
        a1_high_fightgame.herohead_inst.onMpLowerChange();
    end
end




