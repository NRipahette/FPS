using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ray : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.forward * 10, Color.black);

        RaycastHit Hit;

        if (Physics.Raycast(transform.position,Vector3.forward , out Hit))
        {
           Debug.Log("OH CA TOUCHE " + Hit.transform.name + "BIM DANS TA MERE à une distance de " + Hit.distance + " " + Hit.GetType());

        }
    }
}
