using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/ColorCorrect_UI")]
public class ColorCorrect_UI : BaseMeshEffect//BaseVertexEffect
{
    [SerializeField]
    private float Brightness = 1f; //亮度
    [SerializeField]
    private float Saturation = 1f; //饱和度
    [SerializeField]
    private float Contrast = 1f; //对比度

    private int nShader_B = 0;
    private int nShader_S = 0;
    private int nShader_C = 0;
    private Material matCCUI = null;

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
        nShader_B = Shader.PropertyToID("_BrightnessAmount");
        nShader_S = Shader.PropertyToID("_SaturationAmount");
        nShader_C = Shader.PropertyToID("_ContrastAmount");

        matCCUI = Resources.Load<Material>("uifx/ui_color_correct");
        Image pic_image = GetComponent<Image>();
        pic_image.material = matCCUI;
    }

    void Update()
    {
        Image pic_image = GetComponent<Image>();
        pic_image.material.SetFloat(nShader_B, Brightness);
        pic_image.material.SetFloat(nShader_S, Saturation);
        pic_image.material.SetFloat(nShader_C, Contrast);
    }
}
