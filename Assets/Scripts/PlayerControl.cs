using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	public Rigidbody2D rb;
	public int thrust = 1;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		float moveX = Input.GetAxis ("Horizontal");
		float moveY = Input.GetAxis ("Vertical");
		
		Vector3 force = new Vector2 ( moveX, moveY );
		
		rb.AddForce ( force * thrust * Time.deltaTime );
	}
}
