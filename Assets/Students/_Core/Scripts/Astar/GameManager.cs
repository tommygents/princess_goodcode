using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    
    #region SingletonDeclaration 
    private static GameManager instance; 
    public static GameManager FindInstance()
    {
        return instance; //that's just a singletone as the region says
    }

    void Awake() //this happens before the game even starts and it's a part of the singletone
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else if (instance == null)
        {
            //DontDestroyOnLoad(this);
            instance = this;
        }
    }
    #endregion

    [SerializeField] GameObject princessPrefab;
    [SerializeField]List<GameObject> army;
    [SerializeField] float minPause = 1f;
    [SerializeField] float maxPause = 3f;
    [SerializeField] float waveLength;
    float waveTimer;
    bool spawning;
    bool waveFinished;
    [SerializeField] SpawnTower spawnTower;
    
    void Start()
    {
        //need to run GetGrid once to initialize the grid
        GridScript gs = GetComponentInParent<GridScript>();
        gs.GetGrid();


    }

    void StartSpawn()
    {
        
        InvokeRepeating("SpawnPrincess", 0f, Random.Range(minPause, maxPause));
        spawning = true;
    }

    void Update()
    {
        //waits for all mines to be placed to begin spawning princesses
        if (!spawning && spawnTower.allMinesPlaced) StartSpawn();

        //runs a timer once spawning begins
        if (spawning)
        {
            waveTimer += Time.deltaTime;
            //Debug.Log("Time remaining: " + (waveLength - waveTimer));
            if (waveTimer >= waveLength)
            {
                PlayerWin();
                CancelInvoke();
                spawning = false;
            }   
        }
    }

    void SpawnPrincess()
    {
        GameObject goPrincess = Instantiate(princessPrefab);
        army.Add(goPrincess);
    }

    //Destroys a specific princess
    public void DestroyPrincess(GameObject go)
    {
        if (army.Contains(go))
        {
            
            army.Remove(go);
            Destroy(go);


        }
    }

    //The player loses; this is called from FollowAStar right now
    public void PlayerLose()
    {
        
    }

    public void PlayerWin()
    {
        Debug.Log("won");
    }
}
