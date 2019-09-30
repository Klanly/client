using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
	[USequencerFriendlyName("Wait Res Loading")]
	[USequencerEvent("Qsmy/WaitResLoading")]
    public class USWaitResLoadingEvent : USEventBase 
	{
        public USSequencer sequence = null;

        public override void FireEvent()
        {
            if (PlotMain._inst && PlotMain._inst.m_bUntestPlot)
            {
                Debug.Log("资源的加载请求已全发送！！停止播放");

                if (!sequence)
                    Debug.LogWarning("No sequence for USPauseSequenceEvent : " + name, this);

                if (sequence)
                    sequence.Pause();

                PlotMain._inst.m_ePlotStep = PlotMain.ENUM_POLTSTEP.PLSP_Wait_Res;
                PlotMain._inst.m_curSequence = sequence;
                gameST.REV_RES_LIST_OK();
            }
            else
            {
                if (sequence != null)
                    sequence.Pause();
            }
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
	}
}