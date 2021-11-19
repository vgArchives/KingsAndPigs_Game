using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigController : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    
    private void FixedUpdate()
    {
        if(!dead) Move();
    }
}
