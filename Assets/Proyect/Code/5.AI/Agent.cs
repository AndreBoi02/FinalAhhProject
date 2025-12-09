using UnityEngine;

#region Struct
[System.Serializable]
public struct SteeringVars{
    [Tooltip("Controls how fast the agent wants to move" +
        " \n ↑ Increase → More aggressive movement (faster acceleration toward the target)." +
        " \n ↓ Decrease → Smoother, slower movement.")]
    [SerializeField] public float maxVel;

    [Tooltip("Limits how sharply the agent can turn or change direction." +
        " \n ↑ Increase → Sharper turns, more responsive movement (can look \"jittery\" if too high)." +
        " \n ↓ Decrease → Smoother, more gradual turns (can feel \"sluggish\" if too low).")]
    [SerializeField] public float maxForce;

    [Tooltip("Caps the agent’s real speed (after steering forces are applied)." +
        " \n ↑ Increase → The agent moves faster in a straight line." +
        " \n ↓ Decrease → The agent never exceeds this speed, even if m_maxVel is higher.")]
    [SerializeField] public float maxSpeed;

    [Tooltip("Determines how early the agent slows down when approaching a target" +
        " \n ↑ Increase → The agent starts braking earlier (smoother stops)." +
        " \n ↓ Decrease → The agent brakes late (may overshoot the target).")]
    [SerializeField] public float slowingRadius;

    [Tooltip("Minimum distance threshold to trigger new wander points" +
        " \n ↑ Increase → Agent changes direction less frequently (longer paths)." +
        " \n ↓ Decrease → Agent changes direction more often (erratic movement).")]
    [SerializeField] public float m_proximity;

    [Tooltip("Forward projection distance for wander point generation" +
        " \n ↑ Increase → Points appear farther ahead (smoother turns)." +
        " \n ↓ Decrease → Points appear closer (sharper turns).")]
    [SerializeField] public float m_displacement;

    [Tooltip("Circular area around displacement point for random wander targets" +
        " \n ↑ Increase → Wider wandering area (more exploration)." +
        " \n ↓ Decrease → Narrower wandering area (more linear movement).")]
    [SerializeField] public float m_radius;
}

#endregion

[RequireComponent(typeof(Rigidbody), typeof(AttackSystem))]
public abstract class Agent : MonoBehaviour, IAttackSystem {

    #region References
    protected AttackSystem m_attackSystem => GetComponent<AttackSystem>();
    protected Rigidbody m_rb => GetComponent<Rigidbody>();
    [SerializeField] protected Agent m_aTarget;

    [SerializeField] SO_EnemyVariables m_EnemyVars;
    SteeringVars m_steeringVars;

    StatHandler m_statHandler;
    #endregion

    #region Runtime Var

    protected Vector3 m_pos => transform.position;
    protected Vector3 m_targetPos;
    protected Vector3 m_currentVel;

    #endregion

    protected ISteeringBehaviour m_currentBehaviour;
    EventBinding<DeathEvent> deathEvent;

    protected virtual void Start() {
        deathEvent = new EventBinding<DeathEvent>(StopAll);
        EventBus<DeathEvent>.Register(deathEvent);

        m_statHandler = GetComponent<StatHandler>();

        m_targetPos = m_aTarget != null ? m_aTarget.transform.position : Vector3.zero;

        if(m_EnemyVars != null)
            m_steeringVars = m_EnemyVars.SteeringVars;
    }

    void Update() {
        m_statHandler.PlayerAlive();
    }

    private void OnDisable() {
        EventBus<DeathEvent>.Deregister(deathEvent);
    }

    protected virtual void FixedUpdate() {
        ExecuteBehaviour();
        m_targetPos = m_aTarget != null ? m_aTarget.transform.position : Vector3.zero;
    }
     
    protected virtual void ExecuteBehaviour() { }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + transform.forward * m_steeringVars.m_displacement, m_steeringVars.m_radius);
    }

    #region Getters & Setters

    public Agent GetTarget() {
    return m_aTarget; 
    }
    
    public Rigidbody GetRigidbody() {
        return m_rb; 
    }

    public SteeringVars GetSteeringVars() {
        return m_steeringVars;
    }

    public Vector3 GetCurrentPos() {
        return m_pos;
    }
    
    public Vector3 GetTargetPos() {
        return m_targetPos;
    }

    public Vector3 GetCurrentVel() {
        return m_currentVel;
    }
    
    public void SetCurrentVel(Vector3 val) {
        m_currentVel = val;
    }

    public void SetTargetPos(Vector3 val) {
        m_targetPos = val;
    }

    public void SetSteeringVars(SO_EnemyVariables sO_EnemyVariables) {
        m_steeringVars = sO_EnemyVariables.SteeringVars;
    }

    public void SetTarget(Agent t_agent) {
        m_aTarget = t_agent;
    }

    protected void SetBehavior(ISteeringBehaviour newBehaviour) {
        m_currentBehaviour = newBehaviour;
    }

    #endregion

    #region Trash

    [Header("Rotation Settings")]
    [SerializeField] protected float rotationSpeed = 5f;

    protected virtual void FacePlayer() {
        Vector3 direction = (m_targetPos - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    protected float DistanceFromPlayer() {
        return Vector3.Distance(m_pos, m_targetPos);
    }
    [SerializeField] protected float safetyDistance;
    public bool IsPlayerInSideRadius() {
        if (DistanceFromPlayer() < safetyDistance)
            return true;
        return false;
    }

    [SerializeField] protected float attackCoolDown;
    protected float attackCounter;
    protected bool OnAttackCoolDown() {
        if (attackCounter > attackCoolDown) {
            attackCounter = 0;
            return false;
        }
        attackCounter += Time.deltaTime;
        return true;
    }
    #endregion

    public virtual event System.Action OnPrepareAttack;
    public virtual event System.Action OnAttack;

    public AttackSystem AttackSystem => m_attackSystem;

    protected virtual void PrepareOnAttack() {
        OnPrepareAttack?.Invoke();
    }

    protected virtual void InvokeOnAttack() {
        OnAttack?.Invoke();
    }

    public StatHandler GetStatHandler() {
        return m_statHandler;
    }

    bool isDead = false;
    void StopAll(DeathEvent deathEvent) {
        if (deathEvent.Source == gameObject)
            isDead = deathEvent.isDead;
    }

    public bool GetIsDead() {
        return isDead;
    }
}