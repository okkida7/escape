using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Rigidbody rig;
    private void Start()
    {
        Camera.main.enabled = false;
    }

    private void FixedUpdate()
    {
        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");

        Vector3 t_direction = new Vector3(t_hmove, 0f, t_vmove);
        t_direction.Normalize();

        Vector3 t_targetVelocity = transform.TransformDirection(t_direction) * speed * Time.deltaTime;
        t_targetVelocity.y = rig.velocity.y;
        rig.velocity = t_targetVelocity;

    }
}