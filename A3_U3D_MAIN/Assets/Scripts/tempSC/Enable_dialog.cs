using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MuGame;

[AddComponentMenu("COMM_COMPONENT/DIALOG")]
class Enable_dialog : MonoBehaviour
{
    public List<string> desc;

    public GameObject npcrole;
    public int descid;
    void Start()
    {
        if (desc != null)
        {
            TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
            th.type = 11;
            th.dialogid = descid;
            //th.paramStr = desc;
            th.paramGo = new List<GameObject> { npcrole};
        }


        Destroy(this);
    }

}

