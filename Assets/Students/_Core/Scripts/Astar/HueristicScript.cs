using UnityEngine;
using System.Collections;

public class HueristicScript : MonoBehaviour {
		
	//You're drunk, Heuristic. Go home.
	//Keeping this around so that the princesses that spawn show some variation
	public virtual float Hueristic(int x, int y, Vector3 start, Vector3 goal, GridScript gridScript){
		return Random.Range(0, 500);
		//return 0;
	}
}
