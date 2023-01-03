using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenerateSpawn
{
    public string nome;
    public GameObject itemSpawn;
    public Transform spawnTransform;
    public int lastCheckPoint;
    public bool canSpawn;
    public bool destroyed;
}

[System.Serializable]
public class GenerateEnemySpawn
{
    public string nome;
    public GameObject monsterSpawn;
    public Transform spawnTransform;
    public int lastCheckPoint;
    public bool canSpawn;
    public bool died;
    public int direcao;
    public float timeToChange;
}

public class SpawnController : MonoBehaviour
{
    public List<GenerateSpawn> spawnTable = new List<GenerateSpawn>();
    public List<GenerateEnemySpawn> enemySpawnTable = new List<GenerateEnemySpawn>();
    public GameController gameController;
    public EnemyBehaviour enemyBehaviour;

    // Use this for initialization
    void Start()
    {
        gameController = gameController.GetComponent<GameController>();
        enemyBehaviour = enemyBehaviour.GetComponent<EnemyBehaviour>();
        SpawnBox();
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Instancia todos os objetos que estiverem no mesmo checkpoint e podem spawnar
    //É chamado no inicio do jogo e sempre que der restart no jogo (GameOver)
    public void SpawnEnemy()
    {
        for (int i = 0; i < enemySpawnTable.Count; i++)
        {
            if (enemySpawnTable[i].died && enemySpawnTable[i].canSpawn && enemySpawnTable[i].lastCheckPoint == gameController.checkpointNumero)
            {
                enemyBehaviour.SetValues(enemySpawnTable[i].direcao, enemySpawnTable[i].timeToChange);
                GameObject item = (GameObject)Instantiate(enemySpawnTable[i].monsterSpawn, enemySpawnTable[i].spawnTransform.position, Quaternion.identity, enemySpawnTable[i].spawnTransform);
                enemySpawnTable[i].died = false;
                item.gameObject.name = enemySpawnTable[i].nome;
            }
        }
    }

    public void SpawnBox()
    {
        for (int i = 0; i < spawnTable.Count; i++)
        {
            if (spawnTable[i].destroyed && spawnTable[i].canSpawn && spawnTable[i].lastCheckPoint == gameController.checkpointNumero)
            {
                GameObject item = (GameObject)Instantiate(spawnTable[i].itemSpawn, spawnTable[i].spawnTransform.position, Quaternion.identity, spawnTable[i].spawnTransform);
                spawnTable[i].destroyed = false;
                item.gameObject.name = spawnTable[i].nome;
            }
        }
    }

    //Determina que objeto foi destruido
    //É chamado sempre que objeto for destruido ou morto
    public void Died(string enemyName)
    {
        for (int i = 0; i < enemySpawnTable.Count; i++)
        {
            if (enemySpawnTable[i].nome == enemyName)
            {
                enemySpawnTable[i].died = true;
                break;
            }
        }
    }

    public void Destroyed(string itemName)
    {
        for (int i = 0; i < spawnTable.Count; i++)
        {
            if (spawnTable[i].nome == itemName)
            {
                spawnTable[i].destroyed = true;
                break;
            }
        }
    }

    //Altera o checkpoint que objecto está presente
    //É chamado sempre que é alterado o checkpoint
	public void ChangeEnemyCheckpoint(int checkpointNumber)
    {
        for (int i = 0; i < enemySpawnTable.Count; i++)
        {
            if (enemySpawnTable[i].canSpawn && !enemySpawnTable[i].died)
            {
                enemySpawnTable[i].lastCheckPoint = checkpointNumber;
            }
            else if (enemySpawnTable[i].died)
            {
                CannotSpawn(enemySpawnTable[i].nome);
            }
        }
    }

    public void ChangeCheckpoint(int checkpointNumber)
    {
        for (int i = 0; i < spawnTable.Count; i++)
        {
            if (spawnTable[i].canSpawn && !spawnTable[i].destroyed)
            {
                spawnTable[i].lastCheckPoint = checkpointNumber;
            }
            else if (spawnTable[i].destroyed)
            {
                CannotSpawn(spawnTable[i].nome);
            }
        }
    }

    //Determina que objeto não pode mais spawnar
    //É chamado sempre que alterar o checkpoint e objeto estiver destruido
	public void EnemyCannotSpawn(string enemyName)
    {
        for (int i = 0; i < enemySpawnTable.Count; i++)
        {
            if (enemySpawnTable[i].nome == enemyName)
            {
                enemySpawnTable[i].canSpawn = false;
                break;
            }
        }
    }

    public void CannotSpawn(string itemName)
    {
        for (int i = 0; i < spawnTable.Count; i++)
        {
            if (spawnTable[i].nome == itemName)
            {
                spawnTable[i].canSpawn = false;
                break;
            }
        }
    }
}
