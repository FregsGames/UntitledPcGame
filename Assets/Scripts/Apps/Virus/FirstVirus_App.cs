using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FirstVirus_App : App
{
    private string codeToGenerate = "Lorem ipsum dolor sit amet consectetur adipiscing elit fringilla, est diam aliquet senectus rhoncus morbi suscipit fames, " +
        "facilisi dis lacinia varius sodales tempus vivamus. Rhoncus nec tincidunt rutrum sem senectus habitant risus platea nibh, neque interdum feugiat volutpat mauris" +
        " aliquet laoreet at cursus ad, ultricies himenaeos sapien non nulla iaculis ac sagittis. Maecenas tristique dis in bibendum interdum placerat himenaeos tempus, " +
        "inceptos netus nisi vehicula dapibus erat proin torquent nisl, primis congue iaculis vitae facilisis sed eleifend.Ligula fames ornare lacus mi suscipit convallis " +
        "curae nulla, platea vivamus senectus facilisis porttitor quis tempus parturient montes, iaculis litora malesuada magna volutpat nisi faucibus.Mi quis vel suspendisse " +
        "proin gravida volutpat facilisis sodales nunc, dignissim primis venenatis elementum lectus magnis per fusce, at justo commodo euismod litora natoque nibh est.Pretium " +
        "litora placerat potenti nunc venenatis lacinia mattis non integer mauris natoque dis, luctus fringilla convallis neque ridiculus iaculis lobortis gravida nostra odio.";

    [SerializeField]
    private float baseDelay = 1f;
    [SerializeField]
    private float minDelay = 0.05f;
    [SerializeField]
    private float acceleration = 0.01f;

    [SerializeField]
    private float currentDelay;

    [SerializeField]
    private TextMeshProUGUI playerText;
    [SerializeField]
    private TextMeshProUGUI virusText;
    [SerializeField]
    private Image playerProgressBar;
    [SerializeField]
    private Image virusProgressBar;
    [SerializeField]
    private RectTransform playerTextParentRect;
    [SerializeField]
    private RectTransform virusTextParentRect;

    [SerializeField]
    private AnimationCurve enemyAnimationCurve;

    [SerializeField]
    private FirstVirus_Intro intro;
    [SerializeField]
    private GameObject introductionContainer;
    [SerializeField]
    private GameObject gameContainer;

    private bool gameEnded = false;
    private bool answerMode = false;
    private string answer = "";
    private int currentQuestionIndex = 0;
    private string textBackup = "";

    [SerializeField]
    private FirstVirus_Questions virusQuestions;

    [SerializeField]
    private TMP_InputField dummyInputField;

    private EventSystem eventSystem;


    private void OnEnable()
    {
        intro.OnIntroEnded += StartMiniGame;
        intro.OnDefeatDialogueEnded += CloseApp;
        intro.OnWinDialogueEnded += CloseApp;
        gameEnded = false;
        gameContainer.SetActive(false);
        introductionContainer.SetActive(true);
        intro.StartDialogue();

        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void OnDisable()
    {
        intro.OnIntroEnded -= StartMiniGame;
        intro.OnDefeatDialogueEnded -= CloseApp;
        intro.OnWinDialogueEnded -= CloseApp;
        dummyInputField.onValueChanged.RemoveAllListeners();
    }

    private void StartMiniGame()
    {
        gameContainer.SetActive(true);
        introductionContainer.SetActive(false);
        StartCoroutine(ShowText());
        StartCoroutine(ListenPlayer());
    }

    private void CloseApp()
    {
        Close();
    }

    private IEnumerator ListenPlayer()
    {
        int index = 0;
        string writtenPlayerText = "";
        List<float> questionFillAmounts = GetQuestionPoints();

        playerText.text = "";
        playerProgressBar.fillAmount = 0;
        currentQuestionIndex = 0;

        while (gameObject.activeSelf && !gameEnded)
        {
            if (Input.anyKeyDown & !answerMode)
            {
                writtenPlayerText += codeToGenerate[index];
                playerText.text = playerText.text + codeToGenerate[index];

                playerProgressBar.fillAmount = writtenPlayerText.Length * 1.0f / codeToGenerate.Length;
                index++;

                if (currentQuestionIndex < questionFillAmounts.Count && playerProgressBar.fillAmount >= questionFillAmounts[currentQuestionIndex])
                {
                    SetQuestion();
                }

                if (index == codeToGenerate.Length)
                {
                    yield return StartCoroutine(SetPlayerWin());
                }
            }

            if (Input.anyKeyDown && answerMode)
            {
                ProcessPlayerInputAnswer();
            }

            if (playerText.rectTransform.rect.height > playerTextParentRect.rect.height)
            {
                playerText.rectTransform.anchoredPosition = new Vector2(playerText.rectTransform.anchoredPosition.x, playerText.rectTransform.rect.height - playerTextParentRect.rect.height);
            }

            yield return 0;
        }
    }

    private void ProcessPlayerInputAnswer()
    {
        if (Input.anyKeyDown)
        {
            eventSystem.SetSelectedGameObject(dummyInputField.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            playerText.text = playerText.text + "</color>";
            playerText.text = playerText.text + "\n";
            answerMode = false;
            dummyInputField.onValueChanged.RemoveAllListeners();
            textBackup = "";
            // Feedback de respuesta correcta/incorrecta
            CheckAnswer();
        }
    }

    private void UpdateAnswer(string inputFieldText)
    {
        answer = inputFieldText;
        playerText.text = textBackup + inputFieldText;
    }

    private void SetQuestion()
    {
        answerMode = true;
        answer = "";
        dummyInputField.text = "";
        
        eventSystem.SetSelectedGameObject(dummyInputField.gameObject);
        playerText.text = playerText.text + "\n" + "<#ff4f00>" + virusQuestions.GetQuestion(currentQuestionIndex) + "</color>" + "\n";
        playerText.text = playerText.text + "<#4080ff>";
        dummyInputField.onValueChanged.AddListener(UpdateAnswer);
        textBackup = playerText.text;
    }

    private IEnumerator SetPlayerWin()
    {
        gameEnded = true;
        SoundManager.Instance.PlaySound(SoundManager.Sound.Notification);
        yield return new WaitForSeconds(2f);
        gameContainer.SetActive(false);
        introductionContainer.SetActive(true);
        intro.WinDialogue();
    }

    private void CheckAnswer()
    {
        RewardPlayer(virusQuestions.CheckAnwser(answer, currentQuestionIndex));
        currentQuestionIndex++;
    }

    private void RewardPlayer(bool reward)
    {
        //TODO
    }

    private List<float> GetQuestionPoints()
    {
        List<float> result = new List<float>();

        int questionCount = virusQuestions.QuestionCount();

        float initial = 1f / (questionCount + 1);
        result.Add(initial);

        for (int i = 0; i < questionCount; i++)
        {
            if (i == 0)
                continue;

            result.Add(initial * (i + 1));
        }
        return result;
    }

    private IEnumerator ShowText()
    {
        currentDelay = baseDelay;
        int index = 0;
        virusText.text = "";
        virusProgressBar.fillAmount = 0;

        while (index < codeToGenerate.Length && !gameEnded)
        {
            virusText.text = virusText.text + codeToGenerate[index];
            virusProgressBar.fillAmount = enemyAnimationCurve.Evaluate(virusText.text.Length * 1.0f / codeToGenerate.Length);
            index++;
            yield return new WaitForSeconds(currentDelay);
            currentDelay = Mathf.Clamp(currentDelay - acceleration, minDelay, baseDelay);

            if (virusText.rectTransform.rect.height > virusTextParentRect.rect.height)
            {
                virusText.rectTransform.anchoredPosition = new Vector2(virusText.rectTransform.anchoredPosition.x, virusText.rectTransform.rect.height - virusTextParentRect.rect.height);
            }
        }

        if (!gameEnded)
        {
            gameEnded = true;
            SoundManager.Instance.PlaySound(SoundManager.Sound.Cancel);
            yield return new WaitForSeconds(2f);
            gameContainer.SetActive(false);
            introductionContainer.SetActive(true);
            intro.DefeatDialogue();
        }
    }


}
