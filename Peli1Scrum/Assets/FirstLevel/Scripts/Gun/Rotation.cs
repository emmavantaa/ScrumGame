using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    public float Rspeed = 1f;
    public GameObject Gun;
    public ShootingScript shooting;
    public GameObject bunnyzuukaUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, Rspeed, 0, Space.Self);

    }
 

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            bunnyzuukaUI.SetActive(true);
            Gun.SetActive(true);

            Destroy(gameObject, 0.2f);
            shooting.ShootingisOn = true;
        }
    }
}
