using UnityEngine;
using System.Collections;

public class fx : MonoBehaviour {

    public  static int value = 10;  //大于当前canvas 

	// Use this for initialization
	void Start () {

        ShowFx();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    [ContextMenu("ShowFx")]
    public void ShowFx() {

        var renders = this.gameObject.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < renders.Length; i++)
        {
            renders[i].sortingOrder = value;

        }

    }


}
