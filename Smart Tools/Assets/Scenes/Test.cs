using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float speed = 10f;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        float posX = dirX * speed;
        //Debug.Log(posX);
        rb.velocity=new Vector3(posX, 0f, 0f);
        //rb.MovePosition(new Vector3(transform.position.x+dirX*speed*Time.deltaTime, 0.5f, 5f));
        //float posX = transform.position.x + dirX * speed * Time.deltaTime;
        //transform.position = new Vector2(posX, transform.position.y);
    }

}
