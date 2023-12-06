using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class MovieController : MonoBehaviour
{
    [Header("Ambiance")]
    [SerializeField] private int clipIdx = 4;
    [SerializeField] private float titleAmbianceVolume = 0.55f;

    public TextMeshProUGUI dialogueText; // Assign in inspector
    public string[] dialogues; // Add your dialogues in the inspector
    public float letterTypingSpeed = 0.05f;
    public float delayBetweenDialogues = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        AmbianceManager.Instance.PlayAmbiance(clipIdx, titleAmbianceVolume);
        StartCoroutine(DisplayDialogues());
    }

    private IEnumerator DisplayDialogues()
    {
        foreach (string dialogue in dialogues)
        {
            yield return StartCoroutine(TypeDialogue(dialogue));
            yield return new WaitForSeconds(delayBetweenDialogues);
        }

        // Transition to the next scene
        AmbianceManager.Instance.FadeOutAndDestroyAll();
        TimeManager.Instance.shouldShowCutscene = false;
        TimeManager.Instance.shouldShowResults = true;
        TimeManager.Instance.transitioningToResults = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        dialogueText.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(letterTypingSpeed);
        }
    }
}
