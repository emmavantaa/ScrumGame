using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
[DebuggerStepThrough]
public class Soul : MonoBehaviour
{
    public float collectedSouls = 0;
    bool soulCollected;

    public float showTextTime;
    float time1;
    public bool showText;
    public bool immortal;

    GUIStyle style = new GUIStyle();
    //public bool knockback;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        showTextTime = 4;
        style.normal.textColor = Color.red;
        style.fontSize = 80;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Souls: " + collectedSouls);

        
        if (soulCollected && Time.time >= +time1 + showTextTime)
        {
            showText = false;
            soulCollected = false;
        }

    }
    public void CollectSoul()
    {
            collectedSouls += 1;
        soulCollected = true;
        time1 = Time.time;

    }
    private void OnGUI()
    {
        if (soulCollected)
        {
            GUI.Label(new Rect(400, 400, 200, 200), "You have collected a soul", style);
            showText = true;
        }
    }

}
