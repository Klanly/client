using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
    [USequencerFriendlyName("Plot PlayOver")]
	[USequencerEvent("Qsmy/PlotPlayOver")]
	public class USPlotPlayOverEvent : USEventBase 
	{
        //public USSequencer sequence = null;

        public override void FireEvent()
        {
            Debug.Log("¾çÇé²¥·Å½áÊø");
            PlotMain._inst.GamePoltPlayOver();
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
	}
}