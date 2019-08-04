using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private int score;
    private int prevScore;
    public static Score instance;
    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (instance == null) instance = this;   
    }

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        score = 0;
        prevScore = -1;
    }

    private void Update()
    {
        if(score != prevScore) { scoreText.text = score.ToString(); prevScore = score; }
    }

    public void IncreaseScore() { score++; }
}
