using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogCountSetter : MonoBehaviour
{
    [SerializeField] private GameObject logTextObj;
    private TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = logTextObj.GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int currentLogCount = ItemManager.Instance.GetLogCount();
        int maxLogCount = ItemManager.Instance.maxLogs;
        tmp.text = currentLogCount + "/" + maxLogCount;
    }
}
