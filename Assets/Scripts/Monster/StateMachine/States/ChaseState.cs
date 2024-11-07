using Sirenix.Utilities.Editor;
using StateMachine.States;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : BaseState
{
    public float BreakAwayDistance = 5f;
    public float AttackDistance = 2.5f;
    public float MaxChaseDuration = 20f;
    public float PathUpdateDelay = .5f;
    public EnemyBehaviour Enemy => Core.Enemy;
    public float DistanceToPlayer => GetDistanceToPlayer();
    
    public Transform ChasedEntity;

    private bool _isAttacking = false;
    private float _pathUpdateTimer = 0;

    public NavMeshAgent Agent => Enemy.NavMeshAgent;
    
    public override void Enter()
    {
        base.Enter();

        _pathUpdateTimer = 0;
        Debug.Log($"{Enemy.name} started chasing.");

        Switch(Enemy.RunState);
    }

    public override void Tick()
    {
        base.Tick();
        
        if (Core.Animator != null)
            Core.Animator.SetFloat("WalkSpeed", Agent.velocity.magnitude/Agent.speed);

        if (_pathUpdateTimer > PathUpdateDelay)
            UpdateNavMeshPath();
        else _pathUpdateTimer += Time.fixedDeltaTime;
    }

    public override void FixedTick()
    {
        base.FixedTick();

        //CheckLookDirection();
        
        if (DistanceToPlayer <= AttackDistance)
        {
            if (_isAttacking) return;
            
            _isAttacking = true;
            Switch(Enemy.AttackState);
            return;
        }

        if (_isAttacking)
        {
            _isAttacking = false;
            Switch(Enemy.RunState);
        }
    }

    private void UpdateNavMeshPath()
    {
        _pathUpdateTimer = 0;
        
        if (Enemy.transform.position != ChasedEntity.position && DistanceToPlayer < BreakAwayDistance && ElapsedTime <= MaxChaseDuration)
        {
            NavMeshPath path = new();
            NavMesh.CalculatePath(Enemy.transform.position, ChasedEntity.position, NavMesh.AllAreas, path);
            Agent.SetPath(path);
        }
        else
        {
            Agent.destination = Enemy.transform.position;
            IsComplete = true;
        }
    }
    
    private float GetDistanceToPlayer() => Vector3.Distance(Enemy.transform.position, ChasedEntity.transform.position);

    private void CheckLookDirection()
    {
        var prevScale = Enemy.transform.localScale;
        var chasedX = ChasedEntity.transform.position.x;
        var enemyX = Enemy.transform.position.x;
        if ((chasedX < enemyX && Enemy.transform.localScale.x > 0) ||
            (chasedX > enemyX && Enemy.transform.localScale.x < 0))
            Enemy.transform.localScale = new Vector3(-prevScale.x, prevScale.y, prevScale.z);
    }
}