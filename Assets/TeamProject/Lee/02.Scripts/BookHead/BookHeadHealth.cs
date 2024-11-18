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
        base.OnEnable(); // ��Ƴ� ������ bool���� �ʱ�ȭ, hp �ʱ�ȭ
        BookHead_Animator.SetBool(ResetBool, false);
        BookHead_State.state = BookHeadAI.State.IDLE; //��Ƴ��� �ʱ�ȭ
        BookHead_State.BookHead_isDie = false; //��Ƴ��� �ʱ�ȭ, AI�� die ������Ʈ
        dead = false; //��Ƴ��� �ʱ�ȭ, creture�� die ������Ʈ
    }

    public override void OnDamage(object[] param)
    {
        InGameSoundManager.instance.ActiveSound(gameObject, B_Hit, 6.0f, true, false, false, 1, 1.0f); //���� ���Ҹ�
        base.OnDamage(param);
    }
    public override void Die()
    {
        base.Die();
        InGameSoundManager.instance.EditSoundBox($"B_Idle{BookHeadClip_Idx}", false); //���� ������ �Ҵ�� ���� �ڽ�����
        InGameSoundManager.instance.Data.Remove($"B_Idle{BookHeadClip_Idx}"); //������ ���� �ڽ� �Ҵ��� �� ����� key�� ����.

        BookHead_Animator.SetTrigger(DieTrigger);

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        BookHead_State.state = BookHeadAI.State.RESPAWN;
        yield return new WaitForSeconds(3.5f); //3.5�� �ڿ� ��ü ����
        SpawnManager.instance.BookHeadRespawn(gameObject);
        BookHead_Animator.SetBool(ResetBool, true);
        gameObject.SetActive(false);
    }
}
