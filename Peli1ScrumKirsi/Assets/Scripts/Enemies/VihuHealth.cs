using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
//[DebuggerStepThrough]
public class VihuHealth : MonoBehaviour
{
    public float hp = 1;
    public bool immortal;
    GameObject vihu;
    // Start is called before the first frame update
    void Start()
    {
        vihu = transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0&&vihu!=null)
        {
            //float step = kaatumisSpeed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, OpenDrawerPosition.transform.position, step);

            //gameObject.SetActive(false);
            Destroy(vihu);
        }
    }
    public void takeDamage()
    {
        if (immortal == false)
        {
            hp -= 1;
        }

    }
}
