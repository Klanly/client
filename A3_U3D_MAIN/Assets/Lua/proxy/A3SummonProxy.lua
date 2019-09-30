require "proxy/BaseProxy"
A3SummonProxy = BaseProxy:new ();
local this = A3SummonProxy;

function this:init ()
    this:addProxyListener (PKG_NAME.A3SUMMON, this.OP);


end;



this.OP = function (v)
    
    

end;