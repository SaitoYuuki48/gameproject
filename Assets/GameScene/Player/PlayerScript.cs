using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rb;

    public Animator animator;

    public GameObject bullet;

    public float jumpForce = 1f;         // 基本のジャンプ力
    public float maxJumpTime = 0.01f;     // 最大ジャンプ時間
    public float jumpMultiplier = 1f;   // 長押し時の追加ジャンプ力

    private bool isGrounded;
    private float jumpTimeCounter;
    private bool isJumping;

    private Vector3 rdVel;
    private float moveSpeed;              //プレイヤーの速度
    public float bulletSpeed;             //弾の速度

    public float knockBackPower;
    
    public TextMeshProUGUI HpText;

    //[SerializeField]を書くことによりpublicでなくてもInspectorから値を編集できます
    [SerializeField]
    private float hp = 5;  //体力

    [SerializeField] private float invincibleDuration = 2f;  // 無敵時間（秒）
    private bool isInvincible = false;                       // 無敵状態フラグ
    private float invincibleTimer = 0f;                      // 無敵状態のタイマー
    //private Renderer playerRenderer;                         
    private List<Renderer> playerRenderers = new List<Renderer>(); // プレイヤーのRenderer

    public bool IsInvincible => isInvincible; // 無敵状態を取得するプロパティ
    public float InvincibleDuration => invincibleDuration; // 無敵時間の長さを取得するプロパティ

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //playerRenderer = GetComponent<Renderer>();

        // 子オブジェクトも含めて全てのRendererを取得
        playerRenderers.AddRange(GetComponentsInChildren<Renderer>());

        transform.rotation = Quaternion.Euler(0, 90, 0);
        Physics.gravity = new Vector3(0, -20f, 0);
        hp = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Player();
        // 弾発射
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("joystick button 2"))
        {
            Bullet();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 敵に接触した場合の処理
        if (collision.gameObject.tag == "Enemy1" && !isInvincible)
        {
            Debug.Log("Player hit by Enemy1");
            hp -= collision.gameObject.GetComponent<Enemy1Script>().powerEnemy;
            rb.velocity = Vector3.zero;

            // 無敵時間をスタート
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
        //調整後のジャンプの処理
        // 地面にいるか確認
        //レイ
        Vector3 rayPosition = transform.position + new Vector3(0.0f, 0.8f, 0.0f);
        Ray ray = new Ray(rayPosition, Vector3.down);

        float distance = 0.9f;

        isGrounded = Physics.Raycast(ray, distance);
         

        // ジャンプ開始
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;

            // 現在のy速度をリセットしてジャンプ力を直接設定
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }

        // ジャンプ中にボタンを押し続けている場合
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey("joystick button 0")) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                // 高さが制限を超えないようにする
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

        // ボタンを離したとき
        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp("joystick button 0")))
        {
            isJumping = false;

            if (rb.velocity.y > jumpForce / 2)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce / 2, rb.velocity.z);  // 落下スピードを減らす
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

        //Debug.Log("ジャンプ速度: " + rb.velocity.y);
    }

    private void PlayerHp()
    {
        HpText.text = "HP " + hp;

        if (hp <= 0)
        {
            Debug.Log("Game Over");
            // ゲームオーバー処理をここに記述
            // 例: プレイヤーの動きを止める、リスポーンする、ゲームオーバー画面を表示する
            animator.SetTrigger("DEAD");
            rb.velocity = Vector3.zero;
            this.enabled = false;  // プレイヤー操作を無効にする
        }
    }

    private void Bullet()
    {
        bool bulletDirection = false;

        Vector3 position = this.transform.position;

        float stick = Input.GetAxisRaw("Vertical");

        //弾を上に打つか
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

        GameObject newbullet = Instantiate(bullet, position, Quaternion.identity); //弾を生成
        Rigidbody bulletRigidbody = newbullet.GetComponent<Rigidbody>();

        if (bulletDirection == false)
        {
            bulletRigidbody.AddForce(this.transform.forward * bulletSpeed); //キャラクターが向いている方向に弾に力を加える
        }
        else
        {
            bulletRigidbody.AddForce(transform.up * bulletSpeed); //上方向に弾に力を加える
        }

        Destroy(newbullet, 10); //10秒後に弾を消す
    }

    public void TakeDamage(float damage)
    {
        if (!isInvincible && hp > 0) // 無敵状態でなく、HPが0より大きい場合のみダメージ処理
        {
            hp -= damage;

            if (hp <= 0)
            {
                hp = 0; // HPを0で固定
                Debug.Log("Game Over");
                animator.SetTrigger("DEAD");
                rb.velocity = Vector3.zero;
                this.enabled = false; // プレイヤー操作を無効にする
            }
            else
            {
                Debug.Log($"Player took {damage} damage. Remaining HP: {hp}");
            }

            StartCoroutine(InvincibleTime()); // 無敵時間を開始
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
            // すべてのRendererを点滅させる
            foreach (Renderer renderer in playerRenderers)
            {
                if (renderer != null)
                {
                    renderer.enabled = !renderer.enabled;
                }
            }
            yield return new WaitForSeconds(blinkInterval);
        }

        // 無敵時間終了後、レイヤーと見た目を元に戻す
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
