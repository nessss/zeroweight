using UnityEngine;
using System.Collections;

public class Bump : MonoBehaviour {
	public float bumpForce = 5f;

	Thrust thrust;

	void Start(){
		thrust = GetComponent<Thrust>();
	}

	void Update(){
		if(Input.GetButtonDown("Debug")){
			Debug.Log(rigidbody.GetPointVelocity(transform.position));
		}
	}

	void OnCollisionEnter(Collision collision){
		foreach(ContactPoint contact in collision.contacts){
			Debug.DrawRay(contact.point, contact.normal, Color.white);
			rigidbody.AddForce(contact.normal * bumpForce * -1f);
		}
		thrust.disrupt(0.5f);
	}
}
