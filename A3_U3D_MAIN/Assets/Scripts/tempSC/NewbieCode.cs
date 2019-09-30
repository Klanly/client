

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MuGame;
[AddComponentMenu("COMM_COMPONENT/新手脚本")]
public class NewbieCode : MonoBehaviour
{
    public List<string> codes;
    public List<string> waitCodes;
    public int codesid = 0;
    public int waitCodesid = 0;
    void Start()
    {
        if (codesid > 0)
            NewbieTeachMgr.getInstance().add(0,codesid, -1);


        if (codesid <= 0 && waitCodesid <= 0)
        {
            Destroy(gameObject);
            return;
        }

        TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
        th.type = 13;
        th.waitCodesid = waitCodesid;
        //th.paramStr = waitCodes;
        Destroy(this);
    }
}
