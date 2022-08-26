using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : SingletonMono<PlayerMgr>
{
    public int hp;
    public PlayerState playerState;

    public enum PlayerState
    {
        None,
        Idle,
        Jump,
        Rope,
        RopeJump,
        Dead
    }

    void Update()
    {
        switch (playerState)
        {
            case PlayerState.None:
                break;
            case PlayerState.Idle:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
                break;
            case PlayerState.Jump:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    playerState = PlayerState.Rope;
                }
                break;
            case PlayerState.Rope:
                if (Input.GetKey(KeyCode.Space))
                {
                    DetectBulb();
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    CancelDetectBulb();
                }
                break;
            case PlayerState.RopeJump:
                if (Input.GetKey(KeyCode.Space))
                {
                    MoveToBulb();
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    CancelMoveToBulb();
                }
                break;
            case PlayerState.Dead:
                break;
        }
    }

    public void AddHP(int damage)
    {
        hp = hp - damage;
    }

    private void Jump()
    {
        // TODO: Y값 조정
        playerState = PlayerState.Rope;
    }

    private void DetectBulb()
    {
        // TODO: 로프 발사
    }

    private void CancelDetectBulb()
    {
        // TODO: 로프 발사 취소
        playerState = PlayerState.None;
    }

    private void MoveToBulb()
    {
        // TODO: Y값 변경
    }

    private void CancelMoveToBulb()
    {
        // TODO: Y값 변경 취소
        playerState = PlayerState.None;
    }
}
