using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonLevel : MonoBehaviour
{
    public static SingletonLevel Instance { get; private set; }

    private void Awake() 
    {
        ManageSingleton();

    }

    void ManageSingleton(){
    // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void DestroyLevel(){
        Destroy(gameObject);
    }
}
