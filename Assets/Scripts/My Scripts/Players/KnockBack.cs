using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    private PlayerStatus playerStatus;
    Vector3 prePos;
    Vector3 enemyPos;
    bool isStart = false;
    int Step = 0;
    EnemyStatus enemyStatus;
    Rigidbody rb;
    float inoperableTime = 0.1f;
    bool isInoperable = false;

    Renderer[] childrenRenderer;

    bool isEnabledRenderers;
    bool isDamaged;
    Coroutine flicker;
    float flickerDuration = 0.6f;
    float invincibleTime;
    float flickerTotalElapsedTime;
    float flickerElapsedTime;
    float flickerInterval = 0.075f;
    float KnockTime = 0.0f;

    Animator anime;

    GameObject se;

    // Start is called before the first frame update
    void Start()
    {
        invincibleTime = flickerDuration;

        playerStatus = GetComponent<PlayerStatus>();
        rb = GetComponent<Rigidbody>();

        childrenRenderer = GetComponentsInChildren<Renderer>();

        anime = GetComponent<Animator>();

        se = GameObject.Find("SE");
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart && !anime.GetCurrentAnimatorStateInfo(0).IsName("Knock"))
        {
            knockback();
        }
    }

    public void knockback()
    {
        switch (Step)
        {
            case 0:
                anime.SetBool("isKnock", true);
                
                if (enemyStatus.GetATK() - StaticStatus.GetPlayerDEF() > 0)
                {
                    //Debug.Log(enemyStatus.GetATK() - StaticStatus.GetPlayerDEF());
                    playerStatus.SetMinusHp(enemyStatus.GetATK() - StaticStatus.GetPlayerDEF());
                }
                else
                {
                    playerStatus.SetMinusHp(1);
                }

                Step++;
                break;

            case 1:

                this.gameObject.layer = LayerMask.NameToLayer("Invisible");

                isInoperable = true;

                Vector3 distination = new Vector3(this.transform.position.x - enemyPos.x, 0, 0).normalized;

                if (Mathf.Abs(prePos.x - this.transform.position.x) < 1)
                {
                    Knock(distination.x);

                    KnockTime += Time.deltaTime;
                    if (KnockTime > 1.0f)
                    {
                        Step++;
                    }
                }
                else
                {
                    Step++;
                    anime.SetBool("isKnock", false);
                }



                break;

            case 2:
                if (isDamaged)
                    return;
                StartFlicker();
                Step++;
                break;

            case 3:
                if (0 < invincibleTime)
                {
                    invincibleTime -= Time.deltaTime;
                    if(0 < inoperableTime)
                    {
                        inoperableTime -= Time.deltaTime;
                    }
                    else
                    {
                        isInoperable = false;
                    }
                }
                else
                {
                    Step++;
                }
                break;

            
            case 4:

                isStart = false;
                Step = 0;
                this.gameObject.layer = LayerMask.NameToLayer("Player");
                invincibleTime = 2.0f;
                inoperableTime = 0.1f;
                KnockTime = 0.0f;
                break;
        }

    }


    void SetEnabledRenderers(bool b)
    {
        for (int i = 0; i < childrenRenderer.Length; i++)
        {
            childrenRenderer[i].enabled = b;
        }
    }

    void StartFlicker()
    {
        flicker = StartCoroutine("Flicker");
    }


    IEnumerator Flicker()
    {
        isDamaged = true;

        flickerTotalElapsedTime = 0;
        flickerElapsedTime = 0;

        while (true)
        {
            flickerTotalElapsedTime += Time.deltaTime;
            flickerElapsedTime += Time.deltaTime;

            if (flickerInterval <= flickerElapsedTime)
            {
                flickerElapsedTime = 0;
                isEnabledRenderers = !isEnabledRenderers;
                SetEnabledRenderers(isEnabledRenderers);
            }


            if (flickerDuration <= flickerTotalElapsedTime)
            {
                isDamaged = false;

                isEnabledRenderers = true;
                SetEnabledRenderers(true);

                yield break;
            }
            yield return null;
        }
    }

    void ResetFlicker()
    {
        if (flicker != null)
        {
            StopCoroutine(flicker);
            flicker = null;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" && !isStart)
        {
            if (se != null)
            {
                se.GetComponent<SEManager>().PlaySE(0);
            }

            isStart = true;
            enemyPos = other.gameObject.transform.position;
            prePos = this.transform.position;
            enemyStatus = other.gameObject.GetComponentInChildren<EnemyStatus>();
            
        }
    }

    private void Knock(float knockX)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(knockX, 0), ForceMode.Impulse);
    }

    public bool GetIsInoperable()
    {
        return isInoperable;
    }
}
