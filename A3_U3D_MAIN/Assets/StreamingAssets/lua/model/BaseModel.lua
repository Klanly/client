BaseModel = Base:new( { })
local this = BaseModel;
function this:new(o)
    o = o or { }
    setmetatable(o, self)
    self.__index = self
    super = self;
    o:init();
    return o;
end;

function this:init()
   
end;

function this:getInstance()
    if nil == self.m_Instance then
        self.m_Instance = self:new()
    end
    return self.m_Instance
end;



