using UnityEngine;
using System.Collections;

public class CartController : MonoBehaviour 
{
	public static float cartGravity = 10.0f;

	public float handling = 10.0f;
	public float acceleration = 30.0f;
	public float topSpeed = 100.0f;

	public float traction = 1.0f;
	public float rotationalTraction = 2.0f;

	public float suspension = 0.1f;

	public float Speedometer = 0.0f;

	private bool hasTraction = true;
	private float forwardAcceleration = 30.0f;

	//Jer//
	public GameObject[] turnableWheels;
	private float maxTurnDeg = 75;
	//Jer//

	// Update is called once per frame
	void Update () 
	{
		Debug.DrawRay (transform.position, transform.forward * 10.0f);

		if (this.rigidbody == null)
			return;

		if (Input.GetKey(KeyCode.Space)) 
		{
			this.rigidbody.drag = this.traction / 2.0f;
			this.forwardAcceleration = acceleration / 2.0f;
		}
		else
		{
			this.rigidbody.drag = this.traction;
			this.forwardAcceleration = acceleration;
		}

		this.rigidbody.angularDrag = this.rotationalTraction;

		if (hasTraction) 
		{
			//JER//
			float steeringInput = Input.GetAxis ("JoyX0");
			float accelInput = Input.GetAxis("R_Trigger");
			//JER//

			if (Input.GetKey (KeyCode.W)) 
			{
				accelInput = 1;
				this.rigidbody.AddForce (transform.forward * forwardAcceleration);
			}
			if (Input.GetKey (KeyCode.S)) 
			{
				accelInput = -1;
				//this.rigidbody.AddForce (-transform.forward * forwardAcceleration);
			}

			if (Input.GetKey (KeyCode.A)) 
			{
				steeringInput = -1;
				//this.rigidbody.AddTorque(-transform.up * handling);
			}

			if (Input.GetKey (KeyCode.D)) 
			{
				steeringInput = 1;
				//this.rigidbody.AddTorque(transform.up * handling);
			}

			//****Jer's Code**//
			this.rigidbody.AddForce((transform.forward * accelInput * forwardAcceleration));
			this.rigidbody.AddTorque(transform.up * steeringInput * handling);

			if(turnableWheels.Length > 0)
			{
				for(int i = 0; i < turnableWheels.Length; i++)
				{
					turnableWheels[i].transform.eulerAngles = new Vector3(0, steeringInput * maxTurnDeg, 0);
				}
			}
			//****Jer's Code END**//
		}

		RaycastHit hit;

		if(Physics.Raycast(new Ray(transform.position, Vector3.down), out hit))
		{
			if(hit.distance > suspension)
			{
				this.rigidbody.AddForce(Vector3.down * cartGravity);
			}
			else if(hit.distance < suspension)
			{
				this.rigidbody.AddForce(Vector3.up * cartGravity);
			}
		}

		if(rigidbody.velocity.x >= this.topSpeed)
		{
			rigidbody.velocity = new Vector3(this.topSpeed, rigidbody.velocity.y, rigidbody.velocity.z);
		}
	
		this.Speedometer = rigidbody.velocity.magnitude;

	}
}
