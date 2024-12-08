using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rb;

    public Animator animator;

    public GameObject bullet;

    private float moveSpeed;
    public float bulletSpeed;
    private bool isBlock = true;

    public float knockBackPower;
    private bool isDamaged = false;
    private bool isInvincible = false;
    public TextMeshProUGUI HpText;

    //[SerializeField]���������Ƃɂ��public�łȂ��Ă�Inspector����l��ҏW�ł��܂�
    [SerializeField]
    private float hp = 5;  //�̗�

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 90, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        // �e����
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("joystick button 2"))
        {
            Bullet();
        }
        PlayerHp();

        if (isInvincible)
        {
            int invincibleTime = 30;
            invincibleTime--;

            if (invincibleTime <= 0)
            {
                isInvincible = false;
                isDamaged = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //�^�O��EnemyBullet�̃I�u�W�F�N�g��������������{}���̏������s����
        if (other.gameObject.tag == "Enemy")
        {
            if (isInvincible)
            {
                return;
            }

            Debug.Log("hit Player");

            hp -= other.gameObject.GetComponent<Enemy1Script>().powerEnemy;
            rb.velocity = Vector3.zero;
            // �����̈ʒu�ƐڐG���Ă����I�u�W�F�N�g�̈ʒu�Ƃ��v�Z���āA�����ƕ������o���Đ��K��(���x�x�N�g�����Z�o)
            Vector3 distination = (transform.position - other.transform.position).normalized;
            distination.y = 0f;
            distination = distination.normalized;
            rb.AddForce(distination * knockBackPower, ForceMode.VelocityChange);

            isDamaged = true;

            if (isDamaged == true)
            {
                float level = Mathf.Abs(Mathf.Sin(Time.time * 30));
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, level);
                isInvincible = true;
            }

            if (isDamaged == false)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    private void PlayerMove()
    {
        moveSpeed = 5.0f;

        Vector3 v = rb.velocity;

        float stick = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.RightArrow) || stick > 0)
        {
            v.x = moveSpeed;
            transform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("RUN", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || stick < 0)
        {
            v.x = -moveSpeed;
            transform.rotation = Quaternion.Euler(0, -90, 0);
            animator.SetBool("RUN", true);
        }
        else
        {
            v.x = 0;
            animator.SetBool("RUN", false);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && isBlock == true)
        {
            v.y += 10.0f;
        }

        rb.velocity = v;

        //���C
        Vector3 rayPosition = transform.position + new Vector3(0.0f, 0.8f, 0.0f);
        Ray ray = new Ray(rayPosition, Vector3.down);

        float distance = 0.9f;

        isBlock = Physics.Raycast(ray, distance);

        if (isBlock == true)
        {
            Debug.DrawRay(rayPosition, Vector3.down * distance, Color.red);
            animator.SetBool("JUMP", false);
        }
        else
        {
            Debug.DrawRay(rayPosition, Vector3.down * distance, Color.yellow);
            animator.SetBool("JUMP", true);
        }
    }

    private void Bullet()
    {
        bool bulletDirection = false;

        Vector3 position = this.transform.position;

        float stick = Input.GetAxisRaw("Vertical");

        //�e����ɑł�
        if (Input.GetKey(KeyCode.W) || stick > 0)
        {
            bulletDirection = true;
            position.y += 1.8f;
        }
        else
        {
            bulletDirection = false;
            position.y += 0.8f;
            position.x += 0.5f * this.transform.forward.x;
        }

        GameObject newbullet = Instantiate(bullet, position, Quaternion.identity); //�e�𐶐�
        Rigidbody bulletRigidbody = newbullet.GetComponent<Rigidbody>();

        if (bulletDirection == false)
        {
            bulletRigidbody.AddForce(this.transform.forward * bulletSpeed); //�L�����N�^�[�������Ă�������ɒe�ɗ͂�������
        }
        else
        {
            bulletRigidbody.AddForce(transform.up * bulletSpeed); //������ɒe�ɗ͂�������
        }

        Destroy(newbullet, 10); //10�b��ɒe������
    }

    private void PlayerHp()
    {
        HpText.text = "HP" + hp;
    }
}
