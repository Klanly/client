using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/BlurUI")]
public class BlurUI : BaseMeshEffect//BaseVertexEffect
{
    [SerializeField]
    private float blurX = 1f;
    [SerializeField]
    private float blurY = 1f;

    private int nShader_X = 0;
    private int nShader_Y = 0;
    private Material matBlurUI = null;

    //5.4没这个函数了
    //public override void ModifyVertices(List<UIVertex> vertexList)
    //{
    //    if (!IsActive())
    //    {
    //        return;
    //    }
    //}

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
        {
            return;
        }
    }

    void Start()
    {
        nShader_X = Shader.PropertyToID("_X");
        nShader_Y = Shader.PropertyToID("_Y");

        matBlurUI = Resources.Load<Material>("uifx/ui_blur");
        Image pic_image = GetComponent<Image>();
        pic_image.material = matBlurUI;
    }

    void Update()
    {
        Image pic_image = GetComponent<Image>();
        pic_image.material.SetFloat(nShader_X, blurX);
        pic_image.material.SetFloat(nShader_Y, blurY);
    }
}
