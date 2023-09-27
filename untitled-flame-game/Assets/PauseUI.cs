using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private string sceneToLoad = "StartScene";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandlePauseLogic();
        HandleKeyPausing();
    }

    private void HandleKeyPausing()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayPauseUISFX();
            if (TimeManager.Instance.IsPaused())
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void HandlePauseLogic()
    {
        if (TimeManager.Instance.IsPaused())
        {
            pausePanel.SetActive(true);
        }
        else
        {
            pausePanel.SetActive(false);
        }
    }

    public void PauseGame()
    {
        TimeManager.Instance.PauseTimer();
    }

    public void PlayPauseUISFX()
    {
        SFXManager.Instance.PlaySFX(SFXManager.Instance.uiClickIdx, SFXManager.Instance.uiClickVolume);
    }

    public void ResumeGame()
    {
        TimeManager.Instance.ResumeTimer();
    }

    public void ExitLevel()
    {
        TimeManager.Instance.ResumeTimer();
        AmbianceManager.Instance.FadeOutAndDestroyAll();
        FadeManager.Instance.LoadSceneWithFade(sceneToLoad, false);
        
    }
}
