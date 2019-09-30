using System;
using MuGame;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu("COMM_COMPONENT/Trigger")]
public class Trigger : MonoBehaviour
{
 //   public int type = -1;
    public int triggerTimes = 1;
  //  public List<int> paramInts = new List<int>();
  //  public List<GameObject> paramGameobjects = new List<GameObject>();

    
    // Use this for initialization
    void Start()
    {
        ChangePoint cp = gameObject.AddComponent<ChangePoint>();
     //  cp.type = type;
       cp.triggerTimes = triggerTimes;
    //   cp.paramInts = paramInts;
   //    cp.paramGameobjects = paramGameobjects;
       Destroy(this);
    }


}
