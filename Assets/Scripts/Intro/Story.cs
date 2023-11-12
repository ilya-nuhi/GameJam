using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour
{
    [TextAreaAttribute(3,5)]
    [SerializeField] String[] Pages;
    [SerializeField] GameObject playButton;
    private bool _canTurnPage=true;
    TextMeshProUGUI storyText;
    int currentPage = 0;

    private void Awake() {
        storyText = GetComponent<TextMeshProUGUI>();
    }
    private void Start() {
        StartCoroutine(FadeTextToFullAlpha(2f, storyText));
    }
    private void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            if(_canTurnPage){
                StartCoroutine(NextPage(2f, storyText));
            }
        }
    }

    IEnumerator NextPage(float v, TextMeshProUGUI value)
    {
        _canTurnPage = false;
        StartCoroutine(FadeTextToZeroAlpha(v, value));
        yield return new WaitForSeconds(3);
        if(currentPage+1 < Pages.Length){
            currentPage++;
        }
        else{
            playButton.SetActive(true);
            yield break;
        }
        
        storyText.text = Pages[currentPage];
        StartCoroutine(FadeTextToFullAlpha(v, storyText));
        _canTurnPage = true;
    }

    public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
