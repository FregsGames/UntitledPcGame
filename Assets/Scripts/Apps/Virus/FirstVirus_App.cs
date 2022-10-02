using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstVirus_App : App
{

    [SerializeField]
    private float baseDelay = 1f;
    [SerializeField]
    private float minDelay = 0.05f;
    [SerializeField]
    private float acceleration = 0.01f;

    [SerializeField]
    private int playerRewardVirusSub = 20;

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
    private bool playerAnsweredCorrectly = false;
    [SerializeField]
    private int charactersPerKey = 2;
    [SerializeField]
    private int progressBarLength = 20;
    [SerializeField]
    private float timeToProgres = 2f;

    [SerializeField]
    private FirstVirus_Questions virusQuestions;

    [SerializeField]
    private TMP_InputField dummyInputField;

    [SerializeField]
    [TextArea]
    private string codeToGenerate;

    private void OnEnable()
    {
        intro.OnIntroEnded += StartMiniGame;
        intro.OnDefeatDialogueEnded += CloseApp;
        intro.OnWinDialogueEnded += CloseApp;
        gameEnded = false;
        gameContainer.SetActive(false);
        introductionContainer.SetActive(true);
        intro.StartDialogue();
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
        StartCoroutine(ShowVirusText());
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
                for (int i = 0; i < charactersPerKey; i++)
                {
                    writtenPlayerText += codeToGenerate[index];
                    playerText.text = playerText.text + codeToGenerate[index];

                    playerProgressBar.fillAmount = writtenPlayerText.Length * 1.0f / codeToGenerate.Length;
                    index++;

                    if (currentQuestionIndex < questionFillAmounts.Count && playerProgressBar.fillAmount >= questionFillAmounts[currentQuestionIndex])
                    {
                        yield return SetQuestion();
                        break;
                    }

                    if (index == codeToGenerate.Length)
                    {
                        yield return StartCoroutine(SetPlayerWin());
                        break;
                    }
                }
            }

            if (Input.anyKeyDown && answerMode)
            {
                ProcessPlayerInputAnswer();
            }

            AdjustScroll();

            yield return 0;
        }
    }

    private void AdjustScroll()
    {
        if (playerText.rectTransform.rect.height > playerTextParentRect.rect.height)
        {
            playerText.rectTransform.anchoredPosition = new Vector2(playerText.rectTransform.anchoredPosition.x, playerText.rectTransform.rect.height - playerTextParentRect.rect.height);
        }
    }

    private void ProcessPlayerInputAnswer()
    {
        if (Input.anyKeyDown)
        {
            dummyInputField.Select();
            dummyInputField.ActivateInputField();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            playerText.text = playerText.text + "</color>";
            playerText.text = playerText.text + "\n";
            dummyInputField.onValueChanged.RemoveAllListeners();
            textBackup = "";
            StartCoroutine(CheckAnswer());
        }
    }

    private void UpdateAnswer(string inputFieldText)
    {
        answer = inputFieldText;
        playerText.text = textBackup + inputFieldText;
    }

    private IEnumerator SetQuestion()
    {
        answerMode = true;
        answer = "";
        dummyInputField.text = "";

        playerText.text = playerText.text + "\n" + "<#ff4f00>";

        foreach (var questionChar in virusQuestions.GetQuestion(currentQuestionIndex))
        {
            playerText.text = playerText.text + questionChar;
            AdjustScroll();
            yield return new WaitForSeconds(0.05f);
        }

        playerText.text = playerText.text + "</color>" + "\n";
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

    private IEnumerator CheckAnswer()
    {
        yield return StartCoroutine(LoadBar());

        bool correct = virusQuestions.CheckAnwser(answer, currentQuestionIndex);

        playerText.text = playerText.text + " " + (correct ? "<#FFFFFF>SUCCESS!</color>" : "<#E52222>ERROR!</color>") + "\n";

        RewardPlayer(virusQuestions.CheckAnwser(answer, currentQuestionIndex));
        currentQuestionIndex++;
        answerMode = false;

    }

    private IEnumerator LoadBar()
    {
        string backupText = playerText.text;

        for (int i = 0; i < progressBarLength; i++)
        {
            playerText.text = backupText + "processing... [";

            for (int j = 0; j < progressBarLength; j++)
            {
                playerText.text += (j <= i ? "#" : "_");
            }

            float percentage = ((i + 1) * 1.0f / progressBarLength) * 100;

            playerText.text += $"] {percentage.ToString("F0")}%";

            yield return new WaitForSeconds(timeToProgres / progressBarLength);
        }
    }

    private void RewardPlayer(bool reward)
    {
        if (reward)
        {
            playerAnsweredCorrectly = true;
        }
        else
        {
        }
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

    private IEnumerator ShowVirusText()
    {
        currentDelay = baseDelay;
        int index = 0;
        virusText.text = "";
        virusProgressBar.fillAmount = 0;

        while (index < codeToGenerate.Length && !gameEnded)
        {
            if (playerAnsweredCorrectly)
            {
                currentDelay = baseDelay;
                playerAnsweredCorrectly = false;
                var reduction = index < playerRewardVirusSub ? index : playerRewardVirusSub;

                virusText.text = virusText.text.Substring(0, virusText.text.Length - reduction);
                index -= reduction;
            }

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
