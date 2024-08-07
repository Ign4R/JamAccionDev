using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenceBarrera : WeaponBehaviour, IDefence, IDamageable
{
    public Button upgradeDefence;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D coll;
    [SerializeField] private float maxHits=10;
    private int maxLevelUpgrade=10;
    private bool isAttack;
    private bool isResetting;
    private int countHits;
    private float currentCooldown;
    private float maxCooldown=10;
    private int countUpgrade;

    public bool UpgradeMax { get ; set ; }
    public bool IsActive { get; set; }

    /// representarlo con un numero
    protected override void Awake()
    {
        currentCooldown = maxCooldown;
    }

    private void Update()
    {
        if (isResetting)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown < 1)
            {
                currentCooldown = maxCooldown;
                coll.enabled = true;
                isResetting = false;
                spriteRenderer.color = Color.white;
            }
        }
    }
    public void CheckHits()
    {
        countHits++;
        if (countHits >= maxHits)
        {
            countHits = 0;
            coll.enabled = false;
            isResetting = true;
            spriteRenderer.color = Color.red;
            isAttack = false;
        }
        else
        {
            isAttack = true;
        }
    }

    public void UpgradeDefence()
    {
        countUpgrade++;
        if (countUpgrade <= 10)
        {
            currentDamage ++;
            maxHits =+ 4;
            currentCooldown--;
        }
        else
        {
            UpgradeMax = true;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isResetting)
        {
            CheckHits();   
            EnemyStats enemy = collision.gameObject.GetComponent<EnemyStats>();
            StartCoroutine(Attack(enemy));
        }
      
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.gameObject.GetComponent<EnemyStats>();
            StopCoroutine(Attack(enemy));
            isAttack = false;

        }
    }
    IEnumerator Attack(EnemyStats enemy)
    {
        while (isAttack) // Esto crea un bucle infinito
        {
            if (enemy != null)
            {
    
                AudioManager.instance.Play("Barrier");
                enemy.gameObject.GetComponent<Enemy>().animator.SetTrigger("Attack");
                //enemy.gameObject.GetComponent<Enemy>().attackClip.Play();
                enemy.TakeDamage(currentDamage);
                yield return new WaitForSeconds(currentCooldown);
            }
            else
            {
                yield break;

            }
         
           
        }

    }

    public void SetDamage(float dmg)
    {
        throw new NotImplementedException();
    }

    public void TakeDamage()
    {
        AudioManager.instance.Play("Chainsaw",true);
    }
}
