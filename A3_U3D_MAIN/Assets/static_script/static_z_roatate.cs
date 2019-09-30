using UnityEngine;
using System.Collections;

public class static_z_roatate : MonoBehaviour
{
    public float Speed = 0.2f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Speed);
    }
}
