using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
    [USequencerFriendlyName("ZI-MU")]
    [USequencerEvent("Qsmy/ZI-MU")]
    public class USShowZIMU : USEventBase
    {
        //��Ҫ�и��ҽӵĵ�
        public string m_strZiMuText = "���";
        public override void FireEvent()
        {
            gameST.REV_ZIMU_TEXT(m_strZiMuText);
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
    }
}