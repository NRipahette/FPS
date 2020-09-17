using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBoulet : MonoBehaviour
{

   void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Enemy")
        {
            Destroy(col.gameObject);
        }
    }
}
