VipModel = BaseModel:new()
local this = VipModel

function this:init()
    this.level = 0
end

this.modLevel = function(args)
    this.level = args[0]
    if a1_low_fightgame.heroicon_head_low_inst ~= nil then
        a1_low_fightgame.heroicon_head_low_inst.onVipChange()
    end
end