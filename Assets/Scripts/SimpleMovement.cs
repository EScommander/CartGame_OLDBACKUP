using UnityEngine;
using System.Collections;

public class SimpleMovement : MonoBehaviour 
{
	public float speed = 10.0f;

	private void Update () 
	{
		this.gameObject.transform.Translate(Vector3.forward * this.speed * Time.deltaTime);
	}
}
