using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	public int number;
	public IntVector2 mazeSize;
	
	public Maze maze;
	public TextMesh sign;

	private bool initialized = false;

	// Use this for initialization
	void Start () {
		Initialize ();
	}
	
	public bool Initialize () {

		if (!initialized) {

			initialized = true;

			gameObject.name = "Level " + number;

			//find the maze of this level
			foreach (Transform child in transform) {
				if ( child.name == "Maze" ) {
					maze = child.GetComponent<Maze>();
				}
				else if ( child.name == "Level Sign" ) {
					sign = child.GetComponent<TextMesh>();
				}
			}
			
			maze.size = mazeSize;
			
			maze.exitCoordinates = maze.RandomCoordinates;
			
			maze.entranceCoordinates = maze.RandomCoordinates;

			maze.GenerationTime = 1f;
			if (maze.GenerationStepDelay > 0.001f)
				maze.GenerationStepDelay = 0.001f;

			Debug.Log("Delay: " +  maze.GenerationStepDelay + " GenTime: " + maze.GenerationTime);

			//level /number/ /sizeX/ x /sizeY/
			sign.text = "Level " + (number + 1) + " " + maze.size.x + "\u00D7" + maze.size.y;
			//sign.fontSize = maze.size.x * 7;
			
			//maze.regularCellColor = new Color(71f/255f,71f/255f,71f/255f,1f);
			
			StartCoroutine(maze.Generate());

			return true;
		}

		return false;

	}
}
