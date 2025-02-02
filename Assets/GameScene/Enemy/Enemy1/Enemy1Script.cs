using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Script : MonoBehaviour, IDamageable
{
    public Rigidbody rb;
    private float moveSpeed = 5.0f;

    public float powerEnemy = 1; //攻撃力
    public float hp = 5; //体力

    private LayerMask groundLayer;  // Groundレイヤー
    private float rayDistance = 1.0f;  // Rayの距離
    private float rayOffset = 0.5f;   // Rayのオフセット距離

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundLayer = LayerMask.GetMask("Ground");  // Groundレイヤーを取得
        rb.isKinematic = true;  // 物理演算を有効
        rb.useGravity = false;   // 重力は使わず移動
        hp = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Enemy1Move();
        EnemyHP();
    }

    private void Enemy1Move()
    {
        // Rayの発射位置を少し前にずらす
        Vector3 rayPosition = transform.position + transform.right * rayOffset;  // 右側からRay発射
        Ray ray = new Ray(rayPosition, -transform.right);  // 左方向にRayを飛ばす

        // 自分自身のコライダーを無視するRaycast
        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, groundLayer, QueryTriggerInteraction.Ignore);
        bool hitObstacle = false;

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject != gameObject)  // 自分自身は無視
            {
                hitObstacle = true;
                break;
            }
        }

        // 障害物があれば回転、なければ移動
        if (hitObstacle)
        {
            Debug.DrawRay(rayPosition, -transform.right * rayDistance, Color.red);
            RotateEnemy();
        }
        else
        {
            Debug.DrawRay(rayPosition, -transform.right * rayDistance, Color.green);
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
    }

    // 壁や床に衝突したら向きを変える
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            RotateEnemy();
        }
    }

    // 敵の方向転換
    private void RotateEnemy()
    {
        transform.Rotate(new Vector3(0, 180, 0));
    }

    private void EnemyHP()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage()
    {
        Debug.Log($"{gameObject.name} はダメージを受けた！");
        hp--;  // 当たったらHPを減らす
    }

}
