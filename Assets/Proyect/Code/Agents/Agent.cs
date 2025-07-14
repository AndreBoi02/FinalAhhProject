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

    [Tooltip("Not used now")]
    [SerializeField] public float m_proximity;
}

#endregion

[RequireComponent(typeof(Rigidbody), typeof(AttackSystem))]
public class Agent : MonoBehaviour {
    #region Enums

    protected enum typeOfBehaviours {
        Seek,
        Flee,
        none
    }

    #endregion

    #region References

    protected Agent m_aTarget;
    protected Rigidbody m_rb => GetComponent<Rigidbody>();
    protected AttackSystem attackSystem => GetComponent<AttackSystem>();

    SteeringVars steeringVars;

    #endregion

    #region Runtime Var

    [HideInInspector] public Vector3 m_targetPos;
    [HideInInspector] public Vector3 m_pos;
    [HideInInspector] public Vector3 m_currentVel;
    protected typeOfBehaviours type = typeOfBehaviours.Seek;

    #endregion

    protected virtual void Start() {
        m_pos = transform.position;
        m_targetPos = m_aTarget.transform.position;
    }
    
    protected virtual void FixedUpdate() {
        m_pos = transform.position;
        Move();
        m_targetPos = m_aTarget.transform.position;
    }
    
    protected virtual void Move() {
        switch (type) {
            case typeOfBehaviours.Seek:
                EnemyBehaviour.Seek(this);
                break;
            case typeOfBehaviours.Flee:
                EnemyBehaviour.Flee(this);
                break;
            case typeOfBehaviours.none:
                return;
        }
        m_rb.linearVelocity = m_currentVel;
    }

    #region Getters & Setters

    public Agent GetTargetAgent() {
        return m_aTarget;
    }

    public Rigidbody GetRigidbody() {
        return m_rb; 
    }

    public SteeringVars GetSteeringVars() {
        return steeringVars;
    }

    public void SetSteeringVars(SO_EnemyVariables sO_EnemyVariables) {
        steeringVars = sO_EnemyVariables.SteeringVars;
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
}