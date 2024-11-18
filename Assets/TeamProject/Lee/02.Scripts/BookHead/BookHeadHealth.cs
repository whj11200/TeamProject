using System.Collections;
using UnityEngine;

public class BookHeadHealth : CretureUpdate
{
    private Animator BookHead_Animator;
    private BookHeadAI BookHead_State;
    private AudioClip BulletHit;
    private AudioClip B_Hit;

    private readonly string RespawnObj = "Respawn";
    private readonly string DieTrigger = "Die";
    private readonly string ResetBool = "Reset";

    public int BookHeadClip_Idx = 0;

    void Awake()
    {
        BookHead_Animator = GetComponent<Animator>();
        BookHead_State = GetComponent<BookHeadAI>();

        BulletHit = Resources.Load<AudioClip>("Sound/BookHead/BulletHit");
        B_Hit = Resources.Load<AudioClip>("Sound/BookHead/B_Hit");
    }
    protected override void OnEnable()
    {
        base.OnEnable(); // 살아날 때마다 bool변수 초기화, hp 초기화
        BookHead_Animator.SetBool(ResetBool, false);
        BookHead_State.state = BookHeadAI.State.IDLE; //살아날때 초기화
        BookHead_State.BookHead_isDie = false; //살아날때 초기화, AI에 die 업데이트
        dead = false; //살아날때 초기화, creture에 die 업데이트
    }

    public override void OnDamage(object[] param)
    {
        InGameSoundManager.instance.ActiveSound(gameObject, B_Hit, 6.0f, true, false, false, 1, 1.0f); //괴물 비명소리
        base.OnDamage(param);
    }
    public override void Die()
    {
        base.Die();
        InGameSoundManager.instance.EditSoundBox($"B_Idle{BookHeadClip_Idx}", false); //죽을 때마다 할당된 사운드 박스제거
        InGameSoundManager.instance.Data.Remove($"B_Idle{BookHeadClip_Idx}"); //죽으면 사운드 박스 할당할 때 사용한 key값 제거.

        BookHead_Animator.SetTrigger(DieTrigger);

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        BookHead_State.state = BookHeadAI.State.RESPAWN;
        yield return new WaitForSeconds(3.5f); //3.5초 뒤에 시체 삭제
        SpawnManager.instance.BookHeadRespawn(gameObject);
        BookHead_Animator.SetBool(ResetBool, true);
        gameObject.SetActive(false);
    }
}
