using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private void Awake()
    {
        instance = this;
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    [SerializeField] Text timeSec, timeMin;
    [SerializeField] Text score;
    
    float currentTime = 0, currentMin=0;
    int scoreLines = 0;
    public int SCORELINES
    {
        get { return scoreLines; }
        set { scoreLines = value; }
    }
    private void Start()
    {
        timeSec.text = timeMin.text = "00";
        score.text = "0";
        InvokeRepeating("Time", 1, 1);
    }
    
    // Update is called once per frame
    void Update()
    {
        Score(); 
    }

    void Time()
    {
        currentTime++;
        timeSec.text=currentTime.ToString("00");
        if (currentTime==59)
        {
            currentTime = 0;
            currentMin++;
            timeMin.text = currentMin.ToString("00");

        }
    }

    void Score()
    {
        score.text = scoreLines.ToString();
    }
}
