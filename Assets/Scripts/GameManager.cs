using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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
    }

}
