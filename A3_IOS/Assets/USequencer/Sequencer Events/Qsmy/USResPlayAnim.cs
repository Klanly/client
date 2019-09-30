using UnityEngine;
using System.Collections;
using MuGame;

namespace WellFired
{
    [USequencerFriendlyName("Res PlayAnim")]
    [USequencerEvent("Qsmy/Res_PlayAnim")]
    public class USResPlayAnim : USEventBase
    {
        //需要有个挂接的点
        public GameObject m_LinkerObject = null;
        public string m_strAnimName = "idle";
        public float m_fAnimSpeed = 1.0f;
        public WrapMode m_wmAnimWrapMode = WrapMode.Loop;
        public override void FireEvent()
        {
            if (m_LinkerObject != null)
            {
                int child_count = m_LinkerObject.transform.childCount;
                for (int i = 0; i < child_count; i++)
                {
                    GameObject child = m_LinkerObject.transform.GetChild(i).gameObject;
                    Animation animation = child.GetComponent<Animation>();
                    if (animation != null)
                    {
                        if (animation.GetClip(m_strAnimName) != null)
                        {
                            animation[m_strAnimName].speed = m_fAnimSpeed;
                            animation.Play(m_strAnimName);
                            animation.wrapMode = m_wmAnimWrapMode;
                        }
                    }

                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {

        }
    }
}