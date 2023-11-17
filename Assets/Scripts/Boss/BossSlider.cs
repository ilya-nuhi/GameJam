using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] EnemyHealth enemyHealth;
    void Start()
    {
        if(enemyHealth==null){return;}
        slider.maxValue = enemyHealth.GetHealth();
    }

    void Update()
    {
        if(enemyHealth==null){return;}
        slider.value = enemyHealth.GetHealth();
        if(slider.value <= 0){
            Destroy(this.gameObject);
        }
    }
}
