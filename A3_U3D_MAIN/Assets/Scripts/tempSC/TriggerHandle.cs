using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MuGame;
[AddComponentMenu("COMM_COMPONENT/TriggerHanlde")]
public class TriggerHandle : MonoBehaviour {

    public List<int> paramInts = new List<int>();
	void Start () 
    {
     TriggerHanldePoint th=  gameObject.AddComponent<TriggerHanldePoint>();
     th.paramInts = paramInts;
        Destroy(this);
	}
	
	
}
