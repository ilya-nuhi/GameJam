using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Health health;

    void Awake() {
        health = FindObjectOfType<Health>();
    }
    void Update() {
        if(Input.GetKeyDown(KeyCode.R)){
            ResetScene();
        }    
    }
    void ResetScene()
    {
        StartCoroutine(health.Die());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
