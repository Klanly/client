using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MuGame;
[AddComponentMenu("COMM_COMPONENT/HANDLE_UNENABLE_ITEM")]
public class Unenable_item : MonoBehaviour
{
    public List<GameObject> item;
    
    void Start()
    {
        if (item == null)
            return;

        TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
        th.type = 3;
        th.paramGo = item;
        Destroy(this);
    }
}
