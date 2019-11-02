using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //HERO
    public GameObject HeroCharacter;

    //POSİTİONS
    public Vector3 nextHeroPosition;
    public Vector3 lastHeroPosition;//BATTLE

    //SCENES
    public string sceneToLoad;
    public string lastScene;//BATTLE

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
    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

}
