using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindChainLink : TakeDoDamage
{
    public bool isDead = false;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    protected override void ExecuteDeathAnim()
    {
        isDead = true;
        anim.SetTrigger("isDeathChainLink");
    }
}
