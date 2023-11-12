using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    [SerializeField] GameObject storyTextGO;
    public void TriggerStory(){
        storyTextGO.SetActive(true);
    }
}
