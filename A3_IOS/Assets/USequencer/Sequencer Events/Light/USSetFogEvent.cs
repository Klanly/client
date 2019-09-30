using UnityEngine;
using System.Collections;

namespace WellFired
{
	[USequencerFriendlyName("Set Fog")]
	[USequencerEvent("Light/Set Fog")]
	public class USSetFogEvent : USEventBase
	{
        public bool fog_enable = true;
        public float fog_density = 0.01f;
	    public Color fogColor = Color.white;
        public float fog_start = 0.0f;
        public float fog_end = 300.0f;
        public FogMode fog_mode = FogMode.Linear;
		
		public override void FireEvent()
		{
            RenderSettings.fog = fog_enable;
            RenderSettings.fogDensity = fog_density;
            RenderSettings.fogStartDistance = fog_start;
            RenderSettings.fogEndDistance = fog_end;
            RenderSettings.fogMode = fog_mode;

            RenderSettings.fogColor = fogColor;
		}
		
		public override void ProcessEvent(float deltaTime)
		{
			
		}
		
		public override void StopEvent()
		{
			//UndoEvent();
		}
		
		public override void UndoEvent()
		{
			//RenderSettings.ambientLight = prevLightColor;
		}
	}
}