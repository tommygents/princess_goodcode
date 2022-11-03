using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path {
	//tracking all the steps in the past
	//tracking the score for the whole path based on the step cost
	//tracking the length of the path

	public string pathName; //path has a name
	public int nodeInspected;

	public GridScript gridScipt;
	public List<Step> path = new List<Step>();

	public float score;
	public int steps;

	//Path constructor
	//Path contains of the name and gridScrcipt reference
	public Path(string name, GridScript gridScipt){
		this.gridScipt = gridScipt;
		pathName = name;
	}
	
	//getting the index of the step on the path
	//it says what where this index is on the path
	public Step Get(int index){
		return path[index]; //path is a list of steps
	}
	
	//Adds a step to the path and tracks it's total score
	public virtual void Insert(int index, GameObject go){
		float stepCost = gridScipt.GetMovementCost(go);
		score += stepCost; //value of the path, the score for the path is the total cost of all the steps in the path
		
		path.Insert(index, new Step(go, stepCost)); 
		
		steps++; //adding to the count of steps
	}

	public virtual void Insert(int index, GameObject go, Vector3 gridPos){
		float stepCost = gridScipt.GetMovementCost(go);
		score += stepCost;
		
		path.Insert(index, new Step(go, stepCost, gridPos));

		steps++;
	}
}
