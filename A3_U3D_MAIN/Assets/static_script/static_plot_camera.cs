﻿using UnityEngine;

public class static_plot_camera : MonoBehaviour
{
    //不能在激活的时候激活  ??
    //void OnEnable()
    //{
    //    if (this.transform.GetChildCount() > 0)
    //    {
    //        debug.Log("GetChildCountGetChildCountGetChildCountGetChildCountGetChildCountGetChildCount");
    //        GameObject plot_cam = this.transform.GetChild(0).gameObject;

    //        Debug.Log(plot_cam.name);

    //        plot_cam.SetActive(true);
    //    }

    //    debug.Log("OnEnableOnEnableOnEnableOnEnableOnEnableOnEnableOnEnableOnEnableOnEnableOnEnableOnEnableOnEnableOnEnableOnEnableOnEnableOnEnable");
    //}

    void FixedUpdate()
    {
        //if (this.active)
        if (this.isActiveAndEnabled)
        {
            if (this.transform.childCount > 0)
            {
                GameObject plot_cam = this.transform.GetChild(0).gameObject;
                plot_cam.SetActive(true);
            }
        }
    }

}
