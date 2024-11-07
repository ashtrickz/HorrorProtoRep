using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using StateMachine;
using StateMachine.States;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class EnemyBehaviour : StateMachineCore
{
    [Space, SerializeField] public float MoveSpeed = 2f;
    [SerializeField] public NavMeshAgent NavMeshAgent;

    [DisplayAsString] public Vector3 CurrentPatrolPoint;

    [Space, Title("States", TitleAlignment = TitleAlignments.Centered)] [InlineEditor()]
    public IdleState IdleState;

    [InlineEditor] public RunState RunState;
    [InlineEditor] public PatrolState PatrolState;
    [InlineEditor] public ChaseState ChaseState;
    [InlineEditor] public AttackState AttackState;

    [DisplayAsString] public bool PlayerInSight => PatrolState.PlayerInSight;

    private List<BaseState> activeStates = new();

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        Enemy = this;

        InitializeStates();

        NavMeshAgent.speed = MoveSpeed;

        var states = new BaseState[] {IdleState, RunState, PatrolState, ChaseState, AttackState};
        SetupInstances(states);
        StateMachine.SwitchState(PatrolState);

        enabled = true;
    }

    private void InitializeStates()
    {
        if (IdleState   == null) IdleState   = GetComponentInChildren<IdleState>();
        if (RunState    == null) RunState    = GetComponentInChildren<RunState>();
        if (PatrolState == null) PatrolState = GetComponentInChildren<PatrolState>();
        if (ChaseState  == null) ChaseState  = GetComponentInChildren<ChaseState>();
        if (AttackState == null) AttackState = GetComponentInChildren<AttackState>();
    }

    private void Update()
    {
        if (CurrentState.IsComplete)
        {
            Debug.Log($"{CurrentState} for {Enemy.name} is complete.");
            StateMachine.SwitchState(PatrolState);
        }

        CurrentState.TickBranch();
    }

    private void FixedUpdate()
    {
        CurrentState.FixedTickBranch();

        activeStates = StateMachine.GetActiveStateBranch();
    }

    private void OnDrawGizmos()
    {
        if (PatrolState == null) return;

        Handles.color = Color.magenta;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, PatrolState.DetectRadius);

        var viewAngle01 = GetDirectionFromAngle(transform.eulerAngles.y, -PatrolState.DetectAngle / 2);
        var viewAngle02 = GetDirectionFromAngle(transform.eulerAngles.y, PatrolState.DetectAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(transform.position, transform.position + viewAngle01 * PatrolState.DetectRadius);
        Handles.DrawLine(transform.position, transform.position + viewAngle02 * PatrolState.DetectRadius);
    }

    private void OnDrawGizmosSelected()
    {
        if (NavMeshAgent == null) return;

        var statesString = activeStates.Aggregate("", (current, state) => current + (state + ", "));

        var labelPosition = new Vector3(transform.position.x, transform.position.y + NavMeshAgent.height,
            transform.position.z);
        Handles.Label(labelPosition, statesString);
    }

    private Vector3 GetDirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}