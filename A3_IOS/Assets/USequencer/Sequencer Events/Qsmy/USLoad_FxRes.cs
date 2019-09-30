using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
    [USequencerFriendlyName("Load FX")]
	[USequencerEvent("Qsmy/Load_FX")]
	public class USLoad_FXRes : USEventBase 
	{
        //需要有个挂接的点
        public GameObject[] m_LinkerObject;
        public string m_strRFXRes;
        public override void FireEvent()
        {
            if (m_LinkerObject.Length > 0 && m_strRFXRes != null)
            {
                gameST.REV_FXRES_LINKER(m_LinkerObject, m_strRFXRes);
            }
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
	}
}