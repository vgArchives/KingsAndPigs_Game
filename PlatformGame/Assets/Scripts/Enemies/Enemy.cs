using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D enemyRb;
    [SerializeField] protected Animator enemyAnimator;
    [SerializeField] protected BoxCollider2D boxCollider;
    [SerializeField] protected BoxCollider2D damageCollider;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected float moveVelocity;
    [SerializeField] protected float moveTimer;
    [SerializeField] protected float minMoveTime;
    [SerializeField] protected float maxMoveTime;
    [SerializeField] protected int lifePoints;
    [SerializeField] protected bool dead;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        if (IsStuck()) enemyRb.velocity = new Vector2(enemyRb.velocity.x * -1, enemyRb.velocity.y);

        LookingDirection();

        if (moveTimer <= 0)
        {
            enemyRb.velocity = new Vector2(Random.Range(-1, 2) * moveVelocity, enemyRb.velocity.y);
            moveTimer = Random.Range(minMoveTime, maxMoveTime);
        }
        else moveTimer -= Time.deltaTime;

        enemyAnimator.SetBool("Moving", enemyRb.velocity.x != 0);
    }

    public void LookingDirection()
    {
        //Ajusta a direção que o inimigo ta olhando
         if (enemyRb.velocity.x != 0)
         {
            transform.localScale = new Vector3(Mathf.Sign(enemyRb.velocity.x) * -1, 1f, 1f);
         }
    }

    public bool IsStuck()
    {
        var direction = new Vector2(Mathf.Sign(enemyRb.velocity.x), 0f);
        
        //Criando Raycast
        bool wall = Physics2D.Raycast(boxCollider.bounds.center, direction , 0.4f, layerMask);

        return wall;
    }

    public void ReceiveDamage() //Esse método está sendo chamado na animação de hit do inimigo
    {
        this.lifePoints -= 1; 
        Die();
    }

    public void Die()
    {
        if (this.lifePoints <= 0) 
        { 
            dead = true; 
            enemyRb.velocity = Vector2.zero;
            damageCollider.enabled = false;
            Destroy(gameObject, 2f);
        }      
    }

    public int GetLife()
    {
        return lifePoints;
    }
}
