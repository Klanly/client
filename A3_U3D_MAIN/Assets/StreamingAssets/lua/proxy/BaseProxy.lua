BaseProxy = Base:new( { })
local this = BaseProxy;
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

function this:addProxyListener(id,handle)
    NetManager:addProxyListener(id, handle);
end;


function this:sendRPC(id,v)
   NetManager:sendRPC(id, v);
end;

function this:getInstance()
    if nil == self.m_Instance then
        self.m_Instance = self:new()
    end
    return self.m_Instance
end;



