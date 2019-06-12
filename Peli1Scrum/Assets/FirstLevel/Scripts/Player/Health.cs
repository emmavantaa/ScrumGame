using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Invector.CharacterController;
using System.Diagnostics;
using UnityEngine.UI;
//[DebuggerStepThrough]
public class Health : MonoBehaviour
    
{
    public float playerHp;
    GameObject player;
    public float immortalityTime=2;
    public float damageEffectTimeWhenNotUseImmortalMomentAll;
    [SerializeField]
    float timeUntilRestart;
    float timeWhenHit;
    float timeWhenHitByEnemy;
    float timeWhenHitByLava;
    float timeWhenHitByFall;
    public bool immortal;
    public bool useImmortalMomentAll;
    public bool useImmortalMomentHitByEnemy;
    public bool useImmortalMomentHitByLava;
    public bool useImmortalMomentHitByFall;
    public Text healthAmountText;

    [HideInInspector]
    public bool immortalMoment;
    [HideInInspector]
    public bool immortalMomentHitByEnemy;
    [HideInInspector]
    public bool immortalMomentHitByLava;
    [HideInInspector]
    public bool immortalMomentHitByFall;
    [HideInInspector]
    public bool hitByEnemy;
    [HideInInspector]
    public bool hitByLava;
    [HideInInspector]
    public bool hitByFall;
    [HideInInspector]
    public bool dead;

    float startShowingTextTime;
    bool isBloodEmissionOn;
    bool isLavaEmissionOn;
    bool isFallEmissionOn;

    vThirdPersonController cc;
    GUIStyle style = new GUIStyle();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cc = player.GetComponent<vThirdPersonController>();

    }
    private void OnGUI()
    {

        if (dead)
        {
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.normal.textColor = Color.red;
            centeredStyle.fontSize = 300;
            centeredStyle.alignment = TextAnchor.UpperCenter;
            GUI.Label(new Rect(Screen.width / 2 - 415, Screen.height / 2 - 255, 800, 400), "Död", centeredStyle);
        }

        
    }
    public void Die()
    {
        
        dead = true;
        startShowingTextTime = Time.time;

    }
    // Update is called once per frame
    void Update()
    {
        healthAmountText.text = "Health: " + playerHp;
        if (dead)
        {

            healthAmountText.text = "Health: " + "Död";
            cc.lockMovement = true;
            if (Time.time >= startShowingTextTime + 1)
            {
                player.GetComponent<Rigidbody>().isKinematic = true;
            }
            

        }
        if (useImmortalMomentAll)
        {
            useImmortalMomentHitByEnemy = false;
            useImmortalMomentHitByLava = false;
            useImmortalMomentHitByFall = false;
        }
        //Debug.Log("HP: "+playerHp);
        if (dead && Time.time >= startShowingTextTime + timeUntilRestart)
        {
            SceneManager.LoadScene("Level1");
        }
        if (playerHp <= 0&&!dead)
        {
            Die();
        }
        if (useImmortalMomentAll)
        {
            if (immortalMoment && Time.time >= +timeWhenHit + immortalityTime)
            {
                immortalMoment = false;
            }

            if (immortalMoment)
            {
                if (hitByEnemy)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().BloodEmissionOn();
                    }
                }
                if (hitByLava)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().LavaEmissionOn();
                    }
                }
                if (hitByFall)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().FallEmissionOn();
                    }
                }
            }

            else if (!immortalMoment)
            {
                if (hitByEnemy)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().BloodEmissionOff();
                        hitByEnemy = false;
                    }
                }
                if (hitByLava)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().LavaEmissionOff();
                        hitByLava = false;
                    }
                }
                if (hitByFall)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().FallEmissionOff();
                        hitByFall = false;
                    }
                }

            }
        }

        else
        {
            if (useImmortalMomentHitByEnemy)
            {
                if (immortalMomentHitByEnemy && Time.time >= timeWhenHitByEnemy + immortalityTime)
                {
                    immortalMomentHitByEnemy = false;
                }

                if (immortalMomentHitByEnemy && hitByEnemy)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().BloodEmissionOn();
                    }
                }

                else if(!immortalMomentHitByEnemy&&hitByEnemy)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().BloodEmissionOff();
                        hitByEnemy = false;
                    }
                }

            }
            else if (!useImmortalMomentHitByEnemy)
            {
                if (isBloodEmissionOn && Time.time <= timeWhenHitByEnemy + damageEffectTimeWhenNotUseImmortalMomentAll)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().BloodEmissionOn();
                    }
                }

                else
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().BloodEmissionOff();
                        isBloodEmissionOn = false;
                    }
                }

            }

            if (useImmortalMomentHitByLava)
            {
                if (immortalMomentHitByLava && Time.time >= timeWhenHitByLava + immortalityTime)
                {
                    immortalMomentHitByLava = false;
                }

                if (immortalMomentHitByLava && hitByLava)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().LavaEmissionOn();
                    }
                }

                else if(!immortalMomentHitByLava&&hitByLava)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().LavaEmissionOff();
                        hitByLava = false;
                        
                    }
                }

            }
            else if (!useImmortalMomentHitByLava)
            {
                if (isLavaEmissionOn && Time.time <= timeWhenHitByLava + damageEffectTimeWhenNotUseImmortalMomentAll)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().LavaEmissionOn();
                    }
                }

                else
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().LavaEmissionOff();
                        isLavaEmissionOn = false;
                    }
                }

            }
            if (useImmortalMomentHitByFall)
            {
                if (immortalMomentHitByFall && Time.time >= timeWhenHitByFall + immortalityTime)
                {
                    immortalMomentHitByFall = false;
                }

                if (immortalMomentHitByFall && hitByFall)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().FallEmissionOn();
                    } 
                }

                else if(!immortalMomentHitByFall&&hitByFall)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().FallEmissionOff();
                        hitByFall = false;
                    }
                }

            }
            else if (!useImmortalMomentHitByFall)
            {
                if (isFallEmissionOn && Time.time <= timeWhenHitByFall + damageEffectTimeWhenNotUseImmortalMomentAll)
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().FallEmissionOn();

                    }
                }

                else
                {
                    if (player.GetComponent<ImmortalEmission>())
                    {
                        player.GetComponent<ImmortalEmission>().FallEmissionOff();
                        isFallEmissionOn = false;
                    }
                }

            }
        }

    }

    public void TakeDamage()
    {
        if (useImmortalMomentAll)
        {
            if (!immortalMoment && !immortal)
            {
                if (hitByEnemy)
                {
                    playerHp -= 1;
                }
                if (hitByLava)
                {
                    playerHp -= 2;
                }
                if (hitByFall)
                {
                    playerHp -= 1;
                }

                timeWhenHit = Time.time;
                immortalMoment = true;
            }
        }
        else
        {
            
            if (hitByEnemy && !immortal)
            {
                if (useImmortalMomentHitByEnemy)
                {
                    if (!immortalMomentHitByEnemy)
                    {
                        playerHp -= 1;
                        timeWhenHitByEnemy = Time.time;
                        immortalMomentHitByEnemy = true;
                    }
                }
                else
                {
                    playerHp -= 1;
                    timeWhenHitByEnemy = Time.time;
                    isBloodEmissionOn = true;
                    hitByEnemy = false;

                }
            }
            
            if (hitByLava && !immortal)
            {
                if (useImmortalMomentHitByLava)
                {
                    if (!immortalMomentHitByLava)
                    {
                        playerHp -= 2;
                        timeWhenHitByLava = Time.time;
                        immortalMomentHitByLava = true;
                    }
                }
                else
                {
                    playerHp -= 2;
                    timeWhenHitByLava = Time.time;
                    isLavaEmissionOn = true;
                    hitByLava = false;
                }
            }
           
            if (hitByFall && !immortal)
            {
                if (useImmortalMomentHitByFall)
                {
                    if (!immortalMomentHitByFall)
                    {
                        playerHp -= 1;
                        timeWhenHitByFall = Time.time;
                        immortalMomentHitByFall = true;
                    }  
                }
                else
                {
                    playerHp -= 1;
                    timeWhenHitByFall = Time.time;
                    isFallEmissionOn = true;
                    hitByFall = false;
                }
                
                
            }
        }
    }
    public void MoreHealth()
    {
        if (playerHp<3)
        {
            playerHp += 1;
        }
        
    }
}
