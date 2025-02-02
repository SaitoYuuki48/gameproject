using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rb;

    public Animator animator;

    public GameObject bullet;

    public float jumpForce = 1f;         // ��{�̃W�����v��
    public float maxJumpTime = 0.01f;     // �ő�W�����v����
    public float jumpMultiplier = 1f;   // ���������̒ǉ��W�����v��

    private bool isGrounded;
    private float jumpTimeCounter;
    private bool isJumping;

    private Vector3 rdVel;
    private float moveSpeed;              //�v���C���[�̑��x
    public float bulletSpeed;             //�e�̑��x

    public float knockBackPower;
    
    public TextMeshProUGUI HpText;

    //[SerializeField]���������Ƃɂ��public�łȂ��Ă�Inspector����l��ҏW�ł��܂�
    [SerializeField]
    private float hp = 5;  //�̗�

    [SerializeField] private float invincibleDuration = 2f;  // ���G���ԁi�b�j
    private bool isInvincible = false;                       // ���G��ԃt���O
    private float invincibleTimer = 0f;                      // ���G��Ԃ̃^�C�}�[
    //private Renderer playerRenderer;                         
    private List<Renderer> playerRenderers = new List<Renderer>(); // �v���C���[��Renderer

    public bool IsInvincible => isInvincible; // ���G��Ԃ��擾����v���p�e�B
    public float InvincibleDuration => invincibleDuration; // ���G���Ԃ̒������擾����v���p�e�B

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //playerRenderer = GetComponent<Renderer>();

        // �q�I�u�W�F�N�g���܂߂đS�Ă�Renderer���擾
        playerRenderers.AddRange(GetComponentsInChildren<Renderer>());

        transform.rotation = Quaternion.Euler(0, 90, 0);
        Physics.gravity = new Vector3(0, -20f, 0);
        hp = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Player();
        // �e����
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("joystick button 2"))
        {
            Bullet();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �G�ɐڐG�����ꍇ�̏���
        if (collision.gameObject.tag == "Enemy1" && !isInvincible)
        {
            Debug.Log("Player hit by Enemy1");
            hp -= collision.gameObject.GetComponent<Enemy1Script>().powerEnemy;
            rb.velocity = Vector3.zero;

            // ���G���Ԃ��X�^�[�g
            StartCoroutine(InvincibleTime());
        }
        //else if (collision.gameObject.tag == "Wall" && !isGrounded)
        //{
        //    rdVel.x = -moveSpeed*2;
        //}
    }

    private void Player()
    {
        PlayerMove();
        PlayerJump();
        PlayerHp();
    }

    private void PlayerMove()
    {
        moveSpeed = 5.0f;

        rdVel = rb.velocity;

        float stick = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.RightArrow) || stick > 0)
        {
            rdVel.x = moveSpeed;
            transform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("RUN", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || stick < 0)
        {
            rdVel.x = -moveSpeed;
            transform.rotation = Quaternion.Euler(0, -90, 0);
            animator.SetBool("RUN", true);
        }
        else
        {
            rdVel.x = 0;
            animator.SetBool("RUN", false);
        }

        rb.velocity = rdVel;
    }

    private void PlayerJump()
    {
        //������̃W�����v�̏���
        // �n�ʂɂ��邩�m�F
        //���C
        Vector3 rayPosition = transform.position + new Vector3(0.0f, 0.8f, 0.0f);
        Ray ray = new Ray(rayPosition, Vector3.down);

        float distance = 0.9f;

        isGrounded = Physics.Raycast(ray, distance);
         

        // �W�����v�J�n
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;

            // ���݂�y���x�����Z�b�g���ăW�����v�͂𒼐ڐݒ�
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }

        // �W�����v���Ƀ{�^�������������Ă���ꍇ
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey("joystick button 0")) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                // �����������𒴂��Ȃ��悤�ɂ���
                if (rb.velocity.y < jumpForce * 1.2f)
                {
                    rb.velocity += new Vector3(0, jumpForce * 0.0001f * Time.deltaTime, 0);
                }
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        // �{�^���𗣂����Ƃ�
        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp("joystick button 0")))
        {
            isJumping = false;

            if (rb.velocity.y > jumpForce / 2)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce / 2, rb.velocity.z);  // �����X�s�[�h�����炷
            }
            
        }

        if (isGrounded == true)
        {
            Debug.DrawRay(rayPosition, Vector3.down * distance, Color.red);
            animator.SetBool("JUMP", false);
        }
        else
        {
            Debug.DrawRay(rayPosition, Vector3.down * distance, Color.yellow);
            animator.SetBool("JUMP", true);
        }

        //Debug.Log("�W�����v���x: " + rb.velocity.y);
    }

    private void PlayerHp()
    {
        HpText.text = "HP " + hp;

        if (hp <= 0)
        {
            Debug.Log("Game Over");
            // �Q�[���I�[�o�[�����������ɋL�q
            // ��: �v���C���[�̓������~�߂�A���X�|�[������A�Q�[���I�[�o�[��ʂ�\������
            animator.SetTrigger("DEAD");
            rb.velocity = Vector3.zero;
            this.enabled = false;  // �v���C���[����𖳌��ɂ���
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

    public void TakeDamage(float damage)
    {
        if (!isInvincible && hp > 0) // ���G��ԂłȂ��AHP��0���傫���ꍇ�̂݃_���[�W����
        {
            hp -= damage;

            if (hp <= 0)
            {
                hp = 0; // HP��0�ŌŒ�
                Debug.Log("Game Over");
                animator.SetTrigger("DEAD");
                rb.velocity = Vector3.zero;
                this.enabled = false; // �v���C���[����𖳌��ɂ���
            }
            else
            {
                Debug.Log($"Player took {damage} damage. Remaining HP: {hp}");
            }

            StartCoroutine(InvincibleTime()); // ���G���Ԃ��J�n
        }
        else if (hp <= 0)
        {
            Debug.Log("Player is already dead. No further damage.");
        }
    }

    private IEnumerator InvincibleTime()
    {
        isInvincible = true;
        invincibleTimer = invincibleDuration;

        gameObject.layer = LayerMask.NameToLayer("Invincible");

        float blinkInterval = 0.05f;
        int blinkCount = Mathf.CeilToInt(invincibleDuration / blinkInterval);

        for (int i = 0; i < blinkCount; i++)
        {
            // ���ׂĂ�Renderer��_�ł�����
            foreach (Renderer renderer in playerRenderers)
            {
                if (renderer != null)
                {
                    renderer.enabled = !renderer.enabled;
                }
            }
            yield return new WaitForSeconds(blinkInterval);
        }

        // ���G���ԏI����A���C���[�ƌ����ڂ����ɖ߂�
        gameObject.layer = LayerMask.NameToLayer("Player");

        foreach (Renderer renderer in playerRenderers)
        {
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }

        isInvincible = false;
    }
}
