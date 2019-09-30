
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MuGame;

[AddComponentMenu("COMM_COMPONENT/act")]
class Enable_act : MonoBehaviour
{
    public string act="";
    public GameObject role;
    void Start()
    {
        if (act!="")
        {
          
            TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
            th.type = 12;
            th.paramStr = new List<string> { act};
            if(role!=null)
                th.paramGo = new List<GameObject> { role };
        }


        Destroy(this);
    }

}
