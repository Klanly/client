using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MuGame;
[AddComponentMenu("COMM_COMPONENT/HANDLE_WALK_LAYER")]
public class Walk_Layer : MonoBehaviour
{
    public string layers = "";

    void Start()
    {
        if (layers != "")
        {
            List<int> list = new List<int>();
            string[] arr = layers.Split(',');
            for (int i = 0; i < arr.Length;i++ )
            {
                list.Add(int.Parse(arr[i]));  
            }
            TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
            th.type = 4;
            th.paramInts = list;
          
        }


        Destroy(this);
    }
}
