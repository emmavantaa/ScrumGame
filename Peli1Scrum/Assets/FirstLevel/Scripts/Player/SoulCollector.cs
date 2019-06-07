using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
//[DebuggerStepThrough]
public class SoulCollector : MonoBehaviour
{
    public float collectedSouls = 0;
    bool soulCollected;
    public float showTextTime;
    float time1;
    bool showText;

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
        }

    }
    public void CollectSoul()
    {
        collectedSouls += 1;
        soulCollected = true;
        showText = true;
        time1 = Time.time;

    }
    private void OnGUI()
    {
        if (showText)
        {
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.normal.textColor = Color.red;
            centeredStyle.fontSize = 100;
            centeredStyle.alignment = TextAnchor.UpperCenter;
            GUI.Label(new Rect(Screen.width / 2 - 415, Screen.height / 2 - 255, 800, 400), "You have collected a soul", centeredStyle);

        }
    }

}
