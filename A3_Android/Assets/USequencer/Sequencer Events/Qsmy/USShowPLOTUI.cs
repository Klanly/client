using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
    [USequencerFriendlyName("PL-UI")]
    [USequencerEvent("Qsmy/PL-UI")]
    public class USShowPLOTUI : USEventBase
    {
        //需要有个挂接的点
        public string m_strPlotUI = "";
        public override void FireEvent()
        {
            gameST.REV_PLOT_UI(m_strPlotUI);
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
    }
}