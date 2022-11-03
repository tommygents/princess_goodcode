using UnityEngine;
using System.Collections;

public class GridScript : MonoBehaviour {

	public int gridWidth;
	public int gridHeight;
	public float spacing;
	
	public Material[] mats;
	public float[]   costs;


	public Vector3 start = new Vector3(0,0);
	public Vector3 goal = new Vector3(14,14);
	
	GameObject[,] gridArray;
	
	public GameObject startSprite;
	public GameObject goalSprite;

	//For Tower Defense
	private int mouseGridNumX;
	private int mouseGridNumY;
	//Finished generate grids
	public bool haveGrid = false;

    #region Grid Creation

    public virtual GameObject[,] GetGrid(){

		if(gridArray == null){

			gridArray = new GameObject[gridWidth, gridHeight];
			
			float offsetX = (gridWidth  * -spacing)/2f;
			float offsetY = (gridHeight * spacing)/2f;

			for(int x = 0; x < gridWidth; x++){
				for(int y = 0; y < gridHeight; y++){
					GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
					quad.transform.localScale = new Vector3(spacing, spacing, spacing);
					quad.transform.position = new Vector3(offsetX + x * spacing, 
					                                      offsetY - y * spacing, 0);

					quad.transform.parent = transform;
					//I can get this quad and change the sharedMaterial
					gridArray[x, y] = quad;
					
					quad.GetComponent<MeshRenderer>().sharedMaterial = GetMaterial(x, y);

					if(goal.x == x && goal.y == y){
						goalSprite.transform.position = quad.transform.position;
					}
					if(start.x == x && start.y == y){
						startSprite.transform.position = quad.transform.position;
					}
				}
			}

			haveGrid = true;

        }

		return gridArray;
	}

    //Needs a way to replace the material with mines, and then destroy the princess. I can do this by setting 

    //For tower defense
    //Get the selected grid position base on the mouse clicking position


    public virtual float GetMovementCost(GameObject go)
    {
        Material mat = go.GetComponent<MeshRenderer>().sharedMaterial;
        int i;

        for (i = 0; i < mats.Length; i++)
        {
            if (mat.name.StartsWith(mats[i].name))
            {
                break;
            }
        }

        // mines use negative costs to show up to other scripts; we could undo that here.
        
		 return costs[i];
    }

    //sets a random material.
    protected virtual Material GetMaterial(int x, int y)
    {
        return mats[Random.Range(0, mats.Length)];
    }
#endregion
    #region TD Scripts
    // makes a region a trap
	public void SetTrap(GameObject pos)
	{
		pos.GetComponent<MeshRenderer>().sharedMaterial = mats[mats.Length-1];

    }

	//sets off a trap that a princess has stepped on
	public void FireTrap(GameObject pos)
	{
        pos.GetComponent<MeshRenderer>().sharedMaterial = mats[1];
    }

	//takes a mouse position and returns the object that corresponds to it
    public GameObject GetMouseGrid(Vector3 mousePos)
	{
			float offsetX = (gridWidth * -spacing) / 2f;
			float offsetY = (gridHeight * spacing) / 2f;

			for (int x = 0; x < gridWidth; x++)
			{
				float gridRangeXMax = offsetX + x * spacing + spacing / 2;
                float gridRangeXMin = offsetX + x * spacing - spacing / 2;

				if(mousePos.x > gridRangeXMin && mousePos.x < gridRangeXMax)
				{
					mouseGridNumX = x;
                    break;
				}

            }

            for (int y = 0; y < gridHeight; y++)
            {
                float gridRangeYMax = offsetY - y * spacing + spacing / 2;
                float gridRangeYMin = offsetY - y * spacing - spacing / 2;

                if (mousePos.y > gridRangeYMin && mousePos.y < gridRangeYMax)
                {
                    mouseGridNumY = y;
                    break;
                }
            }

			return gridArray[mouseGridNumX, mouseGridNumY];
	}

	//returns an array of the 3x3 square around a grid square that was clicked on
	public GameObject[,] GetMouseSurroundingGrid(Vector3 mousePos)
	{
        //repeats GetMouseGrid but preserves the information to get other squares around the center
        #region GetMouseGrid code
        float offsetX = (gridWidth * -spacing) / 2f;
        float offsetY = (gridHeight * spacing) / 2f;

        for (int x = 0; x < gridWidth; x++)
        {
            float gridRangeXMax = offsetX + x * spacing + spacing / 2;
            float gridRangeXMin = offsetX + x * spacing - spacing / 2;

            if (mousePos.x > gridRangeXMin && mousePos.x < gridRangeXMax)
            {
                mouseGridNumX = x;
                break;
            }

        }

        for (int y = 0; y < gridHeight; y++)
        {
            float gridRangeYMax = offsetY - y * spacing + spacing / 2;
            float gridRangeYMin = offsetY - y * spacing - spacing / 2;

            if (mousePos.y > gridRangeYMin && mousePos.y < gridRangeYMax)
            {
                mouseGridNumY = y;
                break;
            }
        }
        #endregion

        GameObject center = gridArray[mouseGridNumX, mouseGridNumY];

        GameObject[,] surroundingGrid = new GameObject[3,3];

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                //make sure we're in-bounds, otherwise return null
                if (mouseGridNumX + x >= 0 && mouseGridNumY + y >= 0 && mouseGridNumX + x < gridWidth && mouseGridNumY + y < gridHeight)
                {
                    surroundingGrid[x + 1, y + 1] = gridArray[mouseGridNumX + x, mouseGridNumY + y];
                }
                else surroundingGrid[x + 1, y + 1] = null;


            }
        }

		return surroundingGrid;
    }

	#endregion



}
