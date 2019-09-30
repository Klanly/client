using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
    [USequencerFriendlyName("Cm_DF")]
	[USequencerEvent("Camera/Cam - Depth Of Field")]
	public class USCamera_DepthOfField : USEventBase 
	{
        public GameObject m_LinkerCamera = null;

        public Dof34QualitySetting quality = Dof34QualitySetting.OnlyBackground;
        public DofResolution resolution = DofResolution.Low;
        public bool simpleTweakMode = true;
        public float focalPoint = 1.0f;
        public float smoothness = 0.5f;
        public float focalZDistance = 0.0f;
        public float focalZStartCurve = 1.0f;
        public float focalZEndCurve = 1.0f;
        public Transform objectFocus = null;
        public float focalSize = 0.0f;
        public DofBlurriness bluriness = DofBlurriness.Low;
        public float maxBlurSpread = 1.75f;

        public override void FireEvent()
        {
            if (m_LinkerCamera != null)
            {
				//使用新的特效摄像机
                string curplot_camera_rpath = "plot_c/plotcam_depth_of_field";
                GameObject curplot_camera_prefab = Resources.Load(curplot_camera_rpath) as GameObject;
                if (curplot_camera_prefab != null)
                {
                    Camera cam = m_LinkerCamera.GetComponent<Camera>();
                    GameObject child_plot_cam = GameObject.Instantiate(curplot_camera_prefab) as GameObject;
                    child_plot_cam.transform.SetParent(m_LinkerCamera.transform, false);

                    Camera cm_eff = child_plot_cam.GetComponent<Camera>();
                    cm_eff.fieldOfView = cam.fieldOfView;

                    DepthOfField34 dof = child_plot_cam.GetComponent<DepthOfField34>();
                    dof.quality = quality;
                    dof.resolution = resolution;
                    dof.simpleTweakMode = simpleTweakMode;
                    dof.focalPoint = focalPoint;
                    dof.smoothness = smoothness;
                    dof.focalZDistance = focalZDistance;
                    dof.focalZStartCurve = focalZStartCurve;
                    dof.focalZEndCurve = focalZEndCurve;
                    dof.objectFocus = objectFocus;
                    dof.focalSize = focalSize;
                    dof.bluriness = bluriness;
                    dof.maxBlurSpread = maxBlurSpread;

                    if (cam != null) cam.enabled = false;
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
	}
}