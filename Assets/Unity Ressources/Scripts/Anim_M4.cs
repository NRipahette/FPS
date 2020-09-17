using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_M4 : MonoBehaviour
{
    private Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Fire 
        if(GetComponent<TirM4>().IsFiring && !GetComponent<TirM4>().IsReloading)
        {
            //Anim.SetTrigger("fire");
            Anim.SetBool("Firing", true);
        }
        else
        {
              Anim.SetBool("Firing", false); 
        }
        if (GetComponent<TirM4>().IsReloading)
        {
            Anim.SetBool("Reloading",true);
        }else
        {
            Anim.SetBool("Reloading", false);

        }

        if (GetComponent<TirM4>().IsSprinting)
        {
            Anim.SetBool("Run", true);
        }
        else if (GetComponent<TirM4>().IsWalking)
        {
            Anim.SetBool("Run", false);
            Anim.SetBool("Walk", true);
        }else
        {
            Anim.SetBool("Walk", false);
            Anim.SetBool("Run", false);
        }
            
            
            
    }
}
