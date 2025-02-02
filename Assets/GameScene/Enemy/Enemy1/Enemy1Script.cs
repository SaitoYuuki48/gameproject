using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Script : MonoBehaviour, IDamageable
{
    public Rigidbody rb;
    private float moveSpeed = 5.0f;

    public float powerEnemy = 1; //�U����
    public float hp = 5; //�̗�

    private LayerMask groundLayer;  // Ground���C���[
    private float rayDistance = 1.0f;  // Ray�̋���
    private float rayOffset = 0.5f;   // Ray�̃I�t�Z�b�g����

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundLayer = LayerMask.GetMask("Ground");  // Ground���C���[���擾
        rb.isKinematic = true;  // �������Z��L��
        rb.useGravity = false;   // �d�͎͂g�킸�ړ�
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
        // Ray�̔��ˈʒu�������O�ɂ��炷
        Vector3 rayPosition = transform.position + transform.right * rayOffset;  // �E������Ray����
        Ray ray = new Ray(rayPosition, -transform.right);  // ��������Ray���΂�

        // �������g�̃R���C�_�[�𖳎�����Raycast
        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, groundLayer, QueryTriggerInteraction.Ignore);
        bool hitObstacle = false;

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject != gameObject)  // �������g�͖���
            {
                hitObstacle = true;
                break;
            }
        }

        // ��Q��������Ή�]�A�Ȃ���Έړ�
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

    // �ǂ⏰�ɏՓ˂����������ς���
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            RotateEnemy();
        }
    }

    // �G�̕����]��
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
        Debug.Log($"{gameObject.name} �̓_���[�W���󂯂��I");
        hp--;  // ����������HP�����炷
    }

}
