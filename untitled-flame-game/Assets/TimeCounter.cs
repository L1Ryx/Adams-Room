using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private GameObject scoreTextObj;
    private TextMeshProUGUI tmp;

    [Header("Ambiance")]
    [SerializeField] private int clipIdx = 0;

    private void Awake()
    {
        tmp = scoreTextObj.GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        AmbianceManager.Instance.PlayAmbiance(clipIdx, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        // Get the elapsed time from the TimeManager singleton
        float elapsedTime = TimeManager.Instance.GetElapsedTime();

        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime - minutes * 60);

        // Update the text field
        tmp.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
