using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Script : MonoBehaviour, IDamageable
{
    public float moveSpeed = 3f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.right;  // 初期進行方向
    private Vector3 surfaceNormal;                  // 壁や床の法線方向
    private bool isOnSurface = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;  // 重力を無効に
        rb.isKinematic = false;  // 動かせる状態に
        rb.drag = 0;  // 減速しない
        rb.angularDrag = 0;
    }

    void FixedUpdate()
    {
        StickToSurface();
        if (isOnSurface)
        {
            MoveAlongSurface();
        }
    }

    // 壁や天井に沿って進む
    void MoveAlongSurface()
    {
        Vector3 move = Vector3.ProjectOnPlane(moveDirection, surfaceNormal).normalized;
        rb.velocity = move * moveSpeed;

        //Vector3 move = Vector3.ProjectOnPlane(moveDirection, surfaceNormal).normalized;
        //transform.Translate(move * moveSpeed * Time.deltaTime);
    }

    // 壁や天井に張り付く処理
    void StickToSurface()
    {
        RaycastHit hit;

        // Raycastを複数方向に飛ばして検出する
        if (Physics.Raycast(transform.position, -transform.up, out hit, 3f, groundLayer) ||  // 下方向
            Physics.Raycast(transform.position, -transform.right, out hit, 3f, groundLayer) ||  // 右方向
            Physics.Raycast(transform.position, transform.right, out hit, 3f, groundLayer) ||  // 左方向
            Physics.Raycast(transform.position, transform.up, out hit, 3f, groundLayer))  // 上方向
        {
            isOnSurface = true;
            surfaceNormal = hit.normal;

            // 壁の法線に合わせて回転
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            // 壁に強制的に押し付ける処理
            rb.position = hit.point + (hit.normal * 0.3f);  // 距離を調整 (0.3f〜0.5fで調整)
        }
        else
        {
            isOnSurface = false;
        }
    }

    // 壁や天井にぶつかったら進行方向を反射させる
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // 反射方向を計算
            Vector3 reflectDir = Vector3.Reflect(moveDirection, collision.contacts[0].normal);
            moveDirection = reflectDir.normalized;

            // 衝突時に法線方向に押し出す
            rb.position += collision.contacts[0].normal * 0.2f;  // 埋まるのを防止
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (-transform.up * 3f));  // 下方向
        Gizmos.DrawLine(transform.position, transform.position + (-transform.right * 3f));  // 右方向
        Gizmos.DrawLine(transform.position, transform.position + (transform.right * 3f));  // 左方向
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * 3f));  // 上方向
    }

    public void TakeDamage()
    {
        Debug.Log($"{gameObject.name} はダメージを受けた！");
        Destroy(gameObject);  // 当たったらオブジェクトを消す
    }
}
