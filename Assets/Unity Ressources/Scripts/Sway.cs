using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    float amount = 0.04f;
    float maxamount = 0.07f;
    public float smooth = 10;
    float amountMov = 0.04f;
    float maxamountMov = 0.07f;
    float smoothMov = 3;
    private Vector3 def;
    private bool Paused = false;

    void Start()
    {
        def = transform.localPosition;
    }

    void Update()
    {

        float factorMovementX = -(Input.GetAxis("Vertical")) * amount;
        float factorMovementY = -(Input.GetAxis("Horizontal")) * amount;
        //float factorMovementZ = -Input.GetAxis("Vertical") * amount;
        float factorMovementZ = 0 * amount;

        if (!Paused)
        {
            if (factorMovementX > maxamount)
                factorMovementX = maxamount;

            if (factorMovementX < -maxamount)
                factorMovementX = -maxamount;

            if (factorMovementY > maxamount)
                factorMovementY = maxamount;

            if (factorMovementY < -maxamount)
                factorMovementY = -maxamount;

            if (factorMovementZ > maxamount)
                factorMovementZ = maxamount;

            if (factorMovementZ < -maxamount)
                factorMovementZ = -maxamount;

            Vector3 Final = new Vector3(def.x + factorMovementX, def.y + factorMovementY, def.z + factorMovementZ);
            transform.localPosition = Vector3.Slerp(transform.localPosition, Final, (Time.time + smooth));
            Debug.Log("Movement " + transform.localRotation + " axisssss :" + Input.GetAxis("Vertical") + "fzaefzf    " + Input.GetAxis("Horizontal"));
        }


        float factorX = -(Input.GetAxis("Mouse X")) * amount;
        float factorY = -(Input.GetAxis("Mouse Y")) * amount;
        //float factorZ = -Input.GetAxis("Vertical") * amount;
        float factorZ = 0 * amount;

        if (!Paused)
        {
            if (factorX > maxamount + factorMovementX)
                factorX = maxamount + factorMovementX;

            if (factorX < -maxamount + factorMovementX)
                factorX = -maxamount + factorMovementX;

            if (factorY > maxamount + factorMovementY)
                factorY = maxamount + factorMovementY;

            if (factorY < -maxamount + factorMovementY)
                factorY = -maxamount + factorMovementY;

            if (factorZ > maxamount + factorMovementZ)
                factorZ = maxamount + factorMovementZ;

            if (factorZ < -maxamount + factorMovementZ)
                factorZ = -maxamount + factorMovementZ;

            Vector3 Final = new Vector3(def.x + factorX, def.y + factorY, def.z + factorZ);
            transform.localPosition = Vector3.Slerp(transform.localPosition, Final, (Time.time * smooth));
            Debug.Log(transform.localRotation+ " axisssss :" + Input.GetAxis("Mouse Y") + "fzaefzf    "+ Input.GetAxis("Mouse X"));
        }

        
    }
}
