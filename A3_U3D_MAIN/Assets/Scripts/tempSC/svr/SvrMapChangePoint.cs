using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;
public class SvrMapChangePoint : MonoBehaviour
{
    public int id = 0;
    public int 目标地图id = 0;
    public int 目标x;
    public int 目标y;
    public float 目标点朝向=0;

    //void Start()
    //{
    //    MeshFilter mf = gameObject.GetComponent<MeshFilter>();
    //    if (mf != null)
    //        Destroy(mf);
    //    MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
    //    if (mr != null)
    //        Destroy(mr);

    //    gameObject.name = "changeMapTo" + 目标地图id;
    //    ChangePoint tp = gameObject.AddComponent<ChangePoint>();
    //    tp.paramInts = new List<int>() { id };
    //    tp.type = 1;
    //    Destroy(this);
    //}
}

