using UnityEngine;
using System.Collections;

public class InEditorDo : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
#if UNITY_EDITOR
        FastBloom.FAST_BLOOMSHADER = Resources.Load<Shader>("ieshader/MobileBloom");

        DepthOfField34.DOF_BLUR_SHADER = Resources.Load<Shader>("ieshader/SeparableWeightedBlurDof34");
        DepthOfField34.DOF_SHADER = Resources.Load<Shader>("ieshader/DepthOfField34");
        DepthOfField34.BOKEN_SHADER = Resources.Load<Shader>("ieshader/Bokeh34");
#endif

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
