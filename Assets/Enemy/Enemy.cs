using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int IDEnemy;
    public float CurrentHealth;
    public float TotalHealth;
    public string EnemyName;
    public bool IsAlive;

    // Start is called before the first frame update
    void Awake()
    {
        TotalHealth = 100;
        IsAlive = true;
    }

    private void Start()
    {
        CurrentHealth = TotalHealth;
        IsAlive = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CurrentHealth <= 0)
        {
            IsAlive = false;
            GetComponent<AnimationToRagdoll>().ToggleRagdoll(false);
        }

        if (!IsAlive)
        {
            
            GetComponentInParent<EnemyManager>().EnemyList[IDEnemy].IsAlive = false;
            //StartCoroutine(EnemyDead());
           // Destroy(this);
            
            
        }
    }

    private IEnumerator EnemyDead()
    {
       
        
        yield return new WaitForSeconds(1f);
       // Destroy(this.gameObject);
    }

    public void IsDamaged(int amount)
    {
        CurrentHealth -= amount;
    }



}
