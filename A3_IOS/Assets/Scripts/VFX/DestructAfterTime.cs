using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructAfterTime : MonoBehaviour {
	
	public float time = 1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(time>0f)
		{
			time -= Time.deltaTime;
		}
		else
		{
			ParticleEmitter[] pars = (ParticleEmitter[])transform.GetComponentsInChildren<ParticleEmitter>();
			foreach(ParticleEmitter par in pars)
			{
				if(par.maxEnergy == 0 && par.minEnergy == 0)
				{
					// Destroy the never end pars
					Destroy(par.gameObject);
					continue;
				}
				
				// Stop emitting.And set the particle autodestruct
				par.emit = false;
				ParticleAnimator parAnim = par.GetComponent<ParticleAnimator>();
				if(parAnim)
					parAnim.autodestruct = true;
			}
			
			if (transform.parent)
			{
				//List<Transform> children = new List<Transform>();
				foreach (Transform child in transform) 
				{
					child.parent = transform.parent;
				}
			}
			else
				transform.DetachChildren();
			
			Destroy(gameObject);
		}
	}
}
