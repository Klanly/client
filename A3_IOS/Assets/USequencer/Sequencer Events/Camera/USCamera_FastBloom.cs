using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
    [USequencerFriendlyName("Cm_FB")]
	[USequencerEvent("Camera/Cam - Fast Bloom")]
	public class USCamera_FastBloom : USEventBase 
	{
        public GameObject m_LinkerCamera = null;
    	public float threshhold = 0.25f;
        public float intensity = 0.75f;
        public float blurSize = 1.0f;
        public FastBloom.Resolution resolution = FastBloom.Resolution.Low;
        public int blurIterations = 1;
        public FastBloom.BlurType blurType = FastBloom.BlurType.Standard;

        public override void FireEvent()
        {
            if (m_LinkerCamera != null)
            {
				//使用新的特效摄像机
                string curplot_camera_rpath = "plot_c/plotcam_fastbloom";
                GameObject curplot_camera_prefab = Resources.Load(curplot_camera_rpath) as GameObject;
                if (curplot_camera_prefab != null)
                {
                    Camera cam = m_LinkerCamera.GetComponent<Camera>();
                    GameObject child_plot_cam = GameObject.Instantiate(curplot_camera_prefab) as GameObject;
                    child_plot_cam.transform.SetParent(m_LinkerCamera.transform, false);

                    Camera cm_eff = child_plot_cam.GetComponent<Camera>();
                    cm_eff.fieldOfView = cam.fieldOfView;

                    FastBloom fb = child_plot_cam.GetComponent<FastBloom>();
                    fb.threshhold = threshhold;
                    fb.intensity = intensity;
                    fb.blurSize = blurSize;
                    fb.resolution = resolution;
                    fb.blurIterations = blurIterations;
                    fb.blurType = blurType;

                    if (cam != null) cam.enabled = false;
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
	}
}