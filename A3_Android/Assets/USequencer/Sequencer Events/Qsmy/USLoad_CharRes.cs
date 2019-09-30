using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
    [USequencerFriendlyName("Load Char")]
	[USequencerEvent("Qsmy/Load_CharRes")]
	public class USLoad_CharRes : USEventBase 
	{
        //需要有个挂接的点
        public GameObject[] m_LinkerObject;
        public int m_nResID = -1;
        public string[] m_strResAnim;
        public PLOT_CHARRES_TYPE m_eType = PLOT_CHARRES_TYPE.PCRT_HERO;

        public override void FireEvent()
        {
            if (m_LinkerObject.Length > 0 && m_nResID > 0)
            {
                gameST.REV_CHARRES_LINKER(m_LinkerObject, m_eType, m_nResID, m_strResAnim);
            }
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
	}
}