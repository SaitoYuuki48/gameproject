using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Script : MonoBehaviour
{
    public Rigidbody rb;
    private float moveSpeed;
    private Vector3 velocity;
    private bool isBlock = false;

    public float powerEnemy = 1; //çUåÇóÕ

    // Start is called before the first frame update
    void Start()
    {
        velocity.x += 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Enemy1Move();
        //transform.Translate(new Vector3(0.01f, 0, 0));

    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Wall")
        //{
        //    transform.Rotate(new Vector3(0, 180, 0));
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.AddForce(velocity, ForceMode.VelocityChange);
    }

    private void Enemy1Move()
    {
        velocity.x += 5.0f;

        //ÉåÉC
        Vector3 rayPosition = transform.position + new Vector3(0.5f, 0.0f, 0.0f);
        Ray ray = new Ray(rayPosition, Vector3.right);

        float distance = 0.8f;

        isBlock = Physics.Raycast(ray, distance);

        if (isBlock == true)
        {
            Debug.DrawRay(rayPosition, Vector3.right * distance, Color.red);
            //velocity.x *= -1;
        }
        else
        {
            Debug.DrawRay(rayPosition, Vector3.right * distance, Color.yellow);
            
        }

        //transform.position += transform.rotation * velocity;
    }


}
