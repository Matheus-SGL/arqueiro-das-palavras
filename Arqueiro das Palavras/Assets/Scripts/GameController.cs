using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class NewWords
{
    public string nome;
    public bool alreadyUse;
}

public class GameController : MonoBehaviour
{
    public List<NewWords> newWords = new List<NewWords>();

    public PlayerBehaviour playerBehaviour;
    public BookController bookController;
    public EnemyBehaviour enemyBehaviour;
    public SpawnController spawnController;

    public GameObject playerBody;
    public GameObject pauseMenuUI;
    public GameObject gamePlayUI;
    public GameObject gameOverUI;
    public bool isPaused;

    public Text scoreText1;
    public int score;
    public int playerGold;

    public Vector3 cpPosition;
    private int cpScore;
    private int cpPlayerGold;
    public int cpPlayerLife;
    public int cpArrows;

    public int checkpointNumero = 0;

    public int piecesOfWord;

    // Use this for initialization
    void Start()
    {
        playerBehaviour = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
        enemyBehaviour = GameObject.FindWithTag("Enemy").GetComponent<EnemyBehaviour>();
        bookController = bookController.GetComponent<BookController>();
        spawnController = spawnController.GetComponent<SpawnController>();
        Physics2D.IgnoreLayerCollision(11, 11, true);
        Physics2D.IgnoreLayerCollision(9, 13, true);
    }

    // Update is called once per frame
    void Update()
    {
        playerGold = playerBehaviour.playerGold;
        scoreText1.text = score.ToString();

        if (score >= 1000)
        {
            //playerBehaviour.cpPlayerLife++;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused && !playerBehaviour.gamePause)
            {
                PauseMenu();
            }
            else if (isPaused && playerBehaviour.gamePause)
            {
                ResumeGame();
            }
        }

        if (piecesOfWord == 1)
        {
            GameData.hasNewWord = true;
            int randomNum = Random.Range(0,5);
            if (!ActiveWords.alreadyUse[randomNum])
            {
                newWords[randomNum].alreadyUse = true;
                ActiveWords.alreadyUse[randomNum] = true;
                switch(randomNum)
                {
                    case 0:
                        newWords[randomNum].nome = "simples";
                        ActiveWords.nome[randomNum] = "simples";
                        break;
                    case 1:
                        newWords[randomNum].nome = "oculto";
                        ActiveWords.nome[randomNum] = "oculto";
                        break;
                    case 2:
                        newWords[randomNum].nome = "composto";
                        ActiveWords.nome[randomNum] = "composto";
                        break;
                    case 3:
                        newWords[randomNum].nome = "indeterminado";
                        ActiveWords.nome[randomNum] = "indeterminado";
                        break;
                    case 4:
                        newWords[randomNum].nome = "inexistente";
                        ActiveWords.nome[randomNum] = "inexistente";
                        break;
                    default:
                        break;
                }
                piecesOfWord = 0;
            }
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public void PauseMenu()
    {
        playerBehaviour.pauseGame(true);
        pauseMenuUI.SetActive(true);
        gamePlayUI.SetActive(false);
        isPaused = true;
    }
    public void ResumeGame()
    {
        playerBehaviour.pauseGame(false);
        pauseMenuUI.SetActive(false);
        gamePlayUI.SetActive(true);
        isPaused = false;
    }
    public void MenuButton()
    {
        Debug.Log("Carregando Menu...");
        SceneManager.LoadScene("MentalMap");
        Time.timeScale = 1;
    }
    public void QuitButton()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }

    public void SaveCheckpoint()
    {
        cpPosition = playerBehaviour.checkpointPos;
        cpPlayerGold = playerGold;
        cpScore = score;
        cpPlayerLife = playerBehaviour.playerLife;
        cpArrows = playerBehaviour.arrowInInventory;
        spawnController.ChangeCheckpoint(checkpointNumero);
        spawnController.ChangeEnemyCheckpoint(checkpointNumero);

        piecesOfWord++;
        Debug.Log("Encontrada "+piecesOfWord+" fragmentos de palavras.");
    }

    public void LoadCheckPoint()
    {
        playerBehaviour.playerLife = cpPlayerLife;
        playerBehaviour.arrowInInventory = cpArrows;
        score = cpScore;
        playerGold = cpPlayerGold;
        playerBehaviour.playerGold = playerGold;
        playerBody.transform.position = cpPosition;
        gameOverUI.SetActive(false);
        gamePlayUI.SetActive(true);
        playerBehaviour.pauseGame(false);
        spawnController.SpawnBox();
        spawnController.SpawnEnemy();
    }
}
