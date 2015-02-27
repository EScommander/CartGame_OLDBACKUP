using UnityEngine;
using System.Collections;

public class CartController : MonoBehaviour 
{
	public static float cartGravity = 100.0f;

	public float handling = 10.0f;
	public float acceleration = 30.0f;
	public float topSpeed = 100.0f;

	public float traction = 1.0f;
	public float rotationalTraction = 2.0f;

	public float suspension = 0.1f;

	public float Speedometer = 0.0f;

	public ParticleSystem driftFX;

	private bool hasTraction = true;
	private float forwardAcceleration = 30.0f;
	private float steerHandling = 10.0f;

	//Jer//
	public GameObject[] turnableWheels;
	private float maxTurnDeg = 75;
	private float steeringInput = 0;
	private float accelInput = 0;
	//Jer//

	private bool drifting = false;

	// Update is called once per frame
	void Update () 
	{
		if(networkView.isMine || NetworkManager.GetInstance().isOnline == false)
		{
			Debug.DrawRay (transform.position, transform.forward * 10.0f);

			if (this.rigidbody == null)
				return;

			if(Camera.main.transform.IsChildOf(transform))
			{
				if(Input.GetKey (KeyCode.LeftShift))
				{
					Camera.main.transform.localPosition = new Vector3(0,1.4f,2.15f);
					Camera.main.transform.localRotation = Quaternion.Euler(24.0f,180.0f,0.0f);
				}
				else
				{
					Camera.main.transform.localPosition = new Vector3(0,1.4f,-2.15f);
					Camera.main.transform.localRotation = Quaternion.Euler(24.0f,0.0f,0.0f);
				}
			}

			if (Input.GetButton("Drift") ) 
			{
				this.rigidbody.drag = this.traction / 1.5f;
				this.forwardAcceleration = acceleration / 2.0f;
				this.steerHandling = this.handling * 2.0f;
				drifting = true;

				if(this.driftFX != null)
				{
					this.driftFX.enableEmission = true;
				}
			}
			else
			{
				this.rigidbody.drag = this.traction;
				this.forwardAcceleration = acceleration;
				this.steerHandling = this.handling;
				drifting = false;

				if(this.driftFX != null)
				{
					this.driftFX.enableEmission = false;
				}
			}

			this.rigidbody.angularDrag = this.rotationalTraction;

			if (hasTraction) 
			{
				//JER//

				steeringInput = 0;
				steeringInput = Input.GetAxis ("JoyX0");
				accelInput = Input.GetAxis("R_Trigger");

				//JER//

				if (Input.GetKey (KeyCode.W)) 
				{
					accelInput = 1;
					//this.rigidbody.AddForce (transform.forward * forwardAcceleration);
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
				this.rigidbody.AddTorque(transform.up * steeringInput * steerHandling * Time.deltaTime);

				//Potential 4-wheel handling, WIP
				if(!drifting)
				{
					this.rigidbody.velocity = Vector3.RotateTowards(this.rigidbody.velocity, transform.forward, this.traction * 0.3f * Time.deltaTime, 0.0f);
				}
 	
				if(turnableWheels.Length > 0)
				{
					for(int i = 0; i < turnableWheels.Length; i++)
					{
						turnableWheels[i].transform.localEulerAngles = new Vector3(0, steeringInput * maxTurnDeg, 0);
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

				if(hit.distance <= suspension * 1.1f)
				{
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0.0f, rigidbody.velocity.z);
				}
			}

			if(rigidbody.velocity.x >= this.topSpeed)
			{
				rigidbody.velocity = new Vector3(this.topSpeed, rigidbody.velocity.y, rigidbody.velocity.z);
			}
		
			this.Speedometer = rigidbody.velocity.magnitude;
		}

	}
}
