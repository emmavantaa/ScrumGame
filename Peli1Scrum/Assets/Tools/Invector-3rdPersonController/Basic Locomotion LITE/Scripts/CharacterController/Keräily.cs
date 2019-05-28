using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keräily : MonoBehaviour
{
    GameObject meleeWeapon;
    GameObject meleeWeaponPlace;
    GameObject player;
    public Rigidbody rb;
    public Collider col;
    public bool collected;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //rb = player.GetComponent<Rigidbody>();
        meleeWeapon = GameObject.Find("MeleeWeapon");
        meleeWeaponPlace = GameObject.Find("MeleeWeaponPlace");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        //rb.AddForce(Vector3.up*10);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (/*other==meleeWeapon*/ other.gameObject.name == "MeleeWeapon" && collected == false)
        {

            meleeWeapon.transform.parent = transform;
            //meleeWeapon.transform.position = meleeWeaponPlace.transform.position;
            meleeWeapon.transform.localPosition = meleeWeaponPlace.transform.localPosition;
            meleeWeapon.transform.localRotation = Quaternion.Euler(Vector3.forward);
            //meleeWeapon.transform.Translate(0, 0, -0.5f);
            meleeWeapon.transform.localRotation=Quaternion.Euler(90, 0, 90);
            //meleeWeapon.GetComponent<Collider>().isTrigger = false;

            //meleeWeapon.transform.LookAt(transform.forward);
            //meleeWeapon.transform.TransformDirection(player.transform.forward);

            //meleeWeapon.transform.LookAt(player.transform.forward);
            //meleeWeapon.transform.Rotate(new Vector3(0, -90, 0), Space.Self);
            collected = true;
        }
        if (other.gameObject.name=="HealthPack")
        {
            player.gameObject.GetComponent<Health>().MoreHealth();
            Destroy(other.gameObject);
        }
    }
}
