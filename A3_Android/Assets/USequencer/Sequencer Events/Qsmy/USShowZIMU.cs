using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
    [USequencerFriendlyName("ZI-MU")]
    [USequencerEvent("Qsmy/ZI-MU")]
    public class USShowZIMU : USEventBase
    {
        //需要有个挂接的点
        public string m_strZiMuText = "你好";
        public override void FireEvent()
        {
            gameST.REV_ZIMU_TEXT(m_strZiMuText);
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
    }
}