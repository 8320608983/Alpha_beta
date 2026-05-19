using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_PlayerController : MonoBehaviour
{
    public float forwardSpeed = 8;
    public float speedIncreaseRate = 0.2f;

    public int laneCount = 3;
    public float laneDistance = 2f;
    public float laneChangeSpeed = 12f;

    public float jumpForce = 10f;
    public float gravity = -20f;

    CharacterController cc;
    Vector3 velocity;
    int currentLane;
    bool canMove = false;
    private Vector3 startPos;

    
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        GameEvents.OnGameReset += ResetPlayer;
        GameEvents.OnGameEnd += DisableMovement;
        GameEvents.OnGameStart += EnableMovement;
    }

    private void ResetPlayer()
    {
        cc.enabled = false;

        transform.position = startPos;
        velocity = Vector3.zero;
         
        currentLane = laneCount / 2;

        cc.enabled = true;
    }

    private void EnableMovement()
    {
        canMove = true;
    }

    private void DisableMovement()
    {
        canMove = false;
    }

    private void OnDisable()
    {
        GameEvents.OnGameReset -= ResetPlayer;
        GameEvents.OnGameEnd -= DisableMovement;
        GameEvents.OnGameStart -= EnableMovement;

    }
    private void Start()
    {
        startPos = transform.position;
        currentLane = laneCount / 2;
    }
    private void Update()
    {
        if (!canMove) return;
        forwardSpeed = forwardSpeed + speedIncreaseRate * Time.deltaTime;
        Vector3 move = Vector3.forward * forwardSpeed;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(1);
        }

        float targetx = GetLaneX(currentLane);
        float Diffx = targetx - transform.position.x;
        move.x = Diffx * laneChangeSpeed;
        if (cc.isGrounded)
        {
            velocity.y = -1f;

            if (Input.GetKeyDown(KeyCode.Space))
                velocity.y = jumpForce;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        cc.Move((move + velocity)* Time.deltaTime);

    }
    void ChangeLane(int lane)
    {
        currentLane = currentLane + lane;
        currentLane = Mathf.Clamp(currentLane, 0, laneCount - 1);

    }
    float GetLaneX(int laneIndex)
    {
        float MiddleLane = (laneCount - 1) / 2;
        return (laneIndex - MiddleLane) * laneDistance;
    }
}
