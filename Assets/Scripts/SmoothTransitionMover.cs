using UnityEngine;
using System.Collections;

public class SmoothTransitionMover : MonoBehaviour {

	public Vector3 startMarker;
	public Vector3 endMarker;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;

	private bool isMoving = false;

	void Start() {
		startTime = Time.time;
		startMarker = transform.position;
		endMarker = transform.position;

		journeyLength = Vector3.Distance(startMarker, endMarker);
	}

	void FixedUpdate() {
		if (isMoving) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			if (journeyLength != 0f) {
				transform.position = Vector3.Lerp (startMarker, endMarker, Mathf.SmoothStep (0f, 1f, fracJourney));
			} 
			if (fracJourney >= 1)
				isMoving = false;
		}
	}

	public bool SmoothlyMoveTo( Vector3 targetPosition, float transitionTime ){
		bool sueccesful = false;
		if (!isMoving) {
			sueccesful = true;
			isMoving = true;

			startTime = Time.time;
			startMarker = transform.position;
			endMarker = targetPosition;

			journeyLength = Vector3.Distance (transform.position, targetPosition);

			speed = journeyLength / transitionTime;
		}
		return sueccesful;
	}
}
