using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;


public class Enemy : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public Rigidbody rb;
    public string PlayTag = "Player";
    public Transform PlayerPos;
    public Transform tr;
    Light DeadSceneLight;
    PlayableDirector director;
    [Tooltip("공격사거리와 파악위치 사거리")]
    float distance = 20;
    float attackside = 2.0f;
    public int Demon_Counter = 0;
    public bool Killplayer = false;
    private bool isDead = false;
    public bool DemonDie = false;
    float timer = 0;
    CinemachineStateDrivenCamera State_Demon;
    CinemachineVirtualCamera VirtualCamera_Demon;
    CapsuleCollider Demon_cap;
    ParticleSystem particle_somoke;
    AudioClip DemonDie_SFX;
    AudioClip DemonAttack_SFX;
    EnemyFlashDamage enemyFlash;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        PlayTag = GameObject.Find(PlayTag).tag;
        tr = GetComponent<Transform>();
        PlayerPos = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        StartCoroutine(StartFollowingPlayer());
        DeadSceneLight = transform.GetChild(2).gameObject.GetComponent<Light>();
        DeadSceneLight.enabled = false;
        director = GameObject.Find("TimeLine_Demon").gameObject.GetComponent<PlayableDirector>();
        Demon_cap = GetComponent<CapsuleCollider>();
        particle_somoke = transform.GetChild(5).GetComponent<ParticleSystem>();
        DemonDie_SFX = Resources.Load<AudioClip>("Sound/Demon/DemonDie");
        DemonAttack_SFX = Resources.Load<AudioClip>("Sound/Demon/DemonAttackSound");
        enemyFlash = gameObject.GetComponent<EnemyFlashDamage>();
        

    }
    private void OnEnable()
    {

        DemonDie = false;
        StartCoroutine(DelayedStart());
        
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1f); // 2초 대기
        StartCoroutine(StartFollowingPlayer());
        timer = 0;
        InvokeRepeating(nameof(UpdateTimer), 0f, 1f);
        rb.isKinematic = false;
        isDead = false;
    }


    private void UpdateTimer()
    {
        if (!GameManager.G_instance.isGameover)
        {
            timer += 1f;
            if (timer >= 30)
            {
                InGameSoundManager.instance.ActiveSound(gameObject, DemonDie_SFX, 7, true, false, false, 1, 2f);
                timer = 30;
                StartCoroutine(FalseDemon());
                
            }
        }      
    }

    private void FollowPlayertoAttack()
    {
       
        if (!GameManager.G_instance.isGameover)
        {
            var Distance = Vector3.Distance(PlayerPos.transform.position, tr.transform.position);
            Vector3 Playerpos = (PlayerPos.position - transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(Playerpos);
            if (Distance <= attackside)
            {

                animator.SetBool("IsRun", false);
                animator.SetBool("Attack", true);

                agent.isStopped = true;

            }
            else if (Distance <= distance)
            {
                distance = 50f;
                agent.destination = PlayerPos.transform.position;
                animator.SetBool("IsRun", true);
                animator.SetBool("Attack", false);
                agent.isStopped = false;
                
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 3F);
        }

        if (GameManager.G_instance.isGameover)
        {
            if (!isDead) 
            {
                string keyToRemove = $"DemonBgSound_{Demon_Counter}";
                if (InGameSoundManager.instance.Data.ContainsKey(keyToRemove))
                {
                    InGameSoundManager.instance.EditSoundBox(keyToRemove, false);
                    InGameSoundManager.instance.Data.Remove(keyToRemove);
                }
                Destroy(gameObject,0.4f);
            }
        }

        if (GameManager.G_instance.AllStop)
        {
            StartCoroutine(AllStop());
        }
    }
    IEnumerator AllStop()
    {
        agent.speed = 0;
        agent.isStopped = true;

        yield return new WaitForSeconds(5f);
        agent.isStopped = false;
        agent.speed = 5f;
    }
    IEnumerator FalseDemon()
    {
        DemonDie = true;
        StopCoroutine(StartFollowingPlayer());
        StopCoroutine(enemyFlash.FlashingCoroutine());
        if (InGameSoundManager.instance.Data.ContainsKey($"DemonBgSound_{Demon_Counter}"))
        {
            InGameSoundManager.instance.EditSoundBox($"DemonBgSound_{Demon_Counter}", false);
            InGameSoundManager.instance.Data.Remove($"DemonBgSound_{Demon_Counter}");
        }
        enemyFlash.HandleExitState();
        particle_somoke.Stop();
        agent.isStopped = true;
        agent.speed = 0;
        rb.isKinematic = true;
        animator.SetTrigger("Out");
        Demon_cap.enabled = false;
        CancelInvoke(nameof(UpdateTimer));
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        timer = 0;
        DemonDie = false;
    }
    IEnumerator StartFollowingPlayer()
    {
        while (true) 
        {
            FollowPlayertoAttack();
            yield return null; // 다음 프레임까지 대기
        }
    }
    void KillPlayer()
    {

        if (isDead) return;
        enemyFlash.HandleExitState();
        if (InGameSoundManager.instance.Data.ContainsKey($"DemonBgSound_{Demon_Counter}"))
        {
            InGameSoundManager.instance.EditSoundBox($"DemonBgSound_{Demon_Counter}", false);
            InGameSoundManager.instance.Data.Remove($"DemonBgSound_{Demon_Counter}");
        }
        isDead = true;
        animator.speed = 1;
        particle_somoke.Stop();
        Killplayer = true;
        DeadSceneLight.enabled = true;
        StopCoroutine("FalseDemon()");
        StopCoroutine("StartFollowingPlayer()");
        agent.isStopped = true; // 이동 멈춤
        agent.speed = 0;
        animator.SetTrigger("Kill");
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        director.Play();
        DeadSceneLight.enabled = true;
        State_Demon.Priority = 20;
        VirtualCamera_Demon.Priority = 20;
    }


    private void OnDisable()
    {
        CancelInvoke(nameof(UpdateTimer));
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
            rb.isKinematic = true;
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
            rb.isKinematic = false;
    }
    public void Attack()
    {
        InGameSoundManager.instance.ActiveSound(gameObject, DemonAttack_SFX, 5, true, false, false, 1);
    }
}
