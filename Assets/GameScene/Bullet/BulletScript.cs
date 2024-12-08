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
        //GetComponent<Renderer>().material.color = new Color32(0, 0, 0, 1); //弾の色を黒にする
        objectCollider = GetComponent<SphereCollider>();
        objectCollider.isTrigger = true; //Triggerとして扱う
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; //重力を無効にする
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Floor")) //タグがBlockのオブジェクトと衝突した場合
        {
            Destroy(this.gameObject); //弾を消す
        }

        if (collision.gameObject.CompareTag("Wall")) //タグがBlockのオブジェクトと衝突した場合
        {
            Destroy(this.gameObject); //弾を消す
        }

        if (collision.gameObject.CompareTag("Ceiling")) //タグがBlockのオブジェクトと衝突した場合
        {
            Destroy(this.gameObject); //弾を消す
        }

        if (collision.gameObject.CompareTag("Enemy")) //タグがEnemyのオブジェクトと衝突した場合
        {
            Destroy(collision.gameObject); //衝突した相手を消す
            Destroy(this.gameObject); //弾を消す
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
