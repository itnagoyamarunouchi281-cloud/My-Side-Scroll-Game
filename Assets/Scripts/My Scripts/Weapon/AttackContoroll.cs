using UnityEngine;

public class AttackContoroll : MonoBehaviour
{
    bool hitflg;

    public float effectPosY;

    public GameObject damageEffect1;
    public GameObject damageEffect2;
    public GameObject damageEffect3;

    EnemyStatus enemyStatus;
    
    private MyEnemy myEnemy;
    private EnemyInfo enemyInfo;
    private Enemy enemy;

    GameObject se;

	int AttackCnt = 1;
    bool isAttack;

	void Start()
    {
        myEnemy = GameObject.Find("Actor").GetComponent<MyEnemy>();

        hitflg = false;

        se = GameObject.Find("SE");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && hitflg)
        {
            if (AttackCnt == 1)
            {
                if (damageEffect1 != null)
                {
                    GameObject effect = Instantiate(damageEffect1) as GameObject;
                    effect.transform.position = new Vector3(
                        this.gameObject.transform.position.x,
                        this.gameObject.transform.position.y + effectPosY,
                        this.gameObject.transform.position.z - 2.0f);
                }

                Attack();
            }
            if (AttackCnt == 2)
            {
                if (damageEffect2 != null)
                {
                    GameObject effect = Instantiate(damageEffect2) as GameObject;
                    effect.transform.position = new Vector3(
                        this.gameObject.transform.position.x,
                        this.gameObject.transform.position.y + effectPosY,
                        this.gameObject.transform.position.z - 2.0f);
                }

                Attack();
            }
            if (AttackCnt == 3)
            {
                if (damageEffect3 != null)
                {
                    GameObject effect = Instantiate(damageEffect3) as GameObject;
                    effect.transform.position = new Vector3(
                        this.gameObject.transform.position.x,
                        this.gameObject.transform.position.y + effectPosY,
                        this.gameObject.transform.position.z - 2.0f);
                }

                Attack();

                AttackCnt = 0;
            }

            isAttack = true;
        }
	}

    private void Attack()
    {
        if(isAttack)
        {
            // �G���󂯂�_���[�W�i�v���C���[�̍U���� - �G�̖h��́j
            int damage = Mathf.Max(1, StaticStatus.GetPlayerATK() - enemyStatus.GetDEF());
            Debug.Log($"damage: {damage}");
            enemyStatus.SetHp(damage);

            if (se != null)
            {
                // �U���q�b�g��
                se.GetComponent<SEManager>().PlaySE(0);
            }

            isAttack = false;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        // �I�u�W�F�N�g�^�O��Enemy�̂Ƃ�
        if (!hitflg && collision.tag == "Enemy")
        {
            // �G�L������|���������擾
            enemyInfo = collision.gameObject.GetComponent<EnemyInfo>();
            
            enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy == null) return;

            enemyStatus = collision.gameObject.GetComponent<EnemyStatus>();

            Attack();

            hitflg = true;

            AttackCnt++;
        }
    }

    public void SethitFlg(bool flg)
    {
        hitflg = flg;
    }

    public bool GethitFlg()
    {
        return hitflg;
    }

    private void OnTriggerStay(Collider other)
    {
        // �I�u�W�F�N�g�^�O��Enemy�̂Ƃ�
        if (!hitflg && other.tag == "Enemy")
        {
            // �G�L������|���������擾
            enemyInfo = other.gameObject.GetComponent<EnemyInfo>();
            
            enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy == null) return;
            enemyStatus = other.gameObject.GetComponent<EnemyStatus>();
            
            Attack();

            PlayerController2D.OnPlayerDamagedEvent.Invoke();

            hitflg = true;
            AttackCnt++;
        }
    }

    void OnTriggerExit(Collider t)
    {
        //Debug.Log("atattayo!!");
        //Hitflg = false;

        hitflg = false;

        AttackCnt = 1;
    }
}