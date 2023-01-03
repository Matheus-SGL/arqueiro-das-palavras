using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    //instaces vars
    public LootController lootController;
    private ReportController reportController;
    private GameController gameController;
    private SpawnController spawnController;
    private SpriteRenderer enemyMesh;

    //enemy vars
    public int enemyLife;

    //movement vars
    public Rigidbody2D enemyBody;
    public float movementSpeed;
    public int direcao;
    public float timeToChangeDirection;
    private float tempo = 0;
    private bool gamePause;

    //animator vars
    public Animator anim;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        reportController = GameObject.FindWithTag("ReportController").GetComponent<ReportController>();
        spawnController = GameObject.FindWithTag("SpawnController").GetComponent<SpawnController>();
        enemyMesh = GetComponent<SpriteRenderer>();
        if (direcao < 0)
            enemyMesh.flipX = true;
        else
            enemyMesh.flipX = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Colisão com itens dropados
        Physics2D.IgnoreLayerCollision(12, 11, true);

        //Movimentação Patrulha por tempo
        if (timeToChangeDirection <= tempo)
        {
            direcao *= -1;
            tempo = 0;
        }

        //Movimentação esquerda - direita
        Vector2 movementVector = new Vector2(direcao, 0);
        if (gamePause)
            enemyBody.velocity = new Vector2(movementVector.x * 0, enemyBody.velocity.y);

        if (!gamePause)
        {
            tempo += Time.deltaTime;
            enemyBody.velocity = new Vector2(movementVector.x * movementSpeed, enemyBody.velocity.y);
        }

        if (movementVector.x < 0)
            enemyMesh.flipX = true;
        else
            enemyMesh.flipX = false;
    }

    public void pauseGame(bool isPaused)
    {
        if (isPaused)
            gamePause = true;

        else
            gamePause = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Colisão de dano por flechada - Flecha
        if (other.gameObject.tag == "arrow")
        {
            enemyLife -= 1;

            anim.SetBool("hitted", true);

            if (enemyLife == 0)
            {
                spawnController.Died(gameObject.name.ToString());
                gameController.AddScore(200);
                reportController.AddAchievement("enemiesKilled");
                lootController.CalculateLoot(this.gameObject);
            }
        }

        //Colisão com objetos da cena
        if (other.gameObject.tag == "Box" || other.gameObject.tag == "Ground" || other.gameObject.tag == "Chest" || other.gameObject.tag == "Enemy")
        {
            direcao *= -1;
            tempo = 0;
        }
    }

    public void CancelAnim()
    {
        anim.SetBool("hitted", false);
    }

    public void SetValues(int direction, float timeToChange)
    {
        direcao = direction;
        timeToChangeDirection = timeToChange;
    }
}
