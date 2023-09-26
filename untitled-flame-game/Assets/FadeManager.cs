using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [SerializeField] private GameObject fadeImageObj;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 1.0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        fadeImageObj.SetActive(true);
    }

    private void Start()
    {
        FadeIn();
    }

    public void LoadSceneWithFade(string sceneName, bool withLoadingScreen = false)
    {
        StartCoroutine(FadeOutAndLoad(sceneName, withLoadingScreen));
    }

    private IEnumerator FadeOutAndLoad(string sceneName, bool withLoadingScreen)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }

        if (withLoadingScreen)
            SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        else
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }
    }
}
