using UnityEngine;
using System;
using System.Collections;

public class test_ResourceOut : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        //GetComponent<MeshRenderer>().sharedMaterial.shader.
        GetComponent<MeshRenderer>().sharedMaterial.shader = Shader.Find("Unlit/Texture");
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        GC.Collect();
        Resources.UnloadUnusedAssets();
        GC.Collect();

        //ShaderVariantCollection.Clear();
    }
}
