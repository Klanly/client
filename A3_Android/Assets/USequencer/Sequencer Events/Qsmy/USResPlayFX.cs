using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
    [USequencerFriendlyName("Res PlayFX")]
    [USequencerEvent("Qsmy/Res_PlayFX")]
    public class USResPlayFX : USEventBase
    {
        //需要有个挂接的点
        public GameObject[] m_LinkerObject;
        public override void FireEvent()
        {
			for( int i = 0; i < m_LinkerObject.Length; i++ ){
                GameObject curobj = m_LinkerObject[i];
                curobj.SetActive(true);
			}
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
    }
}