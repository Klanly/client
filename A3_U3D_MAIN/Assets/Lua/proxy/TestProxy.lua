require "proxy/BaseProxy"

TestProxy = BaseProxy:new();
local this = TestProxy;


function this:init()
    this:addProxyListener(PKG_NAME.S2C_TEST, this.proxyHandle);
end;

function this.proxyHandle(v)

    if (wintest.instance == nil) then
        return
    end;

    wintest.instance.proxyHandle(v);

end;

function this:sendTest()
    v = variant();
    v:setValue("op", 1);
    this:sendRPC(PKG_NAME.C2S_TEST, v);
end;
