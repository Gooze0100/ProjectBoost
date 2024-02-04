using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    /*
    How to structure code:
    PARAMETERS - for tuning, typically set in the editor
    CACHE - e.g. references for readability or speed e.g. bool isAlive
    STATE - private instance (member) variables

    Then Start, Update methods
    Then public functions
    Then private methods
    */

    [SerializeField] float rocketThrust = 100f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip thrustingSound;
    [SerializeField] ParticleSystem rightParticles;
    [SerializeField] ParticleSystem leftParticles;
    [SerializeField] ParticleSystem boosterParticles;

    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    void RotateLeft()
    {
        RotateRocket(rotationThrust);
        leftParticles.Play();
    }

    void RotateRight()
    {
        RotateRocket(-rotationThrust);
        rightParticles.Play();
    }
    void StopRotating()
    {
        leftParticles.Stop();
        rightParticles.Stop();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.W))
        {
            StartingToThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    void StartingToThrusting()
    {
        // Vector3 is direction and magnitude
        // Vector(0,1,0) same as Vector3.up
        // Time.deltaTime frames from last frame
        rb.AddRelativeForce(Vector3.up * rocketThrust * Time.deltaTime);
        boosterParticles.Play();

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustingSound);
        }
    }

    void StopThrusting()
    {
        boosterParticles.Stop();
        audioSource.Stop();
    }

    void RotateRocket(float rotationThisFrame)
    {
        //freezing rotation so we can manually rotate
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        // unfreezing rotation so the physics system can take over
        rb.freezeRotation = false;
    }
}
