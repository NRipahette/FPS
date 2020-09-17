using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tir : MonoBehaviour
{
    public GameObject Boulet;
    private int nboulet = 0 ;
    private int force = 500 ;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject instanceBoulet = Instantiate(Boulet, transform.position, Quaternion.identity);
            instanceBoulet.name = "Boulet " + nboulet;
            nboulet++;
            instanceBoulet.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, force), ForceMode.Force);
        }

    }
}
