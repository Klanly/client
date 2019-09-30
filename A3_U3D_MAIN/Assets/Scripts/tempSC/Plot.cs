using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;
[AddComponentMenu("COMM_COMPONENT/Plot")]
public class Plot : MonoBehaviour
{
    public bool activeUI = true;
    public GameObject tragetPlot = null;
    public float speed = 10f;

    void Start()
    {
        TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
        th.type = 6;

        if (tragetPlot == null)
        {
            Debug.LogError("Plot缺少参数tragetPlot::" + gameObject.name);
            Destroy(this);
            return;
        }
       // tragetPlot.SetActive(false);

        Transform transCam = tragetPlot.transform.FindChild("c");
        if (transCam == null)
        {
            Debug.LogError("Plot缺少参数c::" + tragetPlot.name);
            Destroy(this);
            return;
        }
        Transform transE = tragetPlot.transform.FindChild("e");
        if (transE == null)
        {
            Debug.LogError("Plot缺少参数e::" + tragetPlot.name);
            Destroy(this);
            return;
        }
        Dictionary<string, GameObject> d = new Dictionary<string, GameObject>();
        int len = transE.childCount;
        for (int i=0;i<len;i++)
        {
            GameObject go = transE.GetChild(i).gameObject;
            d[go.name] =go;
        }

       


        GameObject mainplot = transCam.transform.parent.gameObject;
        if (mainplot.GetComponent<GameAniCamera>() != null)
            return;

        GameAniCamera gac =mainplot .AddComponent<GameAniCamera>();
        gac.dEvt = d;
        gac.speed = speed;
        gac.uiactive = activeUI;
        th.paramGo = new List<GameObject>() { transCam.gameObject, mainplot };


        th.paramBool = activeUI;
        th.paramFloat = new List<float>() { speed };
        Destroy(this);
    }

    public void shake(string pram)
    {

    }

    public void doit(string id)
    {

    }
}

