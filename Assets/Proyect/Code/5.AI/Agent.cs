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

    [Tooltip("Circular area around displacement point for random wander targets" +
        " \n ↑ Increase → Wider wandering area (more exploration)." +
        " \n ↓ Decrease → Narrower wandering area (more linear movement).")]
    [SerializeField] public float m_predictionTime;
}
#endregion

[RequireComponent(typeof(Rigidbody))]
public abstract class Agent : MonoBehaviour {

    #region References
    [SerializeField] protected RAttackSystem m_attackSystem;
    [SerializeField] protected CharacterAnimationHandler m_animHandler;
    protected Rigidbody m_rb => GetComponent<Rigidbody>();
    [SerializeField] protected Agent m_aTarget;
    protected Transform m_targetTransform;
    protected Rigidbody m_targetRb;

    [SerializeField] SO_EnemyVariables m_EnemyVars;
    SteeringVars m_steeringVars;

    StatHandler m_statHandler;
    #endregion

    #region Runtime Var
    protected Vector3 m_pos => transform.position;
    protected Vector3 m_targetPos;
    protected Vector3 m_currentVel;

    [SerializeField] Agent sheltered;
    #endregion

    protected ISteeringBehaviour m_currentBehaviour;
    EventBinding<DeathEvent> deathEvent;
    protected bool isDead = false;

    protected virtual void Start() {
        deathEvent = new EventBinding<DeathEvent>(StopAll);
        EventBus<DeathEvent>.Register(deathEvent);

        if (m_attackSystem == null) m_attackSystem = GetComponent<RAttackSystem>();
        if (m_animHandler == null) m_animHandler = GetComponent<CharacterAnimationHandler>();
        m_statHandler = GetComponent<StatHandler>();

        AcquireTarget();

        if (m_EnemyVars != null)
            m_steeringVars = m_EnemyVars.SteeringVars;
    }

    void Update() {
        if (m_statHandler != null) {
            m_statHandler.PlayerAlive();
        }
    }

    private void OnDisable() {
        EventBus<DeathEvent>.Deregister(deathEvent);
    }

    protected virtual void FixedUpdate() {
        if (isDead) return;

        if (m_targetTransform == null || !m_targetTransform.gameObject.activeInHierarchy) {
            AcquireTarget();
        }

        if (m_targetTransform != null) {
            m_targetPos = m_targetTransform.position;
        } else if (m_aTarget != null) {
            m_targetPos = m_aTarget.transform.position;
        }

        if (m_attackSystem != null) {
            m_attackSystem.TargetPosition = m_targetPos;
        }

        ExecuteBehaviour();
    }
     
    protected virtual void ExecuteBehaviour() { }

    public void AcquireTarget() {
        // If m_aTarget is already set and active, use it
        if (m_aTarget != null && m_aTarget.gameObject.activeInHierarchy) {
            m_targetTransform = m_aTarget.transform;
            m_targetRb = m_aTarget.GetRigidbody();
            m_targetPos = m_targetTransform.position;
            return;
        }

        // Find active player GameObject in the scene
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players) {
            if (p.activeInHierarchy) {
                // Check if it's the main player root (has controller or agent)
                if (p.GetComponent<RPlayerController>() != null || p.GetComponent<Agent>() != null) {
                    m_targetTransform = p.transform;
                    m_targetRb = p.GetComponent<Rigidbody>();
                    m_aTarget = p.GetComponent<Agent>();
                    m_targetPos = m_targetTransform.position;
                    return;
                }
            }
        }

        // Fallback: any active object with tag "Player"
        GameObject fallback = GameObject.FindWithTag("Player");
        if (fallback != null && fallback.activeInHierarchy) {
            m_targetTransform = fallback.transform;
            m_targetRb = fallback.GetComponent<Rigidbody>();
            m_aTarget = fallback.GetComponent<Agent>();
            m_targetPos = m_targetTransform.position;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + transform.forward * m_steeringVars.m_displacement, m_steeringVars.m_radius);
    }

    #region Getters & Setters

    public Agent GetTarget() {
        return m_aTarget; 
    }

    public Transform GetTargetTransform() {
        return m_targetTransform;
    }

    public Rigidbody GetTargetRigidbody() {
        if (m_targetRb != null) return m_targetRb;
        if (m_aTarget != null) return m_aTarget.GetRigidbody();
        return null;
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
        if (t_agent != null && t_agent.gameObject.activeInHierarchy) {
            m_aTarget = t_agent;
            m_targetTransform = t_agent.transform;
            m_targetRb = t_agent.GetRigidbody();
            m_targetPos = m_targetTransform.position;
        } else {
            AcquireTarget();
        }
    }

    public void SetTargetTransform(Transform t_transform) {
        if (t_transform != null && t_transform.gameObject.activeInHierarchy) {
            m_targetTransform = t_transform;
            m_targetRb = t_transform.GetComponent<Rigidbody>();
            m_aTarget = t_transform.GetComponent<Agent>();
            m_targetPos = m_targetTransform.position;
        } else {
            AcquireTarget();
        }
    }

    protected void SetBehavior(ISteeringBehaviour newBehaviour) {
        m_currentBehaviour = newBehaviour;
    }

    #endregion

    #region Utility & Combat Helpers

    [Header("Rotation Settings")]
    [SerializeField] protected float rotationSpeed = 5f;

    public virtual void FacePlayer() {
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

    public float DistanceFromPlayer() {
        return Vector3.Distance(m_pos, m_targetPos);
    }

    [SerializeField] protected float safetyDistance;
    public bool IsPlayerInSideRadius() {
        return DistanceFromPlayer() < safetyDistance;
    }

    [SerializeField] protected float attackCoolDown;
    protected float attackCounter;
    public bool OnAttackCoolDown() {
        if (attackCounter > attackCoolDown) {
            attackCounter = 0;
            return false;
        }
        attackCounter += Time.deltaTime;
        return true;
    }
    #endregion

    public RAttackSystem AttackSystem => m_attackSystem;

    public StatHandler GetStatHandler() {
        return m_statHandler;
    }

    void StopAll(DeathEvent deathEvent) {
        if (deathEvent.Source == gameObject) {
            isDead = deathEvent.isDead;
            if (isDead) {
                m_currentVel = Vector3.zero;
                if (m_rb != null) m_rb.linearVelocity = Vector3.zero;
                m_animHandler?.SetMoving(false);
                m_animHandler?.PlayDeath(true);
            }
            EreaseEnemy();
        }
    }

    public bool GetIsDead() {
        return isDead;
    }

    void EreaseEnemy() {
        Destroy(gameObject, 5);
    }

    public Agent GetSheltered() {
        return sheltered;
    }
}
