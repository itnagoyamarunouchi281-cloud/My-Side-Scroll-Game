using UnityEngine;

public class Attack2DContoroll : MonoBehaviour
{
    bool hitflg;

    public float effectPosY;

    public GameObject damageEffect;

    EnemyStatus enemyStatus;

    private GameObject player;
    private MyEnemy myEnemy;
    private EnemyInfo enemyInfo;
    private EnemysBase enemy;

    GameObject se;

	int AttackCnt = 1;
    bool isAttack;

	void Start()
    {
        player = GameObject.Find("Actor");
        myEnemy = player.GetComponent<MyEnemy>();

        hitflg = false;

        se = GameObject.Find("SE");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)&& !hitflg)
        {
            if (AttackCnt == 1)
            {
                
            }
            if (AttackCnt == 2)
            {
                
            }
            if (AttackCnt == 3)
            {
                
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

            Vector2 attackDirection = (enemy.transform.position - player.transform.position).normalized;
            enemy.TakeDamage(20, attackDirection);

            if (se != null)
            {
                // �U���q�b�g��
                se.GetComponent<SEManager>().PlaySE(0);
            }

            isAttack = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // �I�u�W�F�N�g�^�O��Enemy�̂Ƃ�
        if (!hitflg && collision.tag == "Enemy")
        {
            // �G�L������|���������擾
            enemyInfo = collision.gameObject.GetComponent<EnemyInfo>();
            if (enemyInfo == null) return;

            enemy = collision.gameObject.GetComponent<EnemysBase>();
            if (enemy == null) return;

            enemyStatus = collision.gameObject.GetComponent<EnemyStatus>();
            if (enemyStatus == null) return;

            Attack();

            hitflg = true;

            if (damageEffect != null)
            {
                GameObject effect = Instantiate(damageEffect) as GameObject;
                effect.transform.position = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y + effectPosY,
                    this.gameObject.transform.position.z - 2.0f);
            }

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

    void OnTriggerExit2D(Collider2D t)
    {
        //Debug.Log("atattayo!!");
        //Hitflg = false;

        AttackCnt = 0;
    }
}