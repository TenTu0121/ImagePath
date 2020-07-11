using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform target;

    private Vector3 offset;

    private Vector2 velocity;

    private void Update()
    {
        if (target == null && GameObject.FindWithTag("Player"))
        {
            target = GameObject.FindWithTag("Player").transform;
            offset = target.position - transform.position;
        }   
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            float posX = Mathf.SmoothDamp(transform.position.x, target.position.x- offset.x, ref velocity.x, 0.02f);
            float posY = Mathf.SmoothDamp(transform.position.y, target.position.y - offset.y, ref velocity.y, 0.02f);

            if (posY >= transform.position.y)
            {
                transform.position = new Vector3(posX, posY, transform.position.z);
            }
        }
    }
}
