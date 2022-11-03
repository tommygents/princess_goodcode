using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnTower : MonoBehaviour
{
    public int maxMinesAmount;
    private int minesAmount=0;
    [SerializeField] private GameObject towerP;
    [SerializeField] private GridScript grid;
    public bool allMinesPlaced=false;

    private void Start()
    {
        towerP.transform.localScale = new Vector3(1 / grid.spacing, 1 / grid.spacing, 1 / grid.spacing);
    }

    void Update()
    {
        //if player pressed the left mouse button && grid is generated && tower amout less than maxium tower amount
        if (Input.GetMouseButtonUp(0) && grid.haveGrid && minesAmount < maxMinesAmount)
        {
            //Map the mousePosition
            Vector3 mousePosInGrid = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosInGrid.z = Camera.main.nearClipPlane + 1;
            //SpawnTowers(mousePosInGrid);
            SpawnTraps(mousePosInGrid);
        }

        //if player pressed the right button 
        if(Input.GetMouseButtonUp(1) && grid.haveGrid && minesAmount >0)
        {
            //if this grid has a tower
            //destory this tower
            Vector3 mousePosInGrid = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosInGrid.z = Camera.main.nearClipPlane + 1;
            DestoryTower(mousePosInGrid);
        }
    }

    private void SpawnTowers(Vector3 mousePos)
    {
        GameObject transformParent = grid.GetMouseGrid(mousePos);
        grid.SetTrap(transformParent);

        //if this grid do not have tower
        //spawn a tower
        if(transformParent.GetComponentInChildren<SpriteRenderer>() == null)
        {
            //grid.SetTrap(transformParent);
            //For now, we're letting SetTrap handle the visuals by changing the material
           SpawnTraps(mousePos);
            }
        else
        {
            Debug.Log("You already have a minefield in this area!");
        }

    }

    private void DestoryTower(Vector3 mousePos)
    {
        GameObject transformParent = grid.GetMouseGrid(mousePos);

        if (transformParent.GetComponentInChildren<SpriteRenderer>() != null)
        {
            Destroy(transformParent.GetComponentInChildren<SpriteRenderer>().gameObject);
            minesAmount -= 1;
        }
        else
        {
            Debug.Log("There is no mine here!");
        }
    }

    public void TriggerTrap(GameObject go)
    { 
    }

    private void SpawnTraps(Vector3 mousePos)

        //spawns mines in all the empty spots around a grid square
    {
        GameObject[,] transformParents = grid.GetMouseSurroundingGrid(mousePos);
        for (int x = 0; x < 3; x++) { 
            for(int y = 0; y  < 3; y++) {
                if (transformParents[x, y] != null && transformParents[x,y].GetComponentInChildren<SpriteRenderer>() == null && !allMinesPlaced)
                {
                    GameObject go = transformParents[x, y];
                    Instantiate(towerP, go.transform.position, go.transform.rotation, go.transform);
                    minesAmount += 1;
                    if (minesAmount == maxMinesAmount) allMinesPlaced = true;
                }
            }
                }
        
    }

    



}
