using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    SingletonLevel singletonLevel;
    private void Awake() {
        singletonLevel = FindObjectOfType<SingletonLevel>();
    }
    public void NextScene(){
        if(singletonLevel!=null){
            singletonLevel.DestroyLevel();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void PlayAgain(){
        SceneManager.LoadScene("Level1");
    }

}
