using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    public GameController gameController;
    public BookController bookController;
    public LootController lootController;
    public ScrollController scrollController;
    public EnemyBehaviour enemyBehaviour;
    public ReportController reportController;
    public SpawnController spawnController;

    public bool gamePause;
    private bool btnBookEnter;

    public RawImage heart1;
    public RawImage heart2;
    public RawImage heart3;
    public RawImage heart4;
    public int playerLife;

    //movimentação vars
    public float moveSpeed;
    public Rigidbody2D playerBody;
    public GameObject playerGraphics;
    public Animator playerAnim;
    public PlatformEffector2D effector;
    public PhysicsMaterial2D wallPhysics;
    private Collider2D playerCollider;

    //jump vars
    public float jumpSpeed;
    private bool isGrounded;
    public Transform foot;
    public float groundRadius;
    public LayerMask whatIsGround;

    //shooting vars
    private Vector3 positionRight;
    private Vector3 positionLeft;
    public GameObject flechaPrefab;
    public Transform flechaSpawn;
    public ArrowBehaviour arrow;
    public float flechaSpeed;
    private Vector2 direcao = new Vector2(1, 0);
    public float fireRate;
    private float nextFire;

    //score vars
    public int playerGold;
    public int startArrowAmount;
    public int arrowInInventory;
    public Text arrowQuantity;
    public Text coinsQuantity2;

    //scroll vars
    public RawImage paperImage;
    public GameObject gamePlayUI;
    public GameObject scrollFoundUI;
    public GameObject scrollFinalFoundUI;
    private int questionNumber = 0;

    public GameObject gameOverUI;

    public Vector3 checkpointPos;

    // Use this for initialization
    void Start()
    {
        spawnController = GameObject.FindWithTag("SpawnController").GetComponent<SpawnController>();
        playerCollider = GetComponent<Collider2D>();
        positionRight = playerGraphics.transform.localScale;
        positionLeft = positionRight;
        positionLeft.x *= -1;
        arrowInInventory = startArrowAmount;
        checkpointPos = this.transform.position;
        gameController.cpPosition = checkpointPos;
        gameController.cpPlayerLife = playerLife;
        gameController.cpArrows = arrowInInventory;
        //lootController = lootControllerGO.GetComponent<LootController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movimentação esquerda - direita
        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        SpriteRenderer playerMash = GetComponent<SpriteRenderer>();

        if (movementVector != Vector2.zero && !gamePause)
        {
            direcao = movementVector;

            playerBody.velocity = new Vector2(movementVector.x * moveSpeed, playerBody.velocity.y);

            if (movementVector.x < 0)
                playerMash.flipX = true;
            else
                playerMash.flipX = false;

        }

        if (!gamePause && movementVector != Vector2.zero)
        {
            playerAnim.SetBool("isRunning", true);
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) ||
        Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            playerBody.velocity = new Vector2(0f, playerBody.velocity.y);
            playerAnim.SetBool("isRunning", false);
        }

        //Movimentação pulo
        isGrounded = Physics2D.OverlapCircle(foot.position, groundRadius, whatIsGround);
        //isGrounded = GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"));
        playerAnim.SetBool("isGrounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            effector.rotationalOffset = 0f;
            if (isGrounded && !gamePause)
            {
                playerBody.AddForce(new Vector2(0, jumpSpeed));
            }
        }
        if (isGrounded)
        {
            wallPhysics.friction = 0.4f;
            playerCollider.enabled = false;
            playerCollider.enabled = true;
        }
        else if (!isGrounded)
        {
            wallPhysics.friction = 0f;
            playerCollider.enabled = false;
            playerCollider.enabled = true;
        }

        //Movimentação para baixo da plataforma
        if (isGrounded && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            effector.rotationalOffset = 180f;
            Invoke("RotationEffector", 0.3f);
        }

        //Atirando flechas
        if (direcao.x > 0)
            playerGraphics.transform.localScale = positionRight;
        if (direcao.x < 0)
            playerGraphics.transform.localScale = positionLeft;

        if (canShoot() && !btnBookEnter && Input.GetKeyDown(KeyCode.Mouse0) && Time.time > nextFire && !gamePause)
        {
            nextFire = Time.time + fireRate;

            arrowInInventory--;

            playerAnim.SetBool("canAttack", true);

            if (!isGrounded)
            {
                StartCoroutine(Shoot(0.29f));
            }
            if (isGrounded && movementVector != Vector2.zero)
            {
                StartCoroutine(Shoot(0.33f));
            }
            if (isGrounded && movementVector == Vector2.zero)
            {
                StartCoroutine(Shoot(0.28f));
            }
            reportController.AddAchievement("shootedTimes");
        }

        //UI interface
        coinsQuantity2.text = playerGold.ToString();
        arrowQuantity.text = "x " + arrowInInventory.ToString();

        if (playerLife >= 4)
        {
            heart4.gameObject.SetActive(true);
            heart3.gameObject.SetActive(true);
            heart2.gameObject.SetActive(true);
            heart1.gameObject.SetActive(true);
            playerLife = 4;
        }
        if (playerLife == 3)
        {
            heart4.gameObject.SetActive(false);
            heart3.gameObject.SetActive(true);
            heart2.gameObject.SetActive(true);
            heart1.gameObject.SetActive(true);
        }
        if (playerLife == 2)
        {
            heart4.gameObject.SetActive(false);
            heart3.gameObject.SetActive(false);
            heart2.gameObject.SetActive(true);
            heart1.gameObject.SetActive(true);
        }
        if (playerLife == 1)
        {
            heart4.gameObject.SetActive(false);
            heart3.gameObject.SetActive(false);
            heart2.gameObject.SetActive(false);
            heart1.gameObject.SetActive(true);
        }
        if (playerLife <= 0)
        {
            heart4.gameObject.SetActive(false);
            heart3.gameObject.SetActive(false);
            heart2.gameObject.SetActive(false);
            heart1.gameObject.SetActive(false);
            playerAnim.SetBool("ifDied", true);
            playerLife = 0;
        }
        if (playerLife > 0)
            playerAnim.SetBool("ifDied", false);

        if (this.transform.position.y <= -8.5f)
        {
            playerLife = 0;
        }
    }

    public void RotationEffector()
    {
        effector.rotationalOffset = 0f;
    }

    public IEnumerator Shoot(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        arrow.Atirar(flechaPrefab, flechaSpawn, playerBody, direcao, flechaSpeed);
    }
    public void cancelAtkAnim()
    {
        playerAnim.SetBool("canAttack", false);
    }

    public void pauseGame(bool isPaused)
    {

        if (isPaused)
        {
            gamePause = true;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            gamePause = false;
            btnBookEnter = false;
        }
    }

    public bool canShoot()
    {
        return arrowInInventory > 0;
    }

    public void bookEnter(bool stop)
    {
        btnBookEnter = stop;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Colisão com flecha dropada
        if (other.gameObject.tag == "arrowDrop")
        {
            gameController.AddScore(25);
            arrowInInventory++;
            reportController.AddAchievement("collectedArrows");
            Destroy(other.gameObject);
        }

        //Colisão com dinheiro dropado
        if (other.gameObject.tag == "coin")
        {
            playerGold += 10;
            gameController.AddScore(50);
            reportController.AddAchievement("collectedCoins");
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Chest")
        {
            gameController.checkpointNumero++;
            gameController.AddScore(250);
            checkpointPos = transform.position;
            gameController.SaveCheckpoint();
            lootController.CalculateLoot(other.gameObject);
            reportController.AddAchievement("chests");
            pauseGame(true);
            gamePlayUI.SetActive(false);
            questionNumber++;
            scrollController.setScrollQuestion(questionNumber);
            if (questionNumber == 4)
            {
                scrollFinalFoundUI.SetActive(true);
            }
            else
                scrollFoundUI.SetActive(true);
        }

        if (other.gameObject.tag == "Enemy")
        {
            Collider2D collider = other.collider;
            Vector3 contactPoint = other.contacts[0].point;
            Vector3 center = collider.bounds.center;

            bool right = contactPoint.x < center.x;
            bool top = contactPoint.y > center.y + 0.4f;

            if (top)
            {
                playerBody.AddForce(new Vector2(-500, 750));
                gameController.AddScore(250);
                lootController.CalculateLoot(other.gameObject);
                reportController.AddAchievement("enemiesKilled");
                spawnController.Died(other.gameObject.name.ToString());
            }
            else if (!top)
            {
                StartCoroutine(Invencible(right));
            }
        }

        if (other.gameObject.tag == "Platform")
        {
            effector = other.gameObject.GetComponent<PlatformEffector2D>();
        }
    }

    public IEnumerator Invencible(bool right)
    {
        Physics2D.IgnoreLayerCollision(14, 12, true);
        Debug.Log("invencivel");
        playerLife--;
        if (right)
            playerBody.AddForce(new Vector2(-500, 750), ForceMode2D.Force);

        if (!right)
            playerBody.AddForce(new Vector2(500, 750), ForceMode2D.Force);

        yield return new WaitForSeconds(1.5f);
        Physics2D.IgnoreLayerCollision(14, 12, false);
        Debug.Log("não invencivel");
    }

    public void PlayerDie()
    {
        pauseGame(true);
        gameOverUI.SetActive(true);
        gamePlayUI.SetActive(false);
    }
}
