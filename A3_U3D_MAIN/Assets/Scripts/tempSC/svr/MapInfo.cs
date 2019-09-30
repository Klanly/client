using UnityEngine;
using System.Collections;

[AddComponentMenu("服务器地图参数/地图信息（每张地图只能有一个）")]
public class MapInfo : MonoBehaviour 
{
    // <m id="1" name="1" tile_size="32" width="250" height="333" tile_set="1" pk="1">
    public int 地图id = 0;
    public string 地图名 = "--";
    public bool 是否是pk地图 = false;


	// Use this for initialization
	void Start () 
    {
	

	}
	
	
}
