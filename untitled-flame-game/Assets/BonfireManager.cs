using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;

public class BonfireManager : MonoBehaviour
{
    [SerializeField] private Sprite[] bonfireSprites;
    private Animator anim;
    [SerializeField] private GameObject lightSource;
    [SerializeField] private GameObject smokeSystem;
    public float maxBonfireValue = 40f;
    public float bonfireValue;
    public float decreaseRate = 1f;

    private bool canAddLogs = false;
    [SerializeField] private GameObject pressFCanvas;

    [Header("SFX")]
    [SerializeField] private int logClipIdxLow = 0;
    [SerializeField] private int logClipIdxHigh = 1;
    [SerializeField] private float volume = 0.5f;

    [Header("Campfire Ambiance")]
    [SerializeField] private AudioSource audioSource;


    public enum BonfireState
    {
        Lit,
        Out
    }

    public BonfireState currentState;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        pressFCanvas.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        currentState = BonfireState.Lit;
        bonfireValue = maxBonfireValue;
    }

    // Update is called once per frame
    void Update()
    {
        HandleBonfireValue();
        HandleStates();
        HandleLogAdding();
    }

    private void HandleLogAdding()
    {
        if (canAddLogs && Input.GetKeyDown(KeyCode.F))
        {
            AddLogsToFire();
        }
        if (ItemManager.Instance.GetLogCount() == 0)
        {
            pressFCanvas.SetActive(false);
        }
    }

    private void AddLogsToFire()
    {
        
        int logsToAdd = ItemManager.Instance.GetLogCount(); // Get the current log count
        if (logsToAdd > 0)
        {
            int addLogClipIdx = UnityEngine.Random.Range(logClipIdxLow, logClipIdxHigh + 1);
            SFXManager.Instance.PlaySFX(addLogClipIdx, volume);
        }
        float valueToAdd = logsToAdd * ItemManager.Instance.GetBonfireAddAmount(); // For example, each log adds 5 to bonfireValue
        AddBonfireValue(valueToAdd);
        ItemManager.Instance.ResetLogCount(); // Reset the log count
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canAddLogs = true;
            if (ItemManager.Instance.GetLogCount() != 0)
            {
                pressFCanvas.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canAddLogs = false;
            pressFCanvas.SetActive(false);
        }
    }

    public void AddBonfireValue(float num)
    {
        bonfireValue += num;
    }

    private void HandleBonfireValue()
    {
        if (bonfireValue > maxBonfireValue)
        {
            bonfireValue = maxBonfireValue;
        }
        else if (bonfireValue > 0 && (EventManager.Instance.currentEvent == EventManager.HazardEvent.WindLeft ||
            EventManager.Instance.currentEvent == EventManager.HazardEvent.WindRight))
        {
            bonfireValue -= Time.deltaTime * decreaseRate * 1.5f;
        }
        else
        {
            bonfireValue -= Time.deltaTime * decreaseRate;
        }

        if (bonfireValue <= 0)
        {
            bonfireValue = 0;
        }
    }

    private void HandleStates()
    {
        if (bonfireValue > 0)
        {
            ChangeState(BonfireState.Lit);
        } else
        {
            ChangeState(BonfireState.Out);
        }
        switch (currentState)
        {
            case BonfireState.Lit:
                anim.SetBool("isLit", true);
                lightSource.SetActive(true);
                smokeSystem.SetActive(true);
                if (!audioSource.isPlaying)
                {
                    audioSource.Pause();
                    audioSource.Play();
                }
                EnemySpawnManager.Instance.SetFireIsOut(false);
                break;
            case BonfireState.Out:
                anim.SetBool("isLit", false);
                lightSource.SetActive(false);
                //pressFCanvas.SetActive(false);
                smokeSystem.SetActive(false);
                EnemySpawnManager.Instance.SetFireIsOut(true);
                //EndGame();
                break;
        }
    }

    private void EndGame()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        AmbianceManager.Instance.FadeOutAndDestroyAll();
        TimeManager.Instance.shouldShowResults = true;
    }

    public void ChangeState(BonfireState newState)
    {
        currentState = newState;
    }
}
