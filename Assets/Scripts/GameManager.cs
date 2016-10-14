using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class GameManager : MonoBehaviour {

	public Level levelPrefab;
	public GameObject playerPrefab;

	public Camera mainCamera;
	public float cameraPreZoomSize = 120f;
	public float cameraDefaultSize = 5f;
	public float zoomTime = 5f;

	public float levelTransitionTime = 10f;
	public float distanceBetweenLevels = 100f;
	public int numberOfPreGeneratedLevels = 2;
	public IntVector2 firstLevelSize = new IntVector2(10,10);
	//how fast difficulty (size) of a level grows
	public float difficultyMultiplier = 1.2f;


	private List<Level> levels = new List<Level>();
	private SmoothTransitionMover playerMover;

	private int currentLevelNumber = 0;
	public Level currentLevel {
		get {
			return levels[currentLevelNumber];
		}
		set {
			currentLevelNumber = levels.IndexOf(value);
		}
	}

	private Canvas UICanvas;
	private bool paused = false;


	//----------------------------------------------------------------------------------------------------


	private void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		playerMover = GameObject.FindGameObjectWithTag("Player").GetComponent<SmoothTransitionMover>();
		UICanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();

		BeginGame();
	}
	
	private void Update () {

		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (paused)
				UnPause(); 
			else
				Pause();
		}

		if (levels.Count >= 1) 
		    if (playerMover.transform.position == currentLevel.maze.GetPositionOfCellAt(currentLevel.maze.exitCoordinates) )
				MoveToNextLevel(playerMover);
	}

	private void BeginGame () {
		ResetCamera ();
		levels = new List<Level> ();

		for (int i = 0; i < numberOfPreGeneratedLevels; i++) {
			CreateLevel();
		}

		currentLevel = levels [0];
		playerMover.transform.position = currentLevel.maze.GetPositionOfCellAt(currentLevel.maze.entranceCoordinates );

	}

	public void RestartGame() {

		StopAllCoroutines();
		foreach (Level level in levels) {
			Destroy(level.gameObject);
		}

		BeginGame();

	}

	private void ResetCamera () {
		mainCamera.orthographicSize = firstLevelSize.x * 2;
		mainCamera.fieldOfView = cameraPreZoomSize;

		CameraZoom cameraZoomer = mainCamera.gameObject.GetComponent<CameraZoom> ();

		cameraZoomer.SmoothlyMoveTo (cameraDefaultSize, zoomTime);
	}

	private Level CreateLevel () {
		Level level = Instantiate(levelPrefab) as Level;
		
		level.number = levels.Count;
		level.mazeSize = firstLevelSize * Mathf.Pow(difficultyMultiplier, level.number);
		
		level.transform.position = new Vector3(0, 0, level.number * distanceBetweenLevels);
		
		level.Initialize();
		
		levels.Add(level);

		return level;
	}

	private void MoveToNextLevel (SmoothTransitionMover entity) {
		Level previousLevel = currentLevel;

		if (currentLevel.number >= levels.Count - numberOfPreGeneratedLevels) {
			CreateLevel();
		}
		currentLevelNumber++;

		Vector3 targetPosition = currentLevel.maze.GetPositionOfCellAt(currentLevel.maze.entranceCoordinates);

		//move the player and the exit cell to the entrance of the next level
		entity.SmoothlyMoveTo (targetPosition,10);
		previousLevel.maze.exit.gameObject.GetComponentInChildren<SmoothTransitionMover> ().SmoothlyMoveTo (targetPosition, levelTransitionTime);
	}

	public void Pause () {

		paused = true;

		//blur the background
		BlurOptimized blur = mainCamera.gameObject.GetComponent<BlurOptimized>();
		blur.enabled = true;

		//enable the pause menu
		UICanvas.enabled = true;

	}

	public void UnPause () {

		paused = false;
		
		//disable the blur
		BlurOptimized blur = mainCamera.gameObject.GetComponent<BlurOptimized>();
		blur.enabled = false;
		
		//disable the pause menu
		UICanvas.enabled = false;
		
	}

	public void ExitGame () {
		Application.Quit ();
	}
	
}