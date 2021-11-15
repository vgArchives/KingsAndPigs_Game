using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    private BoxCollider2D playerBoxCollider;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemiesLayerMask;

    [SerializeField] private float moveVelocity;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private int jumpCount;
    [SerializeField] private int lifePoints;
    [SerializeField] private bool lockPlayer = false;
    [SerializeField] private float resetDmgTimer = 0;
    [SerializeField] private float resetAtkTimer = 0;
    private float resetDmg = 2f;
    private float resetAtk = 0.4f;

    [SerializeField] private DoorController door;
    [SerializeField] private PauseController pauseCanvas;

    [SerializeField] private Transform cameraPosition;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip lifeSound;
    [SerializeField] private AudioClip deathSound;


    // Start is called before the first frame update
    void Start()
    {
       pauseCanvas = FindObjectOfType<PauseController>();
       gameManager = FindObjectOfType<GameManager>();
       playerRb = GetComponent<Rigidbody2D>();
       playerAnimator = GetComponent<Animator>();
       playerBoxCollider = GetComponent<BoxCollider2D>();

       lifePoints = gameManager.GetPlayerLifePoints(); 

      
    }

    // Update is called once per frame
    void Update()
    {
       if(!lockPlayer && !pauseCanvas.paused)
        {
            Move();
            Jump();
            Attack();
            Immunity();
            BackSwing();
            OpenDoor();
        }
    }

    private void FixedUpdate() //Usar quando estiver trabalhando com física
    {
        playerAnimator.SetBool("OnFloor", IsGrounded());        
    }

    private void Attack()
    {
        if (resetAtkTimer <= 0 && Input.GetButtonDown("Attack1") && IsGrounded())
        {
            playerAnimator.SetTrigger("Attack");
            PlaySound(attackSound, transform.position, 0.2f);
            resetAtkTimer = resetAtk;

            //Cria um circulo apartir de um ponto e um raio e coleta todos os objetos que colidirem com ele
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemiesLayerMask);
            foreach(Collider2D enemy in hitEnemies)
            {
                if(enemy.CompareTag("Enemy"))
                {
                    if(enemy.GetComponent<Enemy>().GetLife() > 0)
                        {
                            enemy.GetComponentInParent<Animator>().SetTrigger("Hit");
                            PlaySound(hitSound, transform.position, 1f);
                        }
                }
                else if(enemy.CompareTag("Boss"))
                {
                    if(enemy.GetComponent<KingPigController>().GetLife() > 0)
                    {
                        enemy.GetComponentInParent<Animator>().SetTrigger("Hit");
                        PlaySound(hitSound, transform.position, 1f);
                    }
                }
            }
        }            
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPos == null)
            return;
        
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private void Move()
    {
        var currentHorizontalValue = Input.GetAxis("Horizontal"); // -1 = left, 0 = stand, 1 = right
        var movingValue = new Vector2(currentHorizontalValue * moveVelocity, playerRb.velocity.y);
        playerRb.velocity = movingValue;   
        
        //Mathf.Sing retorna 1 se o valor inserido for positivo e -1 se o valor inserido for negativo
        if(currentHorizontalValue != 0) transform.localScale = new Vector3(Mathf.Sign(currentHorizontalValue), 1f, 1f);
        //Esse teste returna true se o valor de movimento for diferente de zero e true caso contrário
        //isso é usado para alterar o valor da variavel de movimento alternando entre as animações
        playerAnimator.SetBool("Moving", currentHorizontalValue != 0); 
    }

    private void Jump()
    {
        playerAnimator.SetFloat("Jump/Fall", playerRb.velocity.y);

        if (Input.GetButtonDown("Jump") && jumpCount > 0)        
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpVelocity);
            PlaySound(jumpSound, transform.position, 1f);
        }

        if (IsGrounded()) jumpCount = 1;
        else jumpCount = 0;
    }

    //Rayscast/BoxCast de colisão no chão (Raycast = linha que aponta para algo)
    private bool IsGrounded()
    {
        //Criando o Raycast
        bool floor = Physics2D.BoxCast(playerBoxCollider.bounds.center, playerBoxCollider.bounds.size, 
                                        0f, Vector2.down, 0.2f, layerMask);
        return floor;
    }

    public void ReceiveDamage(int damage)
    {
        if(resetDmgTimer <= 0)
        {
            this.lifePoints -= damage;
            gameManager.SetPlayerLifePoints(lifePoints);
            gameManager.CheckLife();

            resetDmgTimer = resetDmg;

            playerAnimator.SetTrigger("Hit");
            playerAnimator.SetInteger("Life", lifePoints);

            PlaySound(hitSound, transform.position, 1f);
        }                 
    }

    public void LockPlayer(bool condition)
    {
        playerRb.velocity = new Vector2(0f, playerRb.velocity.y);
        playerAnimator.SetBool("Moving", false);
        lockPlayer = condition;
    }

    public void Die() //Esse método está sendo chamado no início da animação de morte do player
    {
        LockPlayer(true);
        PlaySound(deathSound, transform.position, 1f);
    }

    private void Immunity()
    {
        if (resetDmgTimer > 0) resetDmgTimer -= Time.deltaTime;
    }

    private void BackSwing()
    {
        if (resetAtkTimer > 0) resetAtkTimer -= Time.deltaTime;
    }

    private void EnterDoor()
    {
        playerAnimator.SetTrigger("Entering");
    }

    private void OpenDoor()
    {
        if (door && door.HaveScene() && Input.GetKeyDown(KeyCode.W))
        {
            door.Open();
            Invoke("EnterDoor", 1f);
            LockPlayer(true);
        }                
    }

    private void GameOver() //Esse método é chamado no fim da animação de morte do player
    {
        gameManager.StartNewGame();
    }

    public void GainLife()
    {
        if (lifePoints < 3)
        {
            lifePoints++;
            gameManager.SetPlayerLifePoints(lifePoints);
            gameManager.CheckLife();
        }
        else
        {
            gameManager.GainScore(50);
        }
    }

    private void PlaySound(AudioClip sound, Vector3 position, float volume)
    {
        AudioSource.PlayClipAtPoint(sound, position, volume);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {              
        if (collision.CompareTag("Enemy"))
        {
            if (transform.position.y > collision.transform.position.y)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, moveVelocity);
                collision.GetComponentInParent<Animator>().SetTrigger("Hit");
                PlaySound(hitSound, transform.position, 1f);
            }
            else ReceiveDamage(1);
        }

        if (collision.CompareTag("Boss"))
        {
            playerAnimator.SetTrigger("Hit");
            ReceiveDamage(1);
            if (transform.position.y > collision.transform.position.y)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, moveVelocity);
            }
        }

        if (collision.CompareTag("Door"))
        {
            door = collision.GetComponent<DoorController>();           
        }

        if(collision.CompareTag("Diamonds"))
        {
            gameManager.GainScore(1);
            Destroy(collision.gameObject);
            PlaySound(coinSound, transform.position, 0.2f);
        }

        if(collision.CompareTag("Heart"))
        {
            GainLife();
            Destroy(collision.gameObject);
            PlaySound(lifeSound, transform.position, 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            door = null;
        }
    }
}
