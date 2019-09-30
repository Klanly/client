using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
    [USequencerFriendlyName("Load Sound")]
	[USequencerEvent("Qsmy/Load_SoundRes")]
	public class USLoad_SoundRes : USEventBase 
	{
        public GameObject m_LinkerObject = null;
        public int m_nPlotSoundID = -1;
        public override void FireEvent()
        {
            if (m_LinkerObject != null && m_nPlotSoundID > 0)
            {
                //º”»Î“Ù¿÷
                gameST.REV_SOUNDRES_LINKER(m_LinkerObject, m_nPlotSoundID);
            }
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
	}
}