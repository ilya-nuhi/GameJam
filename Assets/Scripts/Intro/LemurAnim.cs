using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LemurAnim : MonoBehaviour
{   
    [SerializeField] AudioClip blinkSFX;
    [SerializeField] Animator fadeoutAnimator;
    public void TriggerEvent(){
        AudioSource.PlayClipAtPoint(blinkSFX,Camera.main.transform.position);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1);
        fadeoutAnimator.enabled = true;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

    }
}
