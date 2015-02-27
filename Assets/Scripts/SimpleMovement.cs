using UnityEngine;
using System.Collections;

public class SimpleMovement : MonoBehaviour 
{
	public float speed = 10.0f;

	public Vector3 movementVector;
	
	private void Update () 
	{
		this.gameObject.transform.Translate(this.movementVector * this.speed * Time.deltaTime);
	}
}
