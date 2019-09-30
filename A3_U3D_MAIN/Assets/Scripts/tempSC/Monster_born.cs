using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MuGame;
[AddComponentMenu("COMM_COMPONENT/HANDLE_MONSTER_BORN")]
public class Monster_born : MonoBehaviour
{
    public int monsterId = 0;

    void Start()
    {
        if (monsterId == 0)
            return;

        TriggerHanldePoint th = gameObject.AddComponent<TriggerHanldePoint>();
        th.type = 1;
        th.paramInts = new List<int> { monsterId };
        Destroy(this);
    }
}
