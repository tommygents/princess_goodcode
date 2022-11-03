using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowAStarScript : MonoBehaviour {

	protected bool move = false;

	protected Path path;
	public AStarScript astar;
	public Step startPos;
	public Step destPos;


	protected int currentStep = 1;

	protected float lerpPer = 0;
	
	protected float startTime;
	protected float travelStartTime;
	GameManager gm;

	//gameObject is used to check for mines
	public GameObject destGo;

	// Use this for initialization
	protected virtual void Start () {
		gm = GameManager.FindInstance();
		path = astar.path;
		startPos = path.Get(0);
		destPos  = path.Get(currentStep);

		transform.position = startPos.gameObject.transform.position;

//		Debug.Log(path.nodeInspected/100f);

		Invoke("StartMove", path.nodeInspected/100f);

		startTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	protected virtual void Update () {

		if(move){
			lerpPer += Time.deltaTime/destPos.moveCost;

			transform.position = Vector3.Lerp(startPos.gameObject.transform.position, 
			                                  destPos.gameObject.transform.position, 
			                                  lerpPer);



			if(lerpPer >= 1){
				lerpPer = 0;

				currentStep++;

				if(currentStep >= path.steps){
                    //player loses if a Princess reaches the goal. Next round of dev, maybe there are lives that get lost
                    gm.PlayerLose();
                    currentStep = 0;
					move = false;
					Debug.Log(path.pathName + " got to the goal in: " + (Time.realtimeSinceStartup - startTime));
					Debug.Log(path.pathName + " travel time: " + (Time.realtimeSinceStartup - travelStartTime));
				} 

				startPos = destPos;
				destPos = path.Get(currentStep);
				destGo = destPos.gameObject;
				//Debug.Log(destPos.gridPos);

				//Writing a loop that sets off the mine, killing the Princess. Not sure that moveCost is the best way to do it.
				if (destGo.GetComponentInChildren<SpriteRenderer>() != null) {
					//the goal here is to change the material back to being not a mine
					//destPos.gameObject.GetComponent<MeshRenderer>().sharedMaterial = GetMaterial(x, y);
					Destroy(destGo.GetComponentInChildren<SpriteRenderer>().gameObject);
					gm.DestroyPrincess(astar.gameObject);
					//Destroy(this.gameObject);
				}
			}
		}
	}

	protected virtual void StartMove(){
		move = true;
		travelStartTime = Time.realtimeSinceStartup;
	}


}

