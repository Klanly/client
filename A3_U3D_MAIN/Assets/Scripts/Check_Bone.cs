using UnityEngine;
using MuGame;
using System.Collections;

public class Check_Bone : MonoBehaviour {

	// Use this for initialization

    public GameObject mesh_a;
    public GameObject mesh_b;

	void Start () {
        Transform[] a_obj_tf = mesh_a.GetComponent<SkinnedMeshRenderer>().bones;
        Transform[] b_obj_tf = mesh_b.GetComponent<SkinnedMeshRenderer>().bones;

        debug.Log("a 骨骼数 " + a_obj_tf.Length + "      b 骨骼数 "+ b_obj_tf.Length);
        for (int i = 0; i < a_obj_tf.Length; i++)
        {
            bool finded = false;
            for (int j = 0; j < b_obj_tf.Length; j++)
            {
                if (a_obj_tf[i].name == b_obj_tf[j].name)
                {
                    finded = true;
                    break;
                }

            }

            if (finded == false)
            {
                debug.Log("a name = " + a_obj_tf[i].name);
            }
        }


        for (int i = 0; i < b_obj_tf.Length; i++)
        {
            bool finded = false;
            for (int j = 0; j < a_obj_tf.Length; j++)
            {
                if (b_obj_tf[i].name == a_obj_tf[j].name)
                {
                    finded = true;
                    break;
                }

            }

            if (finded == false)
            {
                debug.Log("b name = " + b_obj_tf[i].name);
            }
        }





	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
