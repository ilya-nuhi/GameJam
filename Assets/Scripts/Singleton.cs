using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public Transform respawnPoint;
    public static Singleton Instance { get; private set; }

    PlayerController playerController;

    public int lives = 3;

    private void Awake() 
    {   
        playerController = FindObjectOfType<PlayerController>();
        ManageSingleton();
        respawnPoint = new GameObject("RespawnPoint").transform;
        respawnPoint.position = playerController.transform.position;
        respawnPoint.rotation = playerController.transform.rotation;
        respawnPoint.localScale = playerController.transform.localScale;

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

    public void OnGameOver(){
        Destroy(gameObject);
    }

}
