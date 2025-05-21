using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AttackSystem))]
public class Agent : MonoBehaviour {
    public enum typeOfBehaviours {
        Seek,
        Flee,
        none
    }

    public typeOfBehaviours type = typeOfBehaviours.Seek;

    public Agent aTarget;
    [HideInInspector] public Rigidbody rb => GetComponent<Rigidbody>();

    [HideInInspector] public Vector3 m_targetPos;
    [HideInInspector] public Vector3 m_pos;
    [HideInInspector] public Vector3 m_currentVel;

    public float m_maxVel;
    public float m_maxForce;
    public float m_maxSpeed;
    public float m_slowingFactor;
    public float m_proximity;

    protected AttackSystem attackSystem => GetComponent<AttackSystem>();
    
    protected virtual void Start() {
        m_pos = transform.position;
        m_targetPos = aTarget.transform.position;
    }
    
    protected virtual void FixedUpdate() {
        m_pos = transform.position;
        Move();
        m_targetPos = aTarget.transform.position;
    }
    
    protected virtual void Move() {
        switch (type) {
            case typeOfBehaviours.Seek:
                EnemyBehaviour.seek(this);
                break;
            case typeOfBehaviours.Flee:
                EnemyBehaviour.flee(this);
                break;
            case typeOfBehaviours.none:
                return;
        }
        rb.linearVelocity = m_currentVel;
    }

    [Header("Rotation Settings")]
    [SerializeField] protected float rotationSpeed = 5f;

    protected virtual void FacePlayer() {
        Vector3 direction = (m_targetPos - transform.position).normalized;
        direction.y = 0; // Opcional: mantener el enemigo "plano" en el eje Y

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
}