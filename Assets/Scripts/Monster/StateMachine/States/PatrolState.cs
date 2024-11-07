using Sirenix.OdinInspector;
using StateMachine.States;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : BaseState
{
    public Vector2 TimeBetweenPatroling = new Vector2(1, 3);
    public Vector2 PartolingDistance = new Vector2(5, 5);
    public float PatrolSpeedMultiplier = 0.75f;
    public int PointSelectTriesCount = 5;

    [FoldoutGroup("Field of View"), SerializeField] public float DetectRadius = 2f;
    [Range(0, 360)]
    [FoldoutGroup("Field of View"), SerializeField] public float DetectAngle = 45f;
    [FoldoutGroup("Field of View"), SerializeField] public float DetectDelay = .2f;
    [FoldoutGroup("Field of View"), SerializeField] public LayerMask TargetMask;
    [FoldoutGroup("Field of View"), SerializeField] public LayerMask ObstructionMask;
    
    [HideInInspector] public bool PlayerInSight;
    [HideInInspector] public NavMeshAgent agent;

    private int _positionStuckRepeats = 20;
    private int _positionStuckCounter = 0;
    private float _timeBeforePartoling = 0;
    private float _fovCheckTimer = 0f;
    private bool _patrolingPaused = false;

    private Vector3 _patrolPoint;
    private Vector3 _cachedPosition;

    public EnemyBehaviour Enemy => Core.Enemy;

    public override void Enter()
    {
        base.Enter();
        
        agent = Enemy.NavMeshAgent;
        _fovCheckTimer = 0;
        
        StartWaiting();
    }

    private Vector3 SelectPatrolPoint()
    {
        var patrolPoint = new Vector3();
        var tries = PointSelectTriesCount;

        while (tries >= 0)
        {
            patrolPoint = new Vector3(
                (Enemy.transform.position.x + Random.Range(-PartolingDistance.x, PartolingDistance.x)),
                Enemy.transform.position.y,
                (Enemy.transform.position.z + Random.Range(-PartolingDistance.y, PartolingDistance.y)));
            if (Physics.Raycast(Enemy.transform.position, patrolPoint, out var hit,
                Vector3.Distance(Enemy.transform.position, patrolPoint)))
            {
                if (hit.collider.CompareTag("Obstacle") == false) break;
            }

            tries--;
        }

        return patrolPoint;
    }

    private void CheckFieldOfView()
    {
        _fovCheckTimer = 0;
        
        Collider[] rangeChecks = Physics.OverlapSphere(Enemy.transform.position, DetectRadius, TargetMask);

        if (rangeChecks.Length != 0)
        {
            Transform playerTransform = null;
            foreach (var collider in rangeChecks)
            {
                var player = collider.GetComponent<PlayerManager>();
                if (player == null) continue;
                playerTransform = player.transform;
            }
            
            if (playerTransform == null) return;
            var directionToTarget = (playerTransform.position - Enemy.transform.position).normalized;

            if (Vector3.Angle(Enemy.transform.position, directionToTarget) < DetectAngle / 2)
            {
                var distanceToTarget = Vector3.Distance(Enemy.transform.position, playerTransform.position);

                PlayerInSight = !Physics.Raycast(Enemy.transform.position, directionToTarget, distanceToTarget,
                    ObstructionMask); // NO FOV DETECTION

                if (!PlayerInSight) return;
                
                Enemy.ChaseState.ChasedEntity = playerTransform;
                agent.destination = Enemy.ChaseState.ChasedEntity.position;
                Enemy.StateMachine.SwitchState(Enemy.ChaseState);
            }
            else PlayerInSight = false;
        }
        else if (PlayerInSight) PlayerInSight = false;
    }

    public override void Tick()
    {
        base.Tick();

        if (_patrolingPaused && ElapsedTime >= _timeBeforePartoling)
            StopWaiting();
    }

    public override void FixedTick()
    {
        base.FixedTick();
        
        if (Core.Animator != null)
            Core.Animator.SetFloat("WalkSpeed", agent.velocity.magnitude/agent.speed);

        if (_patrolingPaused) return;

        if (Enemy.NavMeshAgent.destination == Enemy.transform.position) StartWaiting();
        
        CheckForStuck();
        if (_fovCheckTimer > DetectDelay)
            CheckFieldOfView();
        else _fovCheckTimer += Time.fixedDeltaTime;
    }

    private void CheckForStuck()
    {
        if (_cachedPosition == Enemy.transform.position)
        {
            _positionStuckCounter++;
            if (_positionStuckCounter >= _positionStuckRepeats)
                StartWaiting();
        }

        _cachedPosition = Enemy.transform.position;
    }

    private void StartWaiting()
    {
        _positionStuckCounter = 0;

        _timeBeforePartoling = ElapsedTime + Random.Range(TimeBetweenPatroling.x, TimeBetweenPatroling.y);
        _patrolingPaused = true;
        Switch(Enemy.IdleState, true);
    }

    private void StopWaiting()
    {
        _patrolingPaused = false;
        _patrolPoint = SelectPatrolPoint();
        agent.destination = _patrolPoint;
        Switch(Enemy.RunState);
    }
}