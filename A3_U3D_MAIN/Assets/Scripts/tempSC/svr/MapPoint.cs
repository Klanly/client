using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;

[AddComponentMenu("服务器地图参数/地图点（怪物点，出生点）")]
public class MapPoint : MonoBehaviour
{
    public bool 玩家出生点 = false;
    public int 玩家出生点地图id = 0;
    public int 点x_非必填 = 0;
    public int 点y_非必填 = 0;
    public int 范围W = 1;
    public int 范围H = 1;

    public int 怪物id = 1;
    public int 重生时间 = 5000;
    public int 阵营id = 0;

 
}
