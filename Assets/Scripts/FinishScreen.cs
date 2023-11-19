using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScreen : MonoBehaviour
{
    [SerializeField] GameObject button;
    void Start()
    {
        StartCoroutine(PlayAgain());
    }

    IEnumerator PlayAgain()
    {
        yield return new WaitForSeconds(7);
        button.SetActive(true);
    }
}
