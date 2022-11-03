using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarScript : MonoBehaviour {

	public bool check = true;

	public GridScript gridScript;
	public HueristicScript hueristic;

	protected int gridWidth;
	protected int gridHeight;

	GameObject[,] pos;

	protected Vector3 start;
	protected Vector3 goal;

	public Path path;

	protected PriorityQueue<Vector3> frontier;
	protected Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
	protected Dictionary<Vector3, float> costSoFar = new Dictionary<Vector3, float>(); //A dictionary of positions and their costs
	protected Vector3 current;

	List<Vector3> visited = new List<Vector3>();

	// Use this for initialization
	protected virtual void Start () {
		InitAstar(); 
	}

	//Initializes aStar with the new Path and gives it a hueristics game object with referenced gridScript
	//default constructor
	protected virtual void InitAstar(){
		InitAstar(new Path(hueristic.gameObject.name, gridScript));
	}
	//overriden constructor
	protected virtual void InitAstar(Path path){ 
		this.path = path;
		//since grid contains information of start and goal positions on the grid, we're getting them from it
		start = gridScript.start; 
		goal = gridScript.goal;
		//same with width and height
		gridWidth = gridScript.gridWidth;
		gridHeight = gridScript.gridHeight;

		pos = gridScript.GetGrid(); //is this array of all positions in the grid

		//difference between queue and list is FIFO type, saying whichever is the next object in line get it
		//priority queue on the other hand gets the object with the highest priority
		frontier = new PriorityQueue<Vector3>(); //fronties is a priority queue of vector3
		frontier.Enqueue(start, 0); //adds start position to the queue with the priority of zero

		cameFrom.Add(start, start); //Dictionary of vector3 and this adds a start position to it
		costSoFar.Add(start, 0); //so we're adding start to the dictionary and hold it's cost, which is 0

		int exploredNodes = 0; //reset the nodes that were explored

		while(frontier.Count != 0){ //while we haven't gone through everything in frontier and we added the start in the frontier
			exploredNodes++; //adding +1 to nodes we explored 
			current = frontier.Dequeue(); //getting the object with the highest priority from the frontier queue

			visited.Add(current); //add this position to visited

			//this is a vizualizer for us to see which positions were visited before the path waas concluded
			// pos[(int)current.x, (int)current.y].transform.localScale = 
			// 	Vector3.Scale(pos[(int)current.x, (int)current.y].transform.localScale, new Vector3(.8f, .8f, .8f));
			
			//if we hit the goal we break the while loop
			if(current.Equals(goal)){
				Debug.Log("GOOOAL!");
				break;
			}
			
			//takes the current node and adds a node to the right and to the left and adds it to the frontier
			for(int x = -1; x < 2; x+=2){
				AddNodesToFrontier((int)current.x + x, (int)current.y);
			}
			//and this to above and below
			for(int y = -1; y < 2; y+=2){
				AddNodesToFrontier((int)current.x, (int)current.y + y);
			}
		}

		//while breaks only if we found the goal so we assign the goal to the current
		current = goal;

		LineRenderer line = GetComponent<LineRenderer>(); //create a line to visualize the path

		int i = 0;
		float score = 0;

		//drawing the line that shows the Princess's path by updating the cell that gets the line next
		while(!current.Equals(start)){ //when we aren't in the start
			line.positionCount++; //add positions to the line
			
			GameObject go = pos[(int)current.x, (int)current.y]; //it creates a gameobject in the goal
			path.Insert(0, go, new Vector3((int)current.x, (int)current.y)); //and inserts step there in the goal

			current = cameFrom[current]; //

			Vector3 vec = Util.clone(go.transform.position);
			vec.z = -1;

			line.SetPosition(i, vec);
			score += gridScript.GetMovementCost(go);
			i++;
		}


		path.Insert(0, pos[(int)current.x, (int)current.y]);
		path.nodeInspected = exploredNodes; //this is unclear??
		
		//Debug.Log(path.pathName + " Terrian Score: " + score);
		//Debug.Log(path.pathName + " Nodes Checked: " + exploredNodes);
		//Debug.Log(path.pathName + " Total Score: " + (score + exploredNodes));
	}

	//AStar interfaces with Heuristic here; nodes are added, and Heuristic is used to determine the priority of the new nodes
	void AddNodesToFrontier(int x, int y){
		if(x >=0 && x < gridWidth && 
		   y >=0 && y < gridHeight)
		{
			Vector3 next = new Vector3(x, y); //Vector3 with the position of the node we're adding to the frontier
			float new_cost = costSoFar[current] + gridScript.GetMovementCost(pos[x, y]); //cumulative cost of move, if we were to move to next
			if(!costSoFar.ContainsKey(next) || new_cost < costSoFar[next])
			{
				costSoFar[next] = new_cost; //updates the cost of next to the new, lower cost
				float priority = new_cost + hueristic.Hueristic(x, y, start, goal, gridScript); 

				//Heuristic sets priority of next step randomly
				frontier.Enqueue(next, priority);
				cameFrom[next] = current; //update next's 'came from' value to say we came from current, which is the 'shortcut'
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
