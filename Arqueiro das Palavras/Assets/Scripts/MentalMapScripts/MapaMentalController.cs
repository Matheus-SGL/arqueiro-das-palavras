using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WordFound
{
    public string name;
    public GameObject word;
    public GameObject wordShadow;
    public bool founded;
    public bool inCorrectPos;
    public Vector3 posFinal;
}

public class MapaMentalController : MonoBehaviour
{
    public List<WordFound> wordFound = new List<WordFound>();

    // Use this for initialization
    void Start()
    {
        if (GameData.hasNewWord)
        {
            for (int i = 0; i < wordFound.Count; i++)
            {
                if (ActiveWords.nome[i] != null)
                {
                    wordFound[i].founded = true;
                    if (ActiveWords.posWord[i].x != 0)
                    {
                        wordFound[i].inCorrectPos = true;
                        wordFound[i].posFinal = ActiveWords.posWord[i];
                    }
                }
            }
            NewWordFounded();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NewWordFounded()
    {
        for (int i = 0; i < wordFound.Count; i++)
        {
            if (wordFound[i].founded)
            {
                GameObject palavra = (GameObject)Instantiate(wordFound[i].word, wordFound[i].word.transform.position, Quaternion.identity);
                //wordFound[i].word.SetActive(true);
                palavra.SetActive(true);
                wordFound[i].wordShadow.SetActive(false);

                if (wordFound[i].inCorrectPos)
                    palavra.transform.position = wordFound[i].posFinal;
            }
        }
    }

    public void SavingPiecePos(string pieceName, Vector3 pieceTransform)
    {
        for (int i = 0; i < wordFound.Count; i++)
        {
            if (wordFound[i].name == pieceName)
            {
                wordFound[i].inCorrectPos = true;
                wordFound[i].posFinal = pieceTransform;
            }
        }
        switch (pieceName)
        {
            case "simples":
                ActiveWords.posWord[0] = pieceTransform;
                break;

            case "oculto":
                ActiveWords.posWord[1] = pieceTransform;
                break;

            case "composto":
                ActiveWords.posWord[2] = pieceTransform;
                break;

            case "indeterminado":
                ActiveWords.posWord[3] = pieceTransform;
                break;

            case "inexistente":
                ActiveWords.posWord[4] = pieceTransform;
                break;
        }
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("Fase1");
    }
}
