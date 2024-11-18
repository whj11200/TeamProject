using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFlashDamage : MonoBehaviour
{
    public int Demon_Counter = 0;
    float timer;
    bool isSoundPlay;
    [SerializeField] bool isFlashing;
    NavMeshAgent Enemyagent;
    Animator Enemyanimator;
    [SerializeField] Enemy enemy;
    [SerializeField] ParticleSystem particle_somoke;
    [SerializeField] CapsuleCollider Demon_cap;
    [SerializeField] AudioClip Demon_Steam;

    [SerializeField] private Collider currentCollider; // 현재 충돌체 저장

    private void Start()
    {
        Demon_Steam = Resources.Load<AudioClip>("Sound/Demon/Demon_Steam");

        Demon_cap = GetComponent<CapsuleCollider>();
        timer = 0f;
        isFlashing = false;
        Enemyagent = GetComponent<NavMeshAgent>();
        Enemyanimator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        particle_somoke = transform.GetChild(5).GetComponent<ParticleSystem>();
        particle_somoke.Stop();
    }

    private void OnEnable()
    {
        if (Enemyagent == null)
        {
            return;
        }
        else
        {
            timer = 0;
            Enemyagent.isStopped = false;
            Enemyagent.speed = 5;
            Demon_cap.enabled = true;
            particle_somoke.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enemy.DemonDie)
        {
            if (other.gameObject.CompareTag("FlashCol"))
            {
                currentCollider = other; // 현재 충돌체 저장
                Debug.Log(currentCollider);

                if (currentCollider.enabled) // 콜라이더가 활성화되어 있을 때
                {

                    isFlashing = true;
                    particle_somoke.Play();

                    // 사운드 재생 로직
                    if (!isSoundPlay && Demon_Steam != null)
                    {
                        Demon_Steam.name = $"Demon_Steam_{Demon_Counter}";
                        InGameSoundManager.instance.ActiveSound(gameObject, Demon_Steam, 10, true, true, true, 1);
                        isSoundPlay = true;
                    }


                    StartCoroutine(FlashingCoroutine()); // 코루틴 시작
                }
            }

        }
        
    }

    public IEnumerator FlashingCoroutine()
    {
        while (isFlashing && timer < 3f)
        {
            print(timer);
            if (currentCollider != null && currentCollider.enabled) // 콜라이더가 활성화된 경우
            {
                timer += Time.deltaTime; // 타이머 증가

                if (timer >= 2f)
                {
                    if (isSoundPlay)
                    {
                        InGameSoundManager.instance.EditSoundBox($"Demon_Steam_{Demon_Counter}", false);
                        InGameSoundManager.instance.Data.Remove($"Demon_Steam_{Demon_Counter}");
                        isSoundPlay = false;
                        timer = 0f;
                        isFlashing = false;
                        if (enemy.Killplayer == false)
                        {
                            Demon_cap.enabled = false; // 충돌 비활성화
                            particle_somoke.Stop();
                            Enemyagent.isStopped = true; // 이동 멈춤
                            Enemyagent.speed = 0;
                            Enemyanimator.SetTrigger("Flash"); // 애니메이션 트리거

                            yield return new WaitForSeconds(4.5f); // 대기
                            if(enemy.DemonDie == false)
                            {
                                Demon_cap.enabled = true; // 충돌 활성화
                                Enemyagent.speed = 5;
                                Enemyagent.isStopped = false; // 다시 이동 시작
                            }
                        }
                    }
                }

            }
            else
            {
                HandleExitState(); // 콜라이더가 비활성화된 경우 상태 종료
                yield break; // 코루틴 종료
            }

            yield return null; // 다음 프레임까지 대기
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == currentCollider)
        {
            HandleExitState(); // 상태 종료 처리
            currentCollider = null; // 현재 충돌체 초기화
        }
    }

    public void HandleExitState()
    {
     
        isFlashing = false; // 플래시 상태 종료

        if (isSoundPlay && InGameSoundManager.instance.Data.ContainsKey($"Demon_Steam_{Demon_Counter}"))
        {
            InGameSoundManager.instance.EditSoundBox($"Demon_Steam_{Demon_Counter}", false);
            InGameSoundManager.instance.Data.Remove($"Demon_Steam_{Demon_Counter}");
        }

        // 상태 초기화
        timer = 0f; // 타이머 초기화
        particle_somoke.Stop(); // 파티클 정지
        isSoundPlay = false; // 사운드 상태 초기화
    }
}
