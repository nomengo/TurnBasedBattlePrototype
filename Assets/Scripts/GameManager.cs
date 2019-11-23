using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //CLASS RANDOM MONSTER
    [System.Serializable]
    public class RegionData
    {
        public string regionName;
        public int maxAmountEnemys = 4;
        public string BattleScene;
        public List<GameObject> possibleEnemys = new List<GameObject>();
    }
    public List<RegionData> Regions = new List<RegionData>();
    public int curRegion;

    //HERO
    public GameObject HeroCharacter;

    //POSİTİONS
    public Vector3 nextHeroPosition;
    public Vector3 lastHeroPosition;//BATTLE

    //SCENES
    public string sceneToLoad;
    public string lastScene;//BATTLE

    //BOOLS
    public bool isWalking = false;
    public bool canGetEncounter = false;
    public bool gotAttacked = false;

    //ENUM
    public enum GameStates
    {
        World_State,
        Town_State,
        Battle_State,
        Idle
    }

    //BATTLE
    public List<GameObject> enemysToBattle = new List<GameObject>();
    public int enemyAmount;

    public GameStates gameStates;
    private void Awake()
    {
        //check if instance exist
        if(instance == null) 
        {
            // if not , set instance to this
            instance = this;
        }
        //if it exist but is not this instance
        else if(instance != this)
        {
            //destroy it
            Destroy(gameObject);
        }
        //set this to be not destroyable
        DontDestroyOnLoad(gameObject);
        if (!GameObject.Find("HeroCharacter"))
        {
            GameObject hero = Instantiate(HeroCharacter, Vector3.zero, Quaternion.identity) as GameObject; 
            hero.name = "HeroCharacter";
        }
    }

    private void Update()
    {
        switch (gameStates)
        {
            case (GameStates.World_State):
                if (isWalking)
                {
                    RandomEncounter();
                }
                if (gotAttacked)
                {
                    gameStates = GameStates.Battle_State;
                }
            break;
            case (GameStates.Town_State):

            break;
            case (GameStates.Battle_State):
                //LOAD BATTLE SCENES
                StartBattle();
                gameStates = GameStates.Idle;
                //GO TO IDLE
            break;
            case (GameStates.Idle):

            break;
        }
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    public void LoadSceneAfterBattle()
    {
        SceneManager.LoadScene(lastScene);
    }
    void RandomEncounter()
    {
        if(isWalking && canGetEncounter)
        {
            if (Random.Range(0, 1000) < 10)
            {
                //Debug.Log("Saldırıya uğradım");
                gotAttacked = true;
            }
        }
    }
    void StartBattle()
    {
        int i;
        //AMOUNT OF ENEMYS
        enemyAmount = Random.Range(1, Regions[curRegion].maxAmountEnemys + 1);
        //WHICH ENEMYS
        for(i = 0; i < enemyAmount; i++)
        {
            enemysToBattle.Add(Regions[curRegion].possibleEnemys[Random.Range(0, Regions[curRegion].possibleEnemys.Count)]);
        }
        //HERO
        lastHeroPosition = GameObject.Find("HeroCharacter").gameObject.transform.position;
        nextHeroPosition = lastHeroPosition;
        lastScene = SceneManager.GetActiveScene().name;
        //LOAD LEVEL
        SceneManager.LoadScene("BattleScene");

        //RESET THE HERO
        isWalking = false;
        gotAttacked = false;
        canGetEncounter = false;
    }

}
