using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WordList
{
    public string palavra;
    public bool alreadyUsed;
}

public class BookController : MonoBehaviour
{
    public List<WordList> wordList = new List<WordList>();

    public GameObject inventoryHUD;
    public GameObject gameplayHUD;
    public PlayerBehaviour playerBehaviour;
    public EnemyBehaviour enemyBehaviour;

    public Text wordText;
    public int wordAmount;
    private Vector3 wordPosition;

    public bool bookActivated;

    // Use this for initialization
    void Start()
    {
        wordAmount = 0;
        wordPosition = new Vector3(-244.3f, 159f, -7);
        playerBehaviour = playerBehaviour.GetComponent<PlayerBehaviour>();
        try
        {
            enemyBehaviour = GameObject.FindWithTag("Enemy").GetComponent<EnemyBehaviour>();
        }
        catch { }
    }

    // Update is called once per frame
    void Update()
    {
        if (bookActivated && Input.GetKeyDown(KeyCode.Escape))
        {
            notActive();
        }
    }

    public void isActive()
    {
        bookActivated = true;
        playerBehaviour.pauseGame(true);
        enemyBehaviour.pauseGame(true);
        gameplayHUD.SetActive(false);
    }
    public void notActive()
    {
        bookActivated = false;
        inventoryHUD.SetActive(false);
        gameplayHUD.SetActive(true);
        playerBehaviour.pauseGame(false);
        enemyBehaviour.pauseGame(false);
    }

    public void newWord()
    {
        if (wordAmount == 8)
        {
            wordPosition = new Vector3(wordPosition.x + 131.9f, wordPosition.y + 192f, wordPosition.z);
        }
        if (wordAmount <= wordList.Count)
        {
            for (int i = 0; i <= wordAmount; i++)
            {
                int randomWord = Random.Range(0, wordList.Count);
                if (wordList[randomWord].alreadyUsed == false)
                {
                    wordList[randomWord].alreadyUsed = true;
                    wordText.text = wordList[randomWord].palavra.ToString();
                    wordPosition = new Vector3(wordPosition.x, wordPosition.y - 24f, wordPosition.z);

                    Text word = Instantiate(wordText, wordPosition, Quaternion.identity) as Text;
                    word.transform.SetParent(inventoryHUD.transform, false);
                    wordAmount++;
                    break;
                }
            }
        }
    }
}
