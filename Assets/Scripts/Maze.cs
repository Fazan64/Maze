using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	public IntVector2 size;
	public IntVector2 exitCoordinates;
	public IntVector2 entranceCoordinates;
	public Color regularCellColor;

	public MazeCell cellPrefab;
	public MazeCell exitPrefab;
	public MazeCell entrancePrefab;
	public MazePassage passagePrefab;
	public MazeWall wallPrefab;

	private float generationStepDelay;

	private MazeCell[,] cells;



	public float GenerationTime {
		get {
			return generationStepDelay * (float)(size.x * size.y);
		}
		set {
			generationStepDelay = value / (float)(size.x * size.y);
		}

	}

	public float GenerationStepDelay {
		get {
			return generationStepDelay;
		}
		set {
			generationStepDelay = value;
		}
		
	}


	private bool finishedGenerating = false;
	public bool FinishedGenerating {
		get {
			return finishedGenerating;
		}
		set {
		}
	}

	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.y));
		}
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.y >= 0 && coordinate.y < size.y;
	}

	public MazeCell GetCell (IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.y];
	}

	//Use this to get position of a cell which gameobject doesn't exist yet
	public Vector3 GetPositionOfCellAt ( IntVector2 coordinates ) {
		return transform.position + new Vector3(
			coordinates.x * cellPrefab.size.x - size.x * 0.5f * cellPrefab.size.x + 0.5f * cellPrefab.size.x, 
			coordinates.y * cellPrefab.size.y - size.y * 0.5f * cellPrefab.size.y + 0.5f * cellPrefab.size.y,
			0f
		);
	}

	public MazeExit exit{
		get {
			return GetCell(exitCoordinates) as MazeExit;
		}
		set {
		}
	}

	public MazeEntrance entrance{
		get {
			return GetCell(entranceCoordinates) as MazeEntrance;
		}
		set {
		}
	}

	

	public IEnumerator Generate () {

		finishedGenerating = false;

		WaitForSeconds delay = new WaitForSeconds (generationStepDelay);

		cells = new MazeCell[size.x, size.y];
		List<MazeCell> activeCells = new List<MazeCell> ();

		DoFirstGenerationStep (activeCells);

		//since a WaitForSeconds can't be lower than 0.01
		//some delays will be skipped if generationStepDelay
		//is below the minimum value
		float counter = (float)(generationStepDelay / 0.01);		
		while (activeCells.Count > 0) {

			counter += (float)(generationStepDelay / 0.01);

			if (generationStepDelay != 0)
				if (counter >= 1 )
					yield return delay;

			if (counter >= 1)
				counter--;

			DoNextGenerationStep(activeCells);
		}

		finishedGenerating = true;

	}

	private void DoFirstGenerationStep (List<MazeCell> activeCells) {
		activeCells.Add(CreateCell(RandomCoordinates));
	}

	private void DoNextGenerationStep (List<MazeCell> activeCells) {
		//last index
		int currentIndex = activeCells.Count - 1;

		//random
		//int currentIndex = (int)Random.Range (0f, activeCells.Count);
	
		MazeCell currentCell = activeCells[currentIndex];

		if (currentCell.IsFullyInitialized) {
			activeCells.RemoveAt(currentIndex);
			return;
		}

		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();

		if (ContainsCoordinates(coordinates)) {
			MazeCell neighbor = GetCell(coordinates);
			if (neighbor == null) {
				neighbor = CreateCell(coordinates);
				CreatePassage(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}
			else {
				CreateWall(currentCell, neighbor, direction);
			}
		}
		else {
			CreateWall(currentCell, null, direction);
		}

	}

	private MazeCell CreateCell (IntVector2 coordinates) {

		if (coordinates == exitCoordinates) 
			return CreateExit (coordinates);
		else if (coordinates == entranceCoordinates)
			return CreateEntrance (coordinates);
		else 
			return CreateRegularCell (coordinates);

	}

	private MazeCellRegular CreateRegularCell (IntVector2 coordinates) {
		
		MazeCellRegular newCell = Instantiate(cellPrefab) as MazeCellRegular;
		
		cells[coordinates.x, coordinates.y] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.y;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(
			coordinates.x * newCell.size.x - size.x * 0.5f * newCell.size.x + 0.5f * newCell.size.x, 
			coordinates.y * newCell.size.y - size.y * 0.5f * newCell.size.y  + 0.5f * newCell.size.y,
			0f
		);

		//set cell's color
//		foreach (Transform child in newCell.transform) {
//			if ( child.name == "Tile" ) {
//				SpriteRenderer tileRenderer = child.GetComponent<SpriteRenderer> ();
//				tileRenderer.color = regularCellColor;
//			}
//		}

		return newCell;
	}

	private MazeExit CreateExit (IntVector2 coordinates) {
		
		MazeExit newCell = Instantiate(exitPrefab) as MazeExit;
		
		cells[coordinates.x, coordinates.y] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Exit " + coordinates.x + ", " + coordinates.y;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(
			coordinates.x * newCell.size.x - size.x * 0.5f * newCell.size.x + 0.5f * newCell.size.x, 
			coordinates.y * newCell.size.y - size.y * 0.5f * newCell.size.y  + 0.5f * newCell.size.y,
			0f
		);
		
		return newCell;
	}

	private MazeEntrance CreateEntrance (IntVector2 coordinates) {
		
		MazeEntrance newCell = Instantiate(entrancePrefab) as MazeEntrance;
		
		cells[coordinates.x, coordinates.y] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Entrance " + coordinates.x + ", " + coordinates.y;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(
			coordinates.x * newCell.size.x - size.x * 0.5f * newCell.size.x + 0.5f * newCell.size.x, 
			coordinates.y * newCell.size.y - size.y * 0.5f * newCell.size.y  + 0.5f * newCell.size.y,
			0f
		);
		
		return newCell;
	}

	private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazeWall wall = Instantiate(wallPrefab) as MazeWall;
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			wall = Instantiate(wallPrefab) as MazeWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}
}