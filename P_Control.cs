using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Control : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbody;

    public Vector2 inputAxis;
    public float runSpeed = 1;
    public float sprintSpeedMult = 1.5f;
    [SerializeField]
    private float xVel = 0;
    [SerializeField]
    private float xCurTime = 0;
    public float xLerpTime = 0.5f;
    public float stopLerpTime = 0.1f;
    public float jumpPower = 5;
    public float gravityScale = 5;
    private bool isMove = false;
    private bool isSprint = false;
    [SerializeField]
    private bool isJump = false;
    private Vector2 prevPos;
    [SerializeField]
    private bool isGround = true;
    [SerializeField]
    private Collider2D headCollider;

    public bool isWallSlide = false;
    private bool isWatchingRight = true;
    public float wallJumpPower = 5;

    private bool isAttacking = false;
    private bool isDead = false;
    private bool isClimb = false;

    public bool IsJump { get { return isJump; } }

    public bool IsGround { get { return isGround; } }

    public bool IsAttacking { get { return isAttacking; } }

    public bool IsSprint { get { return isSprint; } }

    public bool IsWatchingRight { get { return isWatchingRight; } }

    private void ClimbMove()
    {
        if (!isClimb)
            return;

        transform.Translate(Vector3.up * Time.deltaTime);
    }

    public void SetClimb(bool value)
    {
        rigidbody.simulated = !value;
        isClimb = value;
    }

    public void SetSprint(bool value)
    {
        if (isJump && !value)
            return;
        isSprint = value;
    }

    public void DoAttack()
    {
        if (isDead)
            return;

        if (isAttacking || isWallSlide)
            return;

        isAttacking = true;

        HitBox hitBox = GetComponentInChildren<HitBox>();
        if(hitBox != null)
            hitBox.AttackCheck();
        if (animator != null)
            animator.SetTrigger("Attack");

    }

    private void AttackEndCheck()
    {
        if (!isAttacking)
            return;
        
        if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(0.4f, 0.1f));
    }

    private void GroundCheck()
    {
        Vector2 size = new Vector2(0.4f, 0.1f);
        Collider2D hit = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask("Terrain"));
        //만약 충돌체가 있다면
        if (hit != null)
        {
            Debug.Log(hit.name);
            if(!isGround)
            {
                isGround = true;
                SetSprint(false);
            }
        }
        else
            isGround = false;
    }

    private void JumpEndCheck()
    {
        if (isJump)
        {
            if(prevPos.y > transform.position.y)
            {
                isJump = false;
            }
        }
        prevPos = transform.position;
    }

    public void DoJump()
    {
        if (isDead)
            return;

        if (isJump)
            return;

        if (isWallSlide)
        {
            Vector2 jumpDirection = Vector2.up * 3;
            if (isWatchingRight)
                jumpDirection += Vector2.left;
            else
                jumpDirection += Vector2.right;
            xVel = 0f;
            ExcuteJump((jumpDirection.normalized) * wallJumpPower);
            RotateDirection(!isWatchingRight);
            return;
        }

        if (!isGround)
            return;

        if (inputAxis.y < 0)
        {
            //아래점프
            Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector3.down, 0.1f, LayerMask.GetMask("Terrain"));
            if(rayHit.collider != null && rayHit.collider.transform.tag == "Platform")
            {
                print("downJump");
                Collider2D bodyCollider = GetComponent<Collider2D>();
                bodyCollider.isTrigger = true;
                StartCoroutine(UtillHelper.ReActiveCollider(bodyCollider, 0.2f));
                return;
            }
        }

        ExcuteJump(Vector2.up * jumpPower);
    }

    private void ExcuteJump(Vector2 forceDirection)
    {
        isJump = true;
        isGround = false;
        rigidbody.AddForce(forceDirection, ForceMode2D.Impulse);
        if (animator != null)
            animator.SetTrigger("Jump");
    }

    private void CalVelX()
    {
        float goal = 0;
        if (inputAxis.x > 0)
            goal = runSpeed;
        else if (inputAxis.x < 0)
            goal = -runSpeed;
        if (isSprint)
            goal *= sprintSpeedMult;
        goal *= 0.1f;
        if (xVel == goal)
            return;

        if (goal == 0)
            xVel = LerpLogic(xVel, goal, stopLerpTime, xCurTime, out xCurTime);
        else
            xVel = LerpLogic(xVel, goal, xLerpTime, xCurTime, out xCurTime);
    }

    private float LerpLogic(float target, float goal, float lerpTime, float curTime, out float curTimeOut)
    {
        curTime += Time.deltaTime;
        if (curTime >= lerpTime)
            curTime = lerpTime;
        float t = curTime / lerpTime;
        //t = t * t;
        target = Mathf.Lerp(target, goal, t);
        if (curTime >= lerpTime)
        {
            curTime = 0;
            target = goal;
        }
        curTimeOut = curTime;
        return target;
    }

    public void Move()
    {
        if(isWallSlide)
        {
            if (inputAxis.x >= 0 && isWatchingRight)
                return;
            else if (inputAxis.x <= 0 && !isWatchingRight)
                return;
        }

        Vector2 moveAxisX = new Vector2(xVel * Time.deltaTime * 45, 0);
        transform.Translate(moveAxisX);
        if (xVel != 0)
            isMove = true;
        else
            isMove = false;
    }

    private void SetPlayerAnimation()
    {
        if (animator == null)
            return;

        if (isDead)
        {
            animator.SetBool("IsDead", true);
            return;
        }

        if (xVel > 0)
            RotateDirection(true);
        else if (xVel < 0)
            RotateDirection(false);
        animator.SetBool("IsMove", isMove);
        animator.SetBool("IsWallSlide", isWallSlide);
        animator.SetBool("IsClimb", isClimb);
    }

    public void RotateDirection(bool isRight)
    {
        float direction = 0;
        if (!isRight)
            direction = 180;
        animator.transform.rotation = Quaternion.Euler(0, direction, 0);
        isWatchingRight = isRight;
    }

    public void Dead()
    {
        isDead = true;
        rigidbody.velocity = Vector2.zero;
        GameManager.Instance.EndStage(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SetPlayerAnimation();
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        CalVelX();
        GroundCheck();
        Move();
        ClimbMove();
        JumpEndCheck();
        AttackEndCheck();
    }
}
