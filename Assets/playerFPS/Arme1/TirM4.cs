using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TirM4 : MonoBehaviour
{
    [SerializeField]
    LayerMask LayerMask;

    EnemyManager EnemyManager;
    UnityEditor.Animations.AnimatorController controller;
    public AudioClip FireSound;
    public AudioClip EmptySound;
    public AudioClip ReloadSound;
    public GameObject BulletHole;
    public GameObject effect;
    public GameObject HitEffect;
    public float RPM = 600;
    public int force = 500;
    public int Bullets;
    public int SprayLenght;
    public int Magazines;
    public int BulletsPerMag;
    private float NextFire;
    private Ray ray;
    private RaycastHit hit;
    private Text CanvasText;

    public bool IsReloading = false;
    public bool IsSprinting = false;
    public bool IsWalking = false;
    public bool IsFiring = false;



    // Start is called before the first frame update

    Coroutine reload;
    void Start()
    {
        EnemyManager = FindObjectOfType<EnemyManager>();
        CanvasText = GameObject.Find("CanvasPlayer").GetComponentInChildren<Text>();


    }
    void FixedUpdate()
    {
        CanvasText.text = Bullets + " / " + BulletsPerMag + "  " + Magazines + "Mags";



    }


    // Update is called once per frame
    void Update()
    {
        if (RPM > 0)
        {
            //ça tire
            if (Input.GetButton("Fire1") && !IsReloading)
            {

                IsFiring = true;
                if (Time.time > NextFire)
                {
                    NextFire = Time.time + (1 / (RPM / 60));

                    if (RPM > 0 && Bullets != 0)
                    {
                        Shoot();
                    }
                    else if (RPM > 0 && Bullets == 0)
                    {
                        {
                            NextFire = Time.time + (1 / (RPM / 60));
                            GetComponent<AudioSource>().PlayOneShot(EmptySound);
                        }
                    }
                }


            }
            else
            {
                IsFiring = false;
                SprayLenght = 0;
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && !IsReloading)
            {
                IsFiring = true;
                if (Bullets != 0)
                {
                    ShootOnce();
                }
                else if (Bullets == 0)
                {
                    {
                        GetComponent<AudioSource>().PlayOneShot(EmptySound);
                    }
                }
            }
            else
            {
                IsFiring = false;
            }
        }


        //ça avance avec shift
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") != 0)
        {
            // on stop le reload
            if (IsReloading)
            {
                StopCoroutine("reload"); // stop reload
                GetComponent<AudioSource>().Stop(); // stop sound
                IsReloading = false;
            }

            // on marche plus
            IsWalking = false;
            //on dit qu'on sprint
            IsSprinting = true;

        }
        else if (Input.GetAxis("Vertical") != 0) //sans shift
        {
            //on marhce et on sprinte pas et on annule pas le reload
            IsWalking = true;
            IsSprinting = false;
        }
        else
        {
            // on bouge pas
            IsWalking = false;
            IsSprinting = false;
        }

        if (Input.GetKeyDown(KeyCode.R) && !IsReloading && Magazines > 0 && Bullets != BulletsPerMag && !IsSprinting)
        {
            IsReloading = true;
            // create an IEnumerator object
            reload = StartCoroutine(Reload()); // start the coroutine

        }

        //if (GetComponent<ArmeManager>().IsSwitching)
        //{
        //    if (IsReloading)
        //    {
        //        IsReloading = false;
        //        StopCoroutine("reload"); // stop reload
        //        GetComponent<AudioSource>().Stop(); // stop sound

        //    }
        //}
    }



    void ShootOnce()
    {

        Bullets--;
        Vector2 CenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        ray = Camera.main.ScreenPointToRay(CenterPoint + new Vector2(0, 1.75f) * SprayLenght);

        GetComponent<AudioSource>().PlayOneShot(FireSound);


        /*var tir = Instantiate(effect, ray.origin, Quaternion.Euler(ray.direction.x, ray.direction.y, ray.direction.z));
        Debug.Log(ray.direction.x + ray.direction.y + ray.direction.z + "wow");
        Destroy(tir, 5);*/

        RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, 100f, LayerMask);
        Debug.DrawRay(ray.origin, ray.direction, Color.green, 1f);

        //  if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane,LayerMask))
        //  {
        var direction = transform.forward;

        foreach (RaycastHit rh in hits)
        {
            if (rh.collider.gameObject.layer == 9)
            {
                //if (rh.collider.isTrigger)
                //{
                Debug.Log(rh.transform.name);
                Enemy EnemyHit = rh.transform.GetComponentInParent<Enemy>();
                HitboxChecker HitBox = rh.transform.GetComponent<HitboxChecker>();
                Debug.Log(rh.transform.name);
                if (HitBox.HitType == HitboxChecker.HitboxPart.Head)
                {
                    EnemyHit.IsDamaged(150);
                }
                else if (HitBox.HitType == HitboxChecker.HitboxPart.body)
                {
                    EnemyHit.IsDamaged(34);
                }
                else if (HitBox.HitType == HitboxChecker.HitboxPart.legs)
                {
                    EnemyHit.IsDamaged(22);
                }

                GameObject bloodEffect;
                bloodEffect = Instantiate(EnemyManager.BloodEffect, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
                bloodEffect.transform.Rotate(0, 0, Random.Range(-180, 180));
                bloodEffect.transform.SetParent(rh.transform);
                Destroy(bloodEffect, 4);
                //}
            }
            if (rh.transform.tag == "Enemy")
            {
                Debug.Log("Enemi touché");
                rh.rigidbody.AddForceAtPosition(force * direction, rh.point);
                rh.rigidbody.GetComponent<Enemy>().IsDamaged(27);
            }
            if (rh.transform.tag == "Environement")
            {
                GameObject Impact;
                Impact = Instantiate(BulletHole, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
                Impact.transform.Rotate(0, 0, Random.Range(-180, 180));
                Impact.transform.SetParent(rh.transform);
                Destroy(Impact, 50);

                GameObject HitFX;
                HitFX = Instantiate(HitEffect, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
                HitFX.transform.SetParent(rh.transform);
                Destroy(HitFX, 0.85f);
            }
            if (rh.transform.tag == "Object")
            {
                GameObject Impact;
                Impact = Instantiate(BulletHole, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
                Impact.transform.Rotate(0, 0, Random.Range(-180, 180));
                Impact.transform.SetParent(rh.transform);
                rh.rigidbody.AddForceAtPosition(force * direction, rh.point);
                Destroy(Impact, 20);

                GameObject HitFX;
                HitFX = Instantiate(HitEffect, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
                HitFX.transform.SetParent(rh.transform);
                Destroy(HitFX, 0.85f);
            }
        }

        ///*var tir = Instantiate(effect, ray.origin, Quaternion.Euler(ray.direction.x, ray.direction.y, ray.direction.z));
        //Debug.Log(ray.direction.x + ray.direction.y + ray.direction.z + "wow");
        //Destroy(tir, 5);*/

        //RaycastHit[] hits1 = Physics.RaycastAll(ray.origin, ray.direction, 100f, LayerMask);
        //Debug.DrawRay(ray.origin, ray.direction, Color.green,1f);

        ////if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane))
        ////{

        ////RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, 100f, LayerMask);
        //Debug.Log(hits1.Length);

        //// if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane, LayerMask))
        //// { 
        //var direction = transform.forward;

        //foreach (RaycastHit rh in hits1)
        //{
        //    if (rh.collider.gameObject.layer == 9)
        //    {
        //        if (rh.collider.isTrigger)
        //        {
        //            Enemy EnemyHit = rh.transform.GetComponentInParent<Enemy>();
        //            HitboxChecker HitBox = rh.transform.GetComponent<HitboxChecker>();
        //            Debug.Log(rh.transform.name);
        //            if (HitBox.HitType == HitboxChecker.HitboxPart.Head)
        //            {
        //                EnemyHit.IsDamaged(150);
        //            }
        //            else if (HitBox.HitType == HitboxChecker.HitboxPart.body)
        //            {
        //                EnemyHit.IsDamaged(34);
        //            }
        //            else if (HitBox.HitType == HitboxChecker.HitboxPart.legs)
        //            {
        //                EnemyHit.IsDamaged(22);
        //            }

        //            GameObject bloodEffect;
        //            bloodEffect = Instantiate(EnemyManager.BloodEffect, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
        //            bloodEffect.transform.Rotate(0, 0, Random.Range(-180, 180));
        //            bloodEffect.transform.SetParent(rh.transform);
        //            Destroy(bloodEffect, 4);
        //        }
        //    }

        //    if (rh.transform.tag == "Enemy")
        //    {
        //        Debug.Log("Enemi touché");
        //        rh.rigidbody.AddForceAtPosition(force * direction, rh.point);
        //        rh.rigidbody.GetComponent<Enemy>().IsDamaged(60);
        //    }
        //    if (rh.transform.tag == "Environement")
        //    {
        //        GameObject Impact;
        //        Impact = Instantiate(BulletHole, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
        //        Impact.transform.Rotate(0, 0, Random.Range(-180, 180));
        //        Impact.transform.SetParent(rh.transform);
        //        Destroy(Impact, 50);

        //        GameObject HitFX;
        //        HitFX = Instantiate(HitEffect, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
        //        HitFX.transform.SetParent(rh.transform);
        //        Destroy(HitFX, 0.85f);
        //    }
        //    if (rh.transform.tag == "Object")
        //    {
        //        GameObject Impact;
        //        Impact = Instantiate(BulletHole, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
        //        Impact.transform.Rotate(0, 0, Random.Range(-180, 180));
        //        Impact.transform.SetParent(rh.transform);
        //        rh.rigidbody.AddForceAtPosition(force * direction, rh.point);
        //        Destroy(Impact, 20);

        //        GameObject HitFX;
        //        HitFX = Instantiate(HitEffect, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
        //        HitFX.transform.SetParent(rh.transform);
        //        Destroy(HitFX, 0.85f);
        //    }
        //}
    }
    // }

    void Shoot()
    {

        NextFire = Time.time + (1 / (RPM / 60));
        if (SprayLenght >= 15)
            SprayLenght = 15;
        Bullets--;
        Vector2 CenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        ray = Camera.main.ScreenPointToRay(CenterPoint + new Vector2(0, 1.75f) * SprayLenght);

        GetComponent<AudioSource>().PlayOneShot(FireSound);


        /*var tir = Instantiate(effect, ray.origin, Quaternion.Euler(ray.direction.x, ray.direction.y, ray.direction.z));
        Debug.Log(ray.direction.x + ray.direction.y + ray.direction.z + "wow");
        Destroy(tir, 5);*/

        RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, 100f, LayerMask);
        Debug.DrawRay(ray.origin, ray.direction, Color.green, 1f);

        //  if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane,LayerMask))
        //  {
        var direction = transform.forward;

        foreach (RaycastHit rh in hits)
        {
            if (rh.collider.gameObject.layer == 9)
            {
                // if (rh.collider.isTrigger)
                //{
                Enemy EnemyHit = rh.transform.GetComponentInParent<Enemy>();
                HitboxChecker HitBox = rh.transform.GetComponent<HitboxChecker>();
                Debug.Log(rh.transform.name);
                if (HitBox.HitType ==
                HitboxChecker.HitboxPart.Head)
                {
                    EnemyHit.IsDamaged(150);
                    GameObject brains;
                    brains = Instantiate(EnemyManager.MeatEffect, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
                    brains.transform.Rotate(0, 0, Random.Range(-180, 180));
                    //brains.transform.SetParent(rh.transform);
                    Destroy(brains, 2.5f);
                }
                else if (HitBox.HitType == HitboxChecker.HitboxPart.body)
                {
                    EnemyHit.IsDamaged(34);
                }
                else if (HitBox.HitType == HitboxChecker.HitboxPart.legs)
                {
                    EnemyHit.IsDamaged(22);
                }

                GameObject bloodEffect;
                bloodEffect = Instantiate(EnemyManager.BloodEffect, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
                bloodEffect.transform.localScale *= 0.5f;
                bloodEffect.transform.SetParent(rh.transform);
                Destroy(bloodEffect, 3f);
                // }
            }
            if (rh.transform.tag == "Enemy")
            {
                Debug.Log("Enemi touché");
                rh.rigidbody.AddForceAtPosition(force * direction, rh.point);
                rh.rigidbody.GetComponent<Enemy>().IsDamaged(27);
            }
            if (rh.transform.gameObject.layer == 11)
            {

                rh.rigidbody.AddForceAtPosition(force * direction, rh.point);

                GameObject Impact;
                Impact = Instantiate(BulletHole, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
                Impact.transform.Rotate(0, 0, Random.Range(-180, 180));
                Impact.transform.SetParent(rh.transform);
                Destroy(Impact, 50);

                GameObject HitFX;
                HitFX = Instantiate(HitEffect, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
                HitFX.transform.SetParent(rh.transform);
                Destroy(HitFX, 0.85f);
            }
            if (rh.transform.tag == "Environement")
            {
                GameObject Impact;
                Impact = Instantiate(BulletHole, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
                Impact.transform.Rotate(0, 0, Random.Range(-180, 180));
                Impact.transform.SetParent(rh.transform);
                Destroy(Impact, 50);

                GameObject HitFX;
                HitFX = Instantiate(HitEffect, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
                HitFX.transform.SetParent(rh.transform);
                Destroy(HitFX, 0.85f);
            }
            if (rh.transform.tag == "Object")
            {
                GameObject Impact;
                Impact = Instantiate(BulletHole, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
                Impact.transform.Rotate(0, 0, Random.Range(-180, 180));
                Impact.transform.SetParent(rh.transform);
                rh.rigidbody.AddForceAtPosition(force * direction, rh.point);
                Destroy(Impact, 20);

                GameObject HitFX;
                HitFX = Instantiate(HitEffect, rh.point, Quaternion.FromToRotation(Vector3.forward, rh.normal)) as GameObject;
                HitFX.transform.SetParent(rh.transform);
                Destroy(HitFX, 0.85f);
            }
        }
        SprayLenght += 1;

        //if (hit.transform.GetComponent<HitboxChecker>().HitType == HitboxChecker.HitboxPart.legs)
        //{
        //    Debug.Log("leg");
        //}
        //if (hit.transform.tag == "Enemy")
        //{
        //    Debug.Log("Enemi touché");
        //    hit.rigidbody.AddForceAtPosition(force * direction, hit.point);
        //    hit.rigidbody.GetComponent<Enemy>().IsDamaged(27);
        //}
        //if (hit.transform.tag == "Environement")
        //{
        //    GameObject Impact;
        //    Impact = Instantiate(BulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)) as GameObject;
        //    Impact.transform.Rotate(0, 0, Random.Range(-180, 180));
        //    Impact.transform.SetParent(hit.transform);
        //    Destroy(Impact, 50);

        //    GameObject HitFX;
        //    HitFX = Instantiate(HitEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)) as GameObject;
        //    HitFX.transform.SetParent(hit.transform);
        //    Destroy(HitFX, 0.85f);
        //}
        //if (hit.transform.tag == "Object")
        //{
        //    GameObject Impact;
        //    Impact = Instantiate(BulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)) as GameObject;
        //    Impact.transform.Rotate(0, 0, Random.Range(-180, 180));
        //    Impact.transform.SetParent(hit.transform);
        //    hit.rigidbody.AddForceAtPosition(force * direction, hit.point);
        //    Destroy(Impact, 20);

        //    GameObject HitFX;
        //    HitFX = Instantiate(HitEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)) as GameObject;
        //    HitFX.transform.SetParent(hit.transform);
        //    Destroy(HitFX, 0.85f);
        //}
        //  }
    }
    IEnumerator Reload()
    {
        GetComponent<AudioSource>().PlayOneShot(ReloadSound);
        yield return new WaitForSeconds(2.52f);
        Bullets = BulletsPerMag;
        Magazines--;
        yield return new WaitForSeconds(0.48f);
        IsReloading = false;
    }
}

//    ammo--; //Explained by itself
//}
//if (ammo == 0)
//{   //If you no longer have ammo
//    if (cooldown > Time.time)
//    {   //If there is no cooldown (relatively)
//        cooldown = Time.time + cooldownSeconds;
//    }
//}
