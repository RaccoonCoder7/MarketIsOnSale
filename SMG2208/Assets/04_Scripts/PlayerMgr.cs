using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : SingletonMono<PlayerMgr>
{
    public int hp;
    public PlayerState playerState;
    public float jumpPower;
    public float ropeReachSpeed;
    public float ropeReachLimit;
    public float goToBulbSpeed;
    public float detectStartDegree;
    public float detectEndDegree;
    public Transform bulbTr;
    public List<Transform> bulbTrList;
    public int bulbWaitDistance;
    public GameObject mouse;

    private Rigidbody2D rigid;
    private Coroutine detectBulbRoutine;
    private Coroutine moveToBulbRoutine;
    private float originGravityScale;
    private float currRopeReach;
    private LineRenderer lr;

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
        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.5f;
        lr.endWidth = 0.5f;
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, Vector3.zero);
        mouse.SetActive(false);
        rigid = GetComponent<Rigidbody2D>();
        originGravityScale = rigid.gravityScale;
        rigid.gravityScale = 0;
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (hp <= 0)
        {
            playerState = PlayerState.Dead;
            this.gameObject.SetActive(false);
        }
        if (GameMgr.In.totalTime >= GameMgr.In.gameLimitTime)
        {
            this.gameObject.SetActive(false);
        }
        if (GameMgr.In.gameState == GameMgr.GameState.Play)
        {
            rigid.WakeUp();
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
                        playerState = PlayerState.None;
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
                    GameMgr.In.gameState = GameMgr.GameState.None;
                    SceneMgr.In.ChangeScene(GameMgr.In.EndingSceneName);
                    AudioMgr.In.StopPlay();
                    lr.SetPosition(0, Vector3.zero);
                    lr.SetPosition(1, Vector3.zero);
                    mouse.SetActive(false);
                    break;
            }
        }
        else
        {
            rigid.Sleep();
        }
    }

    public void AddDamage(int damage)
    {
        hp = hp - damage;
    }

    public void InitPlayer()
    {
        rigid.gravityScale = originGravityScale;
        this.gameObject.SetActive(true);
    }

    private void Jump()
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);
        playerState = PlayerState.Jump;
        AudioMgr.In.PlayOneShot(1);
    }

    private void DetectBulb()
    {
        // var startv2 = DegreeToVector2(detectStartDegree);
        // var endv2 = DegreeToVector2(detectEndDegree);
        // var startv3 = new Vector3(startv2.x, startv2.y, 0);
        // var endv3 = new Vector3(endv2.x, endv2.y, 0);
        // Debug.DrawRay(transform.position, startv3.normalized * ropeReachLimit, Color.blue);
        // Debug.DrawRay(transform.position, endv3.normalized * ropeReachLimit, Color.blue);

        if (detectBulbRoutine != null) return;

        detectBulbRoutine = StartCoroutine(DetectBulbRoutine());
        AudioMgr.In.PlayOneShot(2);
    }

    private void CancelDetectBulb()
    {
        if (detectBulbRoutine != null)
        {
            StopCoroutine(detectBulbRoutine);
            detectBulbRoutine = null;
        }

        bulbTr = null;
        currRopeReach = 0;
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, Vector3.zero);
        mouse.SetActive(false);
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

        bulbTr = null;
        rigid.gravityScale = originGravityScale;
        playerState = PlayerState.None;
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, Vector3.zero);
        mouse.SetActive(false);
    }

    private Transform GetTargetBulbTr(float reach)
    {
        foreach (var tr in bulbTrList)
        {
            if (IsTargetted(tr, reach))
            {
                currRopeReach = 0;
                return tr;
            }
        }
        return null;
    }

    private bool IsTargetted(Transform targetTr, float reach)
    {
        Vector2 targetPos = new Vector2(targetTr.position.x, targetTr.position.y);
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);

        Vector2 v2 = targetPos - playerPos;
        float degree = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;

        // var tempDist = new Vector3(v2.x, v2.y, 0);
        // Debug.DrawRay(playerPos, tempDist.normalized * reach, Color.red);

        if (degree < detectStartDegree || detectEndDegree < degree)
        {
            return false;
        }

        var dist = Vector2.Distance(targetPos, playerPos);

        return reach >= dist;
    }

    private IEnumerator DetectBulbRoutine()
    {
        while (bulbTr == null)
        {
            var ropePos = GetRopePos();
            var fixedRopePos = transform.position + ropePos;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, fixedRopePos);
            mouse.SetActive(true);
            mouse.transform.position = fixedRopePos;

            currRopeReach += Time.deltaTime * ropeReachSpeed;
            if (currRopeReach >= ropeReachLimit)
            {
                CancelDetectBulb();
                playerState = PlayerState.None;
                yield break;
            }
            bulbTr = GetTargetBulbTr(currRopeReach);
            yield return null;
        }

        currRopeReach = 0;
        playerState = PlayerState.RopeJump;
        detectBulbRoutine = null;
    }

    private Vector3 GetRopePos()
    {
        var v2 = DegreeToVector2(detectEndDegree - detectStartDegree);
        var v3 = new Vector3(v2.x, v2.y, 0);
        return v3.normalized * currRopeReach;
    }

    private IEnumerator MoveToBulbRoutine()
    {
        rigid.gravityScale = 0;
        rigid.velocity = Vector2.zero;
        Vector3 targetPos = new Vector3(transform.position.x, bulbTr.position.y, 0);
        float dist = Vector2.Distance(new Vector2(bulbTr.position.x, 0)
                                    , new Vector2(transform.position.x, 0));

        while (targetPos.y * 0.925f > transform.position.y || dist > bulbWaitDistance)
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, bulbTr.position);
            mouse.transform.position = bulbTr.position;
            dist = Vector2.Distance(new Vector2(bulbTr.position.x, 0)
                                , new Vector2(transform.position.x, 0));
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * goToBulbSpeed / dist);
            yield return null;
        }

        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, Vector3.zero);
        mouse.SetActive(false);
        rigid.gravityScale = originGravityScale;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector3.up * jumpPower / 5, ForceMode2D.Impulse);
        bulbTr = null;
        playerState = PlayerState.Idle;
        moveToBulbRoutine = null;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Ground"))
        {
            CancelDetectBulb();
            playerState = PlayerState.Idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("DamageObj"))
        {
            var spawnObj = other.GetComponent<SpawnObject>();
            AddDamage(spawnObj.damage);
            spawnObj.DestroyBulb();
        }
        else if (other.gameObject.tag.Equals("Item"))
        {
            // TODO: Item
            GameMgr.In.AddItem(1);
            AudioMgr.In.PlayOneShot(3);
            Destroy(other.gameObject);
        }
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    // Reset the player
    public void ResetPlayer()
    {
        playerState = PlayerState.None;
        hp = 3;
        transform.position = new Vector2(transform.position.x, -1);
        this.gameObject.SetActive(false);
    }
}
