using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerMove playerMove;
    private UseItem playerItem;
    private CamerRay playerAction;
    private InventoryUpdate playerInventoryUpdate;
    private Inventory playerInventory;
    private TalkText playerTalkText;

    [SerializeField]private Vector3 Player_Dir;
    [SerializeField]private Vector2 Player_Rot;

    [SerializeField]private float Player_RunState;
    [SerializeField]private float Player_JumpState;
    [SerializeField]private float Player_FireState;
    [SerializeField]private float Player_CatchState;
    [SerializeField]private float Player_ActionState;
    [SerializeField]private float Player_DropState;
    [SerializeField]private float Player_InventoryIdx;

    [SerializeField]private bool Player_isRun;
    [SerializeField]private bool Player_isJump;
    [SerializeField]private bool Player_isUse;
    [SerializeField]private bool Player_isCatch;
    [SerializeField]private bool Player_isAction;
    [SerializeField]private bool Player_isDrop;
    
    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerItem = GetComponent<UseItem>();
        playerAction = transform.GetChild(0).GetComponent<CamerRay>();
        playerInventoryUpdate = GetComponent<InventoryUpdate>();
        playerInventory = GetComponent<Inventory>();
        playerTalkText = GameObject.Find("Subtitle").GetComponent<TalkText>();
    }
    void Update()
    {
        if (GameManager.G_instance.isGameover || !playerTalkText.talkout || GameManager.G_instance.AllStop|| InGameUIManager.instance.OnOptionsPlayerstop)
        {
            playerMove.Player_isRun = false;
            playerMove.Player_IsJump = false;
            playerItem.IsUse = false;
            playerAction.IsCatch = false;
            playerAction.IsAction = false;
            playerInventory.IsDrop = false;
            playerMove.PlayerDir = Vector3.zero;
            playerMove.PlayerRot = Vector2.zero;
            return;
        }
        CheckState();
        UpdateState();

    }

    private void UpdateState()
    {
        //이동 관련 업데이트
        playerMove.PlayerDir = Player_Dir;
        playerMove.PlayerRot = Player_Rot;
        playerMove.Player_isRun = Player_isRun;
        playerMove.Player_IsJump = Player_isJump;

        //아이템 관련 업데이트
        playerItem.IsUse = Player_isUse;
        playerInventory.IsDrop = Player_isDrop;

        //상호작용 관련 업데이트
        playerAction.IsCatch = Player_isCatch;
        playerAction.IsAction = Player_isAction;
    }

    private void CheckState()
    {
        if (Player_RunState > 0.1f && Player_Dir.z >= 0.7f) //달리기 입력감지
            Player_isRun = true;
        else
            Player_isRun = false;

        if (Player_FireState != 0) //총쏘기 입력 감지
            Player_isUse = true;
        else
            Player_isUse = false;

        if (Player_JumpState != 0) //점프 입력 감지
            Player_isJump = true;
        else
            Player_isJump = false;

        if(Player_CatchState != 0) //줍기 입력 감지
            Player_isCatch = true;
        else
            Player_isCatch= false;

        if(Player_ActionState != 0) //상호 작용 감지
            Player_isAction = true;
        else
            Player_isAction= false;

        if (Player_DropState != 0) //상호 작용 감지
            Player_isDrop = true;
        else
            Player_isDrop = false;
    }

    private void SendInventoryIdx(int idx)
    {
        playerInventoryUpdate.Inventory_Idx = idx;
        playerItem.Inventory_Idx = idx;
    }

    private void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        Player_Dir = new Vector3(dir.x, 0f, dir.y).normalized;
    }

    private void OnLook(InputValue value)
    {
        Player_Rot = value.Get<Vector2>();
    }

    private void OnRun(InputValue value)
    {
        Player_RunState = value.Get<float>();
    }

    private void OnJump(InputValue value)
    {
        Player_JumpState = value.Get<float>();
    }

    private void OnFire(InputValue value)
    {
        Player_FireState = value.Get<float>();
    }

    private void OnCatch(InputValue value)
    {
        Player_CatchState = value.Get<float>();
    }

    private void OnAction(InputValue value)
    {
        Player_ActionState = value.Get<float>();
    }

    private void OnDrop(InputValue value)
    {
        Player_DropState = value.Get<float>();
    }
    private void OnOptions(InputValue value)
    {
        
        if (value.Get<float>() == 1f)
        {
            InGameUIManager.instance.OnOption_S_Toggle();
        }
       
        
    }
    private void OnInventory(InputValue value)
    {
        var key = Keyboard.current;

        if (key.digit1Key.wasPressedThisFrame)
            SendInventoryIdx(0);
        else if (key.digit2Key.wasPressedThisFrame)
            SendInventoryIdx(1);
        else if (key.digit3Key.wasPressedThisFrame)
            SendInventoryIdx(2);
        else if (key.digit4Key.wasPressedThisFrame)
            SendInventoryIdx(3);
    }
}
