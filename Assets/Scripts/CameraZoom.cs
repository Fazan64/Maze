using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	public float start;
	public float target;

	private float speed = 1.0F;
	private float startTime;
	private float journeyLength;

	public Camera camera;

	void Awake() {
		camera = gameObject.GetComponent<Camera> ();
	}

	void Start() {

		startTime = Time.time;

		if (camera.orthographic)
			start = camera.orthographicSize;
		else
			start = camera.fieldOfView;

		journeyLength = Mathf.Abs (target - start);

	}

	void Update() {

		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		if (camera.orthographic)
			camera.orthographicSize = Mathf.Lerp(start, target, Mathf.SmoothStep(0f, 1f, fracJourney));
		else
			camera.fieldOfView = Mathf.Lerp(start, target, Mathf.SmoothStep(0f, 1f, fracJourney));

	}

	public void SmoothlyMoveTo( float targetSize, float transitionTime ){
		startTime = Time.time;

		if (camera.orthographic)
			start = camera.orthographicSize;
		else
			start = camera.fieldOfView;

		target = targetSize;
		journeyLength = Mathf.Abs (target - start);

		speed = journeyLength / transitionTime;
	}

}
