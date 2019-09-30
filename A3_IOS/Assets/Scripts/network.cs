using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MuGame;
using System.Collections.Generic;

public class network : MonoBehaviour 
{

    public static network _instance;
    void start()
    {
        _instance = this;
    }
    void networkState()
    {
        //无网络
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            
        }
        //数据网
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {

        }
        //wifi
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {

        }
    }




}
