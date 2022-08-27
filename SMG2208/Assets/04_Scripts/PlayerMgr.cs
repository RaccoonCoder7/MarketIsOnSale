using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : SingletonMono<PlayerMgr>
{
    public int hp;
    public PlayerState playerState;
    public float jumpPower;
    public Transform bulbTr;
    public int lerpSpeed;

    private Rigidbody2D rigid;
    private Coroutine detectBulbRoutine;
    private Coroutine moveToBulbRoutine;
    private float originGravityScale;

    public enum PlayerState
    {
        None,
        Idle,
        Jump,
        Rope,
        RopeJump,
        Dead
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        originGravityScale = rigid.gravityScale;
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
        rigid.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);
        playerState = PlayerState.Jump;
    }

    private void DetectBulb()
    {
        if (detectBulbRoutine != null) return;

        detectBulbRoutine = StartCoroutine(DetectBulbRoutine());
    }

    private void CancelDetectBulb()
    {
        if (detectBulbRoutine != null)
        {
            StopCoroutine(detectBulbRoutine);
            detectBulbRoutine = null;
        }

        detectBulbRoutine = null;
        playerState = PlayerState.None;
    }

    private void MoveToBulb()
    {
        if (moveToBulbRoutine != null) return;

        moveToBulbRoutine = StartCoroutine(MoveToBulbRoutine());
    }

    private void CancelMoveToBulb()
    {
        if (moveToBulbRoutine != null)
        {
            StopCoroutine(moveToBulbRoutine);
            moveToBulbRoutine = null;
        }

        rigid.gravityScale = originGravityScale;
        playerState = PlayerState.None;
    }

    private IEnumerator DetectBulbRoutine()
    {
        // TODO: 로프 발사 구현
        yield return new WaitForSeconds(0.1f);
        // TODO: bulb 오브젝트 세팅

        playerState = PlayerState.RopeJump;
        detectBulbRoutine = null;
    }

    private IEnumerator MoveToBulbRoutine()
    {
        rigid.gravityScale = 0;
        rigid.velocity = Vector2.zero;
        Vector3 targetPos = new Vector3(transform.position.x, bulbTr.position.y, 0);

        // TODO: 타겟의 x값을 가지고 lerpSpeed를 바꿔줘야 함
        while (targetPos.y * 0.9f > transform.position.y)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * lerpSpeed);
            yield return null;
        }

        rigid.gravityScale = originGravityScale;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);
        playerState = PlayerState.None;
        moveToBulbRoutine = null;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Ground"))
        {
            playerState = PlayerState.Idle;
        }
    }
}
