using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    public LootController boxBehaviour;
    private ReportController reportController;
    private GameController gameController;
    private SpawnController spawnController;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        reportController = GameObject.FindWithTag("ReportController").GetComponent<ReportController>();
        spawnController = GameObject.FindWithTag("SpawnController").GetComponent<SpawnController>();
        reportController = reportController.GetComponent<ReportController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Mantém a ponta da flecha de acordo com sua queda
        Vector3 dir = GetComponent<Rigidbody2D>().velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    //Função de atirar
    public void Atirar(GameObject flecha, Transform flechaSpawn, Rigidbody2D Player, Vector2 direcao, float flechaSpeed)
    {
        GameObject bullet = (GameObject)Instantiate(flecha, flechaSpawn.position, flechaSpawn.rotation);
        
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), Player.GetComponent<Collider2D>());
        Physics2D.IgnoreLayerCollision(9, 11, true);

        var rgdBullet = bullet.GetComponent<Rigidbody2D>();

        rgdBullet.AddForce(new Vector2(direcao.x * flechaSpeed, 10f), ForceMode2D.Impulse);

        Destroy(bullet, 2.0f);
    }

    //Função de colisão
    void OnCollisionEnter2D(Collision2D other)
    {
        //Colisão com box
        if (other.gameObject.tag == "Box")
        {
            gameController.AddScore(100);
            boxBehaviour.CalculateLoot(other.gameObject);
            spawnController.Destroyed(other.gameObject.name.ToString());
            Destroy(gameObject);
            reportController.AddAchievement("boxes");
        }

        //Colisão com inimigo (Inimigo calcula o loot)
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }

        //Colisão com chão (BUG de ficar girando)
        if (other.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
