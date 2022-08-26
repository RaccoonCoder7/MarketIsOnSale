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
                break;
            case PlayerState.Rope:
                break;
            case PlayerState.RopeJump:
                break;
            case PlayerState.Dead:
                break;
        }
    }

    public void AddHP(int damage)
    {
        hp = hp - damage;
    }
}
