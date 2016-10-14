using UnityEngine;
using System.Collections;

public class PlayerControlDiscrete : MonoBehaviour {

	public Rigidbody2D rb2D;
	public float moveStep = 5f;
	public float moveTime = 0.2f;
	public bool canMove = true;

	private SmoothTransitionMover mover;

	// Use this for initialization
	void Start () {
		rb2D = GetComponent<Rigidbody2D> ();
		mover = gameObject.GetComponent<SmoothTransitionMover> ();
	}
	
	// Update is called once per frame
	void Update () {
		if ( canMove ) {

			Vector3 movement = GetDesiredMovement ();

			if (movement == Vector3.zero) {return;}

			RaycastHit2D hit = Physics2D.Raycast(
				origin: transform.position, 
				direction: movement, 
				distance: moveStep, 
				layerMask: LayerMask.GetMask("Wall"),
			    minDepth: transform.position.z - 1, 
			    maxDepth: transform.position.z + 1
			);

			if (hit.collider != null) {return;}
		
			Vector3 targetPos = transform.position + movement * moveStep;
			mover.SmoothlyMoveTo (targetPos, moveTime);
		}
	}

	Vector3 GetDesiredMovement() {
		
		Vector3 movement = Vector3.zero;

		if (Input.GetKeyDown (KeyCode.W))
			movement +=  Vector3.up;
		if (Input.GetKeyDown(KeyCode.S))
			movement += -Vector3.up;
		if (Input.GetKeyDown(KeyCode.D))
			movement +=  Vector3.right;
		if (Input.GetKeyDown(KeyCode.A))
			movement += -Vector3.right;

		return movement;
	}
}
