using UnityEngine;
using System.Collections;

public class SimpleRotation : MonoBehaviour 
{
	public float speed = 10.0f;

	private void Update () 
	{
		this.gameObject.transform.Rotate (Vector3.up, this.speed * Time.deltaTime);
	}
}
