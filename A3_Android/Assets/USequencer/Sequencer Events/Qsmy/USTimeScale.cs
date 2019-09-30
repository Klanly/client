using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
    [USequencerFriendlyName("TS")]
    [USequencerEvent("Qsmy/TS")]
    public class USTimeScale : USEventBase
    {
        //��Ҫ�и��ҽӵĵ�
        public float m_ftimeScale = 1f;
        public override void FireEvent()
        {
            Globle.setTimeScale(m_ftimeScale);
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
    }
}