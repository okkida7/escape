using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Rigidbody rig;
    public AudioSource footstepAudio;  // Audio source for footstep sounds
    public AudioClip footstepClip;     // The footstep sound clip
    public float footstepInterval = 0.5f; // Interval between footstep sounds

    private float footstepCooldown;   // Timer for footstep sound interval

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

        // Play footstep sound
        PlayFootstepSounds(t_hmove, t_vmove);
    }

    // Play footstep sounds based on movement and a cooldown
    private void PlayFootstepSounds(float hmove, float vmove)
    {
        // Check if player is moving
        if (hmove != 0 || vmove != 0)
        {
            if (footstepCooldown <= 0)
            {
                footstepAudio.PlayOneShot(footstepClip);
                footstepCooldown = footstepInterval;
            }
            footstepCooldown -= Time.fixedDeltaTime;
        }
    }
}
