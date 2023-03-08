using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Input : MonoBehaviour
{
    public bool isTouchInput = false;
    private P_Control player;

    private float leftTapTime = 0f;
    public float leftTapTimeLimit = 0.2f;

    private bool isLeftTouch = false;
    private bool isRightTouch = false;
    private Coroutine leftTap;
    private Vector2 rightTouchPos;
    private Vector2 leftTouchPos;
    public float leftSwipeLength = 1f;
    [SerializeField]
    private VirtualJoystick joystick;

    private bool IsTouchRight(Touch touchInput)
    {
        Vector3 point = Camera.main.ScreenToViewportPoint(touchInput.position);
        if (point.x > 0.5)
            return true;

        return false;
    }

    private void ExcuteLeftTouch(Touch touchInput)
    {
        if (touchInput.phase == TouchPhase.Began && !isLeftTouch)
        {
            isLeftTouch = true;
            if (leftTap != null)
                StopCoroutine(leftTap);
            leftTouchPos = touchInput.position;
            leftTap = StartCoroutine(IExcuteLeftTouch());
        }

        if (touchInput.phase == TouchPhase.Moved || touchInput.phase == TouchPhase.Stationary)
        {
            if(leftTapTime > leftTapTimeLimit)
                player.SetSprint(true);
            Vector3 point = Camera.main.ScreenToViewportPoint(touchInput.position);
            if (touchInput.position.y > leftTouchPos.y && (touchInput.position - leftTouchPos).magnitude > leftSwipeLength)
            {
                player.DoJump();
                leftTouchPos = touchInput.position;
                EndLeftTouch(touchInput, false);
            }
            if (point.x > 0.49)
                EndLeftTouch(touchInput);
        }

        if (touchInput.phase == TouchPhase.Ended)
        {
            EndLeftTouch(touchInput);
        }
    }

    private void EndLeftTouch(Touch touchInput, bool attackCheck = true)
    {
        if (!isLeftTouch)
            return;

        isLeftTouch = false;
        StopCoroutine(leftTap);
        
        if (leftTapTime < leftTapTimeLimit && attackCheck)
            player.DoAttack();
        player.SetSprint(false);
    }

    private IEnumerator IExcuteLeftTouch()
    {
        leftTapTime = 0;
        while(leftTapTime < leftTapTimeLimit)
        {
            leftTapTime += Time.deltaTime;
            yield return null;
        }
    }

    private void ExcuteRightTouch(Touch touchInput)
    {
        if (touchInput.phase == TouchPhase.Began && !isRightTouch)
        {
            isRightTouch = true;
            rightTouchPos = touchInput.position;
            if (joystick != null)
                joystick.SetActive(true, touchInput.position);
        }

        if(touchInput.phase == TouchPhase.Moved || touchInput.phase == TouchPhase.Stationary && isRightTouch)
        {
            Vector3 point = Camera.main.ScreenToViewportPoint(touchInput.position);
            if (point.x < 0.51)
                EndRightTouch(touchInput);

            Vector2 curPos = touchInput.position;
            Vector2 directionalInput = curPos - rightTouchPos;
            if (directionalInput.y < -150)
                directionalInput.x = 0f;
            else
                directionalInput.y = 0;
            player.inputAxis = directionalInput;
            if (joystick != null)
                joystick.SetHandlePos(touchInput.position, rightTouchPos);
        }

        if (touchInput.phase == TouchPhase.Ended)
        {
            EndRightTouch(touchInput);
        }
    }

    private void EndRightTouch(Touch touchInput)
    {
        if (!isRightTouch)
            return;

        isRightTouch = false;
        if (joystick != null)
            joystick.SetActive(false, touchInput.position);
    }

    private void TouchInput()
    {
        Touch[] touchInputs = Input.touches;
        foreach(Touch touchInput in touchInputs)
        {
            if (IsTouchRight(touchInput))
                ExcuteRightTouch(touchInput);
            else
                ExcuteLeftTouch(touchInput);
        }

        if (!isRightTouch)
            player.inputAxis = Vector2.zero;
    }

    private void KeyBoardInput()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.inputAxis = directionalInput;
        player.SetSprint(Input.GetKey(KeyCode.LeftShift));
        if (Input.GetKeyDown(KeyCode.Space))
            player.DoJump();
        if (Input.GetKeyDown(KeyCode.Z))
            player.DoAttack();
    }


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<P_Control>();
        joystick = FindObjectOfType<VirtualJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsPause)
            return;

        if (isTouchInput)
            TouchInput();
        else
            KeyBoardInput();

    }
}
