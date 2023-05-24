using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public enum State { Idle, Trace, Return, Attack, Patrol, Size }

     public TMP_Text text;
     public float detectRange;
     public float moveSpeed;
     public float attackRange;
     public Transform[] patrolPoints;


    private StateBase<Bee>[] states;
    private State curState;

    public Transform player;
    public Vector3 returnPosition;
    public int patrolIndex = 0;

    private void Awake()
    {
        states = new StateBase<Bee>[(int)State.Size];
        states[(int)State.Idle] = new BeeState.IdleState(this);
        states[(int)State.Trace] = new BeeState.TraceState(this);
        states[(int)State.Return] = new BeeState.ReturnState(this);
        states[(int)State.Attack] = new BeeState.AttackState(this);
        states[(int)State.Patrol] = new BeeState.PatrolState(this);
    }

    private void Start()
    {
        curState = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
    }

    private void Update()
    {
        text.text = curState.ToString();
        states[(int)curState].Update();
    }
    public void ChangeState(State state)
    {
        curState = state;
    }
}

namespace BeeState
{
    public class IdleState : StateBase<Bee>
    {
        private float idleTime = 0;
        public IdleState(Bee owner) : base(owner)
        {
        }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void Setup()
        {
            
        }

        public override void Update()
        {
            idleTime += Time.deltaTime;

            if(idleTime > 2)
            {
                idleTime = 0;
                owner.patrolIndex = (owner.patrolIndex + 1) % owner.patrolPoints.Length;
                owner.ChangeState(Bee.State.Patrol);
            }
            else if(Vector2.Distance(owner.transform.position, owner.player.position) < owner.detectRange)
            {
                owner.ChangeState(Bee.State.Trace);
            }
        }
    }

    public class TraceState : StateBase<Bee>
    {
        public TraceState(Bee owner) : base(owner)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void Setup()
        {
            
        }

        public override void Update()
        {
            Vector2 dir = (owner.player.position - owner.transform.position).normalized;
            owner.transform.Translate(dir * owner.moveSpeed * Time.deltaTime);

            if (Vector2.Distance(owner.player.position, owner.transform.position) > owner.detectRange)
            {
                owner.ChangeState(Bee.State.Return);
            }
            else if (Vector2.Distance(owner.player.position, owner.transform.position) < owner.attackRange)
            {
                owner.ChangeState(Bee.State.Attack);
            }
        }
    }

    public class ReturnState : StateBase<Bee>
    {
        public ReturnState(Bee owner) : base(owner)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void Setup()
        {
            
        }

        public override void Update()
        {
            Vector2 dir = (owner.returnPosition - owner.transform.position).normalized;
            owner.transform.Translate(dir * owner.moveSpeed * Time.deltaTime);

            if (Vector2.Distance(owner.transform.position, owner.returnPosition) < 0.02)
            {
                owner.ChangeState(Bee.State.Idle);
            }
            else if (Vector2.Distance(owner.transform.position, owner.player.position) < owner.detectRange)
            {
                owner.ChangeState(Bee.State.Trace);
            }
        }
    }

    public class AttackState : StateBase<Bee>
    {
        public AttackState(Bee owner) : base(owner)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void Setup()
        {
            
        }

        public override void Update()
        {
            Debug.Log("공격");

            if (Vector2.Distance(owner.player.position, owner.transform.position) > owner.attackRange)
            {
                owner.ChangeState(Bee.State.Trace);
            }
        }
    }

    public class PatrolState : StateBase<Bee>
    {
        public PatrolState(Bee owner) : base(owner)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void Setup()
        {
            
        }

        public override void Update()
        {
            Vector2 dir = (owner.patrolPoints[owner.patrolIndex].position - owner.transform.position).normalized;
            owner.transform.Translate(dir * owner.moveSpeed * Time.deltaTime);

            if (Vector2.Distance(owner.transform.position, owner.patrolPoints[owner.patrolIndex].position) < 0.02f)
            {
                owner.ChangeState(Bee.State.Idle);
            }
        }
    }

}















/*
public class Bee : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float detectRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform[] patrolPoints;
    public enum State { Idle, Trace, Return, Attack, Patrol }

    private State curState;

    private Transform player;
    private Vector3 returnPosition;
    private int patrolIndex = 0;

    private void Start()
    {
        curState = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
    }

    private void Update()
    {
        switch (curState)
        {
            case State.Idle:
                text.text = "Idle";
                IdleUpdate();
                break;
            case State.Trace:
                text.text = "Trace";
                TraceUpdate();
                break;
            case State.Return:
                text.text = "Return";
                ReturnUpdate();
                break;
            case State.Attack:
                text.text = "Attack";
                AttackUpdate();
                break;
            case State.Patrol:
                text.text = "Patrol";
                PatrolUpdate();
                break;
        }
    }

    float idleTime = 0;
    private void IdleUpdate()
    {
        if(idleTime > 2)
        {
            idleTime = 0;
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            curState = State.Patrol;
        }
        idleTime += Time.deltaTime;
        if (Vector2.Distance(player.position, transform.position) < detectRange)
        {
            curState = State.Trace;
        }
    }

    private void TraceUpdate()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    
        if (Vector2.Distance(player.position, transform.position) > detectRange)
        {
            curState = State.Return;
        }
        else if(Vector2.Distance(player.position, transform.position) < attackRange)
        {
            curState = State.Attack;
        }
    }

    private void ReturnUpdate()
    {
        Vector2 dir = (returnPosition - transform.position).normalized; 
        transform.Translate(dir*moveSpeed * Time.deltaTime);
    
        if (Vector2.Distance(transform.position, returnPosition) < 0.02)
        {
            curState = State.Idle;
        }
        else if (Vector2.Distance(transform.position, player.position) < detectRange)
        {
            curState = State.Trace;
        }
    }

    private void AttackUpdate()
    {
        Debug.Log("공격");

        if (Vector2.Distance(player.position, transform.position) > attackRange)
        {
            curState = State.Trace;
        }
    }

    private void PatrolUpdate()
    {
        
        Vector2 dir = (patrolPoints[patrolIndex].position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolPoints[patrolIndex].position) < 0.02f)
        {
            curState = State.Idle;
        }
        else if(Vector2.Distance(transform.position, player.position) < detectRange)
        {
            curState = State.Trace;
        }
    }

}
*/