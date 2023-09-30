using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreReportText : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject scoreTextObj;
    [SerializeField] private GameObject highScoreTextObj;
    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private TextMeshProUGUI highScoreTMP;
    // Start is called before the first frame update
    void Start()
    {
        scoreTMP = scoreTextObj.GetComponent<TextMeshProUGUI>();
        highScoreTMP = highScoreTextObj.GetComponent<TextMeshProUGUI>();
        ShowScoreReport();
    }

    public void ShowScoreReport()
    {
        if (TimeManager.Instance.shouldShowResults)
        {
            scoreTMP.text = "Adam lasted for " + ConvertToTime(TimeManager.Instance.score);
            scoreTextObj.SetActive(true);
        }
        else
        {
            scoreTMP.text = "";
            scoreTextObj.SetActive(false);
        }
        TimeManager.Instance.shouldShowResults = false;
        TimeManager.Instance.transitioningToResults = false;
    }

    private string ConvertToTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60; // Calculate the minutes
        int seconds = totalSeconds % 60; // Calculate the remaining seconds

        return string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    // Update is called once per frame
    void Update()
    {
        if (HighScoreManager.Instance.GetHighScore() > 0)
        {
            highScoreTMP.text = "Best: " + ConvertToTime(HighScoreManager.Instance.GetHighScore());
            highScoreTextObj.SetActive(true);
        }
        else
        {
            highScoreTextObj.SetActive(false);
        }
    }
}
