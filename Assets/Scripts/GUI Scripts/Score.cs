using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI ScoreCount;

    // Start is called before the first frame update
    void Start()
    {
        ScoreCount = GetComponent<TextMeshProUGUI>();
    }

    //Function that adds 100 score if the player elimnates a small enemy
    public void smallElim()
    {
        ScoreCount.text = (Convert.ToInt32(ScoreCount.text) + 25).ToString();
    }

    //Function that adds 100 score if the player elimnates a medium enemy
    public void middleElim()
    {
        ScoreCount.text = (Convert.ToInt32(ScoreCount.text) + 50).ToString();
    }

    //Function that adds 100 score if the player elimnates a large enemy
    public void largeElim()
    {
        ScoreCount.text = (Convert.ToInt32(ScoreCount.text) + 100).ToString();
    }
    
}
