using UnityEngine;
using System.Collections;

public class Step {
	//holding the information of it's own cost 

	public GameObject gameObject;
	public float moveCost;
	public Vector3 gridPos;

	
	//Step constructor which takes a gameObject and a moveCost float
	public Step(GameObject gameObject, float cost){
		this.gameObject = gameObject;
		moveCost = cost;
	}

	// step decideds how much it would cost for an object to move through it
	public Step(GameObject gameObject, float cost, Vector3 gridPos){
		this.gameObject = gameObject;
		moveCost = cost;
		this.gridPos = gridPos; 
	}


}
