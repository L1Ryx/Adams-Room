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
    public float maxBonfireValue = 40f;
    public float bonfireValue;
    public float decreaseRate = 1f;

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
        // DEBUGGING BELOW
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeState(BonfireState.Lit);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeState(BonfireState.Out);
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
        if (bonfireValue > 0)
        {
            bonfireValue -= Time.deltaTime * decreaseRate;
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
                break;
            case BonfireState.Out:
                anim.SetBool("isLit", false);
                lightSource.SetActive(false);
                break;
        }
    }

    public void ChangeState(BonfireState newState)
    {
        currentState = newState;
    }
}
