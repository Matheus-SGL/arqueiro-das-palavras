using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    public PlayerBehaviour playerBehaviour;
    public EnemyBehaviour enemyBehaviour;
    public ReportController reportController;

    public Text scrollAnswer;
    public GameObject scrollUI;
    public GameObject gameplayUI;
    public GameObject scrollFinal;
    public GameObject scrollCorrect;
    public GameObject scrollAlmost;
    public GameObject scrollIncorrect;
    public InputField answerInput;
    public Button sealButton;

    public RawImage scrollQuestion;
    public Texture2D texQuestion;

    public string[] correctAnswer;
    private int questNumber = 1;
    private int quantityCorrectAnswer;

    // Use this for initialization
    void Start()
    {
        playerBehaviour = playerBehaviour.GetComponent<PlayerBehaviour>();
        enemyBehaviour = enemyBehaviour.GetComponent<EnemyBehaviour>();
        if (questNumber == 1)
        {
            correctAnswer[0] = "simples";
            correctAnswer[1] = "oculto";
            quantityCorrectAnswer = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && sealButton.IsInteractable())
        {
            scrollButton_OnClick();
        }

        if (questNumber == 2)
        {
            correctAnswer[0] = "simples";
            correctAnswer[1] = "simples";
            quantityCorrectAnswer = 2;
        }
        if (questNumber == 3)
        {
            correctAnswer[0] = "composto";
            correctAnswer[1] = null;
            quantityCorrectAnswer = 1;
        }

    }

    public void scrollButton_OnClick()
    {
        if (scrollAnswer.text != null)
        {
            sealButton.interactable = false;
            string resposta = scrollAnswer.text.ToLower();
            string[] palavrasResp = resposta.Split(' ');
            int correctAnswerRate = 0;
            int k = 0;

            for (int i = 0; i < palavrasResp.Length; i++)
            {
                if (palavrasResp[i] == correctAnswer[k])
                {
                    k++;
                    correctAnswerRate++;
                }
                if (k > correctAnswer.Length)
                {
                    k = correctAnswerRate;
                }
            }

            if (correctAnswerRate >= 1 && correctAnswerRate < quantityCorrectAnswer)
            {
                //Resposta quase correta
                scrollUI.SetActive(false);
                scrollAlmost.SetActive(true);
                reportController.AddAchievement("silverMedals");
            }
            if (correctAnswerRate == quantityCorrectAnswer)
            {
                //Resposta correta"
                scrollUI.SetActive(false);
                scrollCorrect.SetActive(true);
                reportController.AddAchievement("goldMedals");
            }
            if (correctAnswerRate == 0)
            {
                //Resposta errada"
                scrollUI.SetActive(false);
                scrollIncorrect.SetActive(true);
                reportController.AddAchievement("wrongMedals");
            }
        }

        answerInput.text = null;
    }

    public void setScrollQuestion(int questionNumber)
    {
        if (questionNumber == 1)
        {
            texQuestion = Resources.Load("Scrolls/Fase1/scrollquestion1") as Texture2D;
        }
        if (questionNumber == 2)
        {
            texQuestion = Resources.Load("Scrolls/Fase1/scrollquestion2") as Texture2D;
        }
        if (questionNumber == 3)
        {
            texQuestion = Resources.Load("Scrolls/Fase1/scrollquestion3") as Texture2D;
        }
        scrollQuestion.texture = texQuestion;
        questNumber = questionNumber;
    }

    public void scroll_AnswerBackBtn()
    {
        scrollIncorrect.SetActive(false);
        if (questNumber != 4)
        {
            scrollAlmost.SetActive(false);
            scrollUI.SetActive(true);
            answerInput.Select();
            sealButton.interactable = true;
        }
        else
        {
            scrollFinal.SetActive(true);
        }
    }
    public void scroll_GameBackBtn()
    {
        scrollCorrect.SetActive(false);
        scrollAlmost.SetActive(false);
        gameplayUI.SetActive(true);
        playerBehaviour.pauseGame(false);
    }
}
