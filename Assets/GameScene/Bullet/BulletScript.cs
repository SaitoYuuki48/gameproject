using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BulletScript : MonoBehaviour
{
    private Collider objectCollider;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Renderer>().material.color = new Color32(0, 0, 0, 1); //�e�̐F�����ɂ���
        objectCollider = GetComponent<SphereCollider>();
        objectCollider.isTrigger = true; //Trigger�Ƃ��Ĉ���
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; //�d�͂𖳌��ɂ���
    }

    //void OnTriggerEnter(Collider collision)
    //{
    //    if (collision.gameObject.CompareTag("Floor")) //�^�O��Block�̃I�u�W�F�N�g�ƏՓ˂����ꍇ
    //    {
    //        Destroy(this.gameObject); //�e������
    //    }

    //    if (collision.gameObject.CompareTag("Wall")) //�^�O��Block�̃I�u�W�F�N�g�ƏՓ˂����ꍇ
    //    {
    //        Destroy(this.gameObject); //�e������
    //    }

    //    if (collision.gameObject.CompareTag("Ceiling")) //�^�O��Block�̃I�u�W�F�N�g�ƏՓ˂����ꍇ
    //    {
    //        Destroy(this.gameObject); //�e������
    //    }

    //    if (collision.gameObject.CompareTag("Enemy")) //�^�O��Enemy�̃I�u�W�F�N�g�ƏՓ˂����ꍇ
    //    {
    //        //Destroy(collision.gameObject); //�Փ˂������������
    //        Destroy(this.gameObject); //�e������
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    // �Փ˂����I�u�W�F�N�g��IDamageable�������Ă��邩�m�F
    //    IDamageable target = collision.gameObject.GetComponent<IDamageable>();

    //    if (target != null)
    //    {
    //        // IDamageable������΃_���[�W��^����
    //        target.TakeDamage();
    //    }

    //    // �e�͓��������������
    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter(Collider collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();

        if (target != null)
        {
            target.TakeDamage();
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
