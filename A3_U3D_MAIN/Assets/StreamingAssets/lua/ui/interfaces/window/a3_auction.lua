a3_auction = BaseLayer:new();
local this = a3_auction;

a3_auction.init = function ()
    this.addClick (this.transform:FindChild ("btn_close").gameObject, this.onCloseClick);
    this.tabControler = this.getTabControler ("tabs", "contents", this.onSwitch);
end;

a3_auction.onCloseClick = function (go)
    InterfaceMgr.close (InterfaceMgr.A3AUCTION);
end;

a3_auction.onSwitch = function (tc)
	
end