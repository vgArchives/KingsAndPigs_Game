using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingPigController : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D enemyRb;
    [SerializeField] protected Animator enemyAnimator;
    [SerializeField] protected BoxCollider2D boxCollider;
    [SerializeField] protected BoxCollider2D damageCollider;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected int lifePoints;
    [SerializeField] protected bool dead;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private int maxLifePoints;

    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private float distance;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerLayerMask;

    [SerializeField] private GameObject deathAnimation;
    [SerializeField] private GameObject endDoor;

    [SerializeField] private AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        healthSlider = GetComponentInChildren<Slider>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        enemyAnimator.SetInteger("Life", maxLifePoints);
        healthSlider.maxValue = maxLifePoints;
        healthSlider.value = maxLifePoints;
        lifePoints = maxLifePoints;

        target = GameObject.FindGameObjectWithTag("PlayerChar").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!dead)
        {
            Move();
        }
    }

    public void Move()
    {        
        if(Vector2.Distance(transform.position, target.position) >= distance)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), speed * Time.deltaTime);

            enemyAnimator.SetBool("Moving",true);
            enemyAnimator.SetBool("Attacking",false);
        }
        else
        {
            enemyAnimator.SetBool("Moving", false);
            Attack();
        }

        if (transform.position.x > target.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            healthBar.transform.localScale = new Vector3(1f, 1f, 1f);
        }            
        else if (transform.position.x < target.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            healthBar.transform.localScale = new Vector3(-1f, 1f, 1f);
        }            
    }

    public void Attack()
    {
        enemyAnimator.SetBool("Attacking", true);

        //Cria um circulo apartir de um ponto e um raio e coleta todos os objetos que colidirem com ele
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayerMask);
        foreach (Collider2D enemy in hitPlayer)
        {
            enemy.GetComponent<PlayerController>().ReceiveDamage(1);
        }    
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void ReceiveDamage() //Esse método está sendo chamado na animação de hit do inimigo
    {
        this.lifePoints -= 1;
        enemyAnimator.SetInteger("Life", lifePoints);
        healthSlider.value = lifePoints;
        Die();
    }

    public void Die()
    {
        if (this.lifePoints <= 0)
        {
            dead = true;
            enemyRb.velocity = Vector2.zero;
            damageCollider.enabled = false;
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }
    }

    public void EndingAnimation()
    {
        gameObject.SetActive(false);
        if (transform.position.x > target.position.x)
        {
            deathAnimation.transform.localScale = new Vector3(1f, 1f, 1f);

        }
        else if (transform.position.x < target.position.x)
        {
            deathAnimation.transform.localScale = new Vector3(-1f, 1f, 1f);            
        }

        Instantiate(deathAnimation, transform.position, Quaternion.identity);
        Invoke("CreateEndDoor", 5f);
    }

    public int GetLife()
    {
        return lifePoints;
    }

    public void CreateEndDoor()
    {
        Instantiate(endDoor, new Vector3(transform.position.x, -3.13f, 0f), Quaternion.identity);
    }
}

