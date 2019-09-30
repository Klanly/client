using UnityEngine;
using System.Collections;
using MuGame;
using System.Collections.Generic;
[AddComponentMenu("COMM_COMPONENT/NPC")]
public class NpcSC : MonoBehaviour {

    public string rolename = "路人";
    public string openID="";
    public List<string> desc = new List<string>{"0:你好！","1:你好！"};
    public Vector3 offset = new Vector3(-131.4f, -4.25f, -128f);
    public Vector3 scale = new Vector3(2.5f, 2.5f, 2.5f);

    public bool navmesh = true;
	void Start () {
        NpcRole th = gameObject.AddComponent<NpcRole>();
        th.openid = openID;
        th.lDesc = desc;
        th.id = int.Parse(gameObject.name);
       // th.name = rolename;
        th.talkOffset = offset;
        th.talkScale = scale;
        th.nav = navmesh;
        Destroy(this);
	}
}
