using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI Timer;
    float currentTime = 0f;
    float startingTime = 60;
    //CountDown Timer;
    // Start is called before the first frame update
    void Start()
    {
        Timer = GetComponent<TextMeshProUGUI>();
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Decreases by 1 every second
        currentTime -=1 * Time.deltaTime;
        Timer.text = currentTime.ToString("0");

        if(currentTime <= 0){
            currentTime = 0;
        }
    }
}
