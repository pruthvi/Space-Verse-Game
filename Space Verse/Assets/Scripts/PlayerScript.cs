using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity = this.transform.forward * speed;
        }

               if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, 10, 0) * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, -10, 0) * Time.deltaTime * speed, Space.World);
        }
    }
}
