using UnityEngine;
using System.Collections;
using MuGame;

namespace MuGame
{
	class UtilsOut
	{
        public static void init()
        {
           //gameST.req
            gameST.REQ_SET_FAST_BLOOM = setFastBloomParm;
         //  gameST.req
        }

        public static void setFastBloomParm(GameObject go, float intensity)
        {
            FastBloom fa = go.GetComponent<FastBloom>();
            if (fa == null)
                return;
            if (intensity < 0f)
                return;
            if (intensity > 2.5f)
                return;
            fa.intensity = intensity;
        }
	}
}
