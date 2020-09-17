using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject BloodEffect;
    public GameObject MeatEffect;
    [SerializeField]
    public List<Enemy> EnemyList = new List<Enemy>();


    // Start is called before the first frame update
    void Start()
    {
        //Spawn(1);
        //InvokeRepeating("IsAnybodyAlive", 2.0f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!IsAnybodyAlive()) {
            Spawn(1);
        } ;*/
    }

    internal void Spawn(int v)
    {
        
        //for (int i = 0; i< v; i++)
        //{
            int rand = UnityEngine.Random.Range(-3, 3);
            GameObject newEnemy = EnemyPrefab;
            Enemy newOne = newEnemy.GetComponent<Enemy>(); 
            newOne.IDEnemy = EnemyList.Count;
            newOne.EnemyName = "Ratiah " + newOne.IDEnemy;
            newOne.CurrentHealth = 100;
            newOne.TotalHealth = 100;
            newOne.IsAlive = true;
            EnemyList.Add(newOne);
            EnemyList[newOne.IDEnemy].IsAlive = true;
            
            GameObject myEnemy = Instantiate(newEnemy, new Vector3(rand, 5, 0), Quaternion.identity, transform) as GameObject;


            // myEnemy.transform.parent = transform;


        //}
        
    }

    //IEnumerator CheckEnemiesAlive()
    //{
    //    yield return new WaitForSeconds(.1f);
    //    if(IsAnybodyAlive())
    //}

    bool IsAnybodyAlive()
    {
        if (EnemyList.Count == 0)
        {
            Spawn(1); 
            return false;
        }
        else
        {
            //for (int i = 0; i < EnemyList.Count; i++)
            //{
            //    Debug.Log(EnemyList[i].EnemyName + "  " + EnemyList[i].IsAlive);
            //    if (EnemyList[i].transform.GetComponent<Enemy>().IsAlive == true)
            //        return true;

            //}
            foreach (Enemy e in EnemyList)
            {
                Debug.Log(e.EnemyName + "  " + e.IsAlive);
                if (e.IsAlive == true)
                    return true;
            }
        }
        Spawn(1);
        return false;
        
    }
}
