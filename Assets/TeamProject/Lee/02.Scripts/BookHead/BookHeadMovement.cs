using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BookHeadMovement : MonoBehaviour
{
    private Transform BookHead_Transform;
    private NavMeshAgent BookHead_agent;
    private Animator BookHead_animator;

    private float Damping;

    [SerializeField]private float point_TraceSpeed = 0.8f;
    [SerializeField]private float player_TraceSpeed = 3.0f;

    private readonly int hashMovement = Animator.StringToHash("Movement");

    private bool _isIdle;
    public bool IsIdle
    {
        get { return _isIdle; }
        set 
        { 
            _isIdle = value;
            if(_isIdle == true)
            {
                WaitTrace();
            }
        }
    }
    private Vector3 _Point_Trace;
    public Vector3 Point_Trace
    {
        get { return _Point_Trace; }
        set
        {
            _Point_Trace = value;
            if(_Point_Trace != Vector3.zero)
            {
                PointTrace();
            }
        }
    }
    private Vector3 _Player_Trace;
    public Vector3 Player_Trace
    {
        get { return _Player_Trace; }
        set
        {
            _Player_Trace = value;
            if(_Player_Trace != Vector3.zero)
            {
                PlayerTrace();
            }
        }
    }
    void Awake()
    {
        BookHead_Transform = transform;
        BookHead_agent = GetComponent<NavMeshAgent>();
        BookHead_animator = GetComponent<Animator>();

        GameManager.G_instance.SpeedUp += IncreaseSpeed;
    }

    private void Update()
    {
        if (BookHead_agent.isStopped == false && !GameManager.G_instance.isGameover)
        {
            if (BookHead_agent.desiredVelocity != Vector3.zero) //BookHead_agent.desiredVelocity는 도착지점에 도달하면 0,0,0 벡터 발생, 해당 벡터를 넣으면 경고를 계속 보냄.
            {
                Quaternion rot = Quaternion.LookRotation(BookHead_agent.desiredVelocity);
                BookHead_Transform.rotation = Quaternion.Lerp(BookHead_Transform.rotation, rot, Time.deltaTime * Damping);
            }
        }
    }
    private void WaitTrace()
    {
        BookHead_animator.SetFloat(hashMovement, 0.0f);
        BookHead_agent.isStopped = true;
    }

    private void PointTrace()
    {
        _isIdle = false;
        BookHead_animator.SetFloat(hashMovement, 0.5f);
        BookHead_agent.isStopped = false;
        Damping = 2.5f;
        BookHead_agent.speed = point_TraceSpeed;
        BookHead_agent.destination = Point_Trace;
    }

    private void PlayerTrace()
    {
        _isIdle = false;
        BookHead_animator.SetFloat(hashMovement, 1.0f);
        BookHead_agent.isStopped = false;
        Damping = 5.0f;
        BookHead_agent.speed = player_TraceSpeed;
        BookHead_agent.destination = Player_Trace;
    }

    public void BookHeadStop()
    {
        BookHead_agent.isStopped = true;
    }

    private void IncreaseSpeed()
    {
        point_TraceSpeed += 0.4f;
        player_TraceSpeed += 1.0f;
    }


    public void PlayerDie()
    {
        BookHead_animator.SetFloat(hashMovement, 0.0f);
        BookHead_agent.isStopped = true;
        this.gameObject.SetActive(false);
    }

}
