using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookHeadAI : MonoBehaviour
{
    private Transform Player_Transform;
    private Transform BookHead_Transform;

    private BookHeadMovement BookHead_Movement;
    private BookHeadAttack BookHead_Attack;

    private Vector3 TracePoint;

    private float dist;
    private float TraceVect_dist;

    private readonly float Attack_dist = 2.0f;
    private readonly float OnTrace = 10.0f;

    private readonly string playerTag = "Player";

    private bool bookHead_isDie;

    private bool bookHead_isKill;
    public bool BookHead_isKill
    {
        get { return bookHead_isKill; }
        set { bookHead_isKill = value; }
    }

    public bool BookHead_isDie
    {
        get { return bookHead_isDie; }
        set { bookHead_isDie = value; }
    }

    public enum State
    {
        IDLE = 0, POINT_TRACE, PLAYER_TRACE, ATTACK, RESPAWN, WAIT
    }
    public State state = State.IDLE;
    void Awake()
    {
        Player_Transform = GameObject.FindWithTag(playerTag).GetComponent<Transform>();
        BookHead_Transform = GetComponent<Transform>();
        BookHead_Movement = GetComponent<BookHeadMovement>();
        BookHead_Attack = GetComponent<BookHeadAttack>();
    }
    private void OnEnable()
    {
        TracePoint = Player_Transform.position;

        StartCoroutine(CheckState()); //몬스터의 상태 업데이트
        StartCoroutine(UpdatePlayerTransform()); //플레이어 위치를 10초마다 업데이트
        StartCoroutine(Action()); //state에 따라 정보 업데이트
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1.0f); // 초기 설정기다리기
        while(!BookHead_isDie && !GameManager.G_instance.isGameover) //bookhead가 죽거나, 플레이어가 죽으면 정지 (리스폰시 재진입)
        {
            if(state == State.RESPAWN) yield break;
            dist = (Player_Transform.position - BookHead_Transform.position).magnitude;
            TraceVect_dist = (BookHead_Transform.position - TracePoint).magnitude;

            if (dist < Attack_dist) //어택이 최우선
            {
                state = State.ATTACK;
            }

            else if (dist < OnTrace) //플레이어 쫓기가 2순위
            {
                state = State.PLAYER_TRACE;
            }

            else if (TraceVect_dist < 1.5f) //플레이어 추격을 위한 지점 업데이트가 3순위
            {
                state = State.IDLE;
            }

            else if (dist > OnTrace) //플레이어 이전 위치 추격이 4순위
            {
                state = State.POINT_TRACE;
            }

            if (GameManager.G_instance.AllStop)
            {
                state = State.WAIT;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }


    IEnumerator Action()
    {
        yield return new WaitForSeconds(0.5f); // 초기 설정기다리기

        while (!BookHead_isDie && !GameManager.G_instance.isGameover) //bookhead가 죽거나, 플레이어가 죽으면 정지 (리스폰시 재진입)
        {
            yield return new WaitForSeconds(0.2f);

            switch (state)
            {
                case State.IDLE:
                    BookHead_Movement.IsIdle = true;
                    yield return new WaitForSeconds(2.0f);
                    TracePoint = Player_Transform.position;
                    break;

                case State.POINT_TRACE:
                    BookHead_Movement.Point_Trace = TracePoint;
                    break;

                case State.PLAYER_TRACE:
                    BookHead_Movement.Player_Trace = Player_Transform.position;
                    break;

                case State.ATTACK:
                    BookHead_Attack.isAttack = true;
                    yield return new WaitForSeconds(1.8f);
                    break;

                case State.RESPAWN:
                    BookHead_Movement.BookHeadStop();
                    BookHead_isDie = true;
                    break;

                case State.WAIT:
                    BookHead_Movement.IsIdle = true;
                    yield return new WaitForSeconds(4.0f);
                    break;
            }
        }
        if (!BookHead_isKill && !BookHead_isDie)
            BookHead_Movement.PlayerDie();
    }


    IEnumerator UpdatePlayerTransform()
    {
        yield return new WaitForSeconds(0.5f); // 초기 설정기다리기

        while (!GameManager.G_instance.isGameover)
        {
            TracePoint = Player_Transform.position;
            yield return new WaitForSeconds(20.0f);
        }
    }
}
