using UnityEngine;
using System.Collections;

public class Lifetime : MonoBehaviour 
{
	public float lifetime = 5.0f;

	private float _timer = 0.0f;

	void Update () 
	{
		this._timer += Time.deltaTime;

		if(this._timer >= this.lifetime)
		{
			Destroy(gameObject);
		}
	}
}
