using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }

    private void Awake() 
    {
        ManageSingleton();

    }

    void ManageSingleton(){
    // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this)
        {
            Debug.Log("did work");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void OnGameOver(){
        Destroy(gameObject);
    }

}
