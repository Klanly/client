using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
    [USequencerFriendlyName("Res PlaySound")]
    [USequencerEvent("Qsmy/Res_PlaySound")]
    public class USResPlaySound : USEventBase
    {
        //需要有个挂接的点
        public GameObject m_LinkerObject = null;
        public float m_fSoundVolume = 1.0f;
        public override void FireEvent()
        {
            if (!GlobleSetting.SOUND_ON)
                return;

            if (m_LinkerObject != null)
            {
                AudioSource audio_src = m_LinkerObject.GetComponent<AudioSource>();
                if (audio_src != null) {
                    audio_src.Play();
                    audio_src.volume = m_fSoundVolume;
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
    }
}