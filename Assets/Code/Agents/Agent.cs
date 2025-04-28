using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Agent : MonoBehaviour 
{
    public enum typeOfBehaviours {
        Seek,
        Flee,
        none
    }

    public typeOfBehaviours type = typeOfBehaviours.Seek;

    public Agent aTarget;
    [HideInInspector] public Vector3 m_targetPos;

    [HideInInspector] public Rigidbody2D rb2D => GetComponent<Rigidbody2D>();
    [HideInInspector] public Vector3 m_pos;
    [HideInInspector] public Vector3 m_currentVel;

    public float m_maxVel;
    public float m_maxForce;
    public float m_maxSpeed;
    public float m_slowingFactor;
    public float m_proximity;

    [SerializeField] string targetNameA;
    [SerializeField] string targetNameV;

    protected virtual void Start() {
        m_pos = transform.position;
        if (targetNameA != "") {
            aTarget = GameObject.Find(targetNameA).GetComponent<Agent>();
        }
        if (targetNameV != "") {
            m_targetPos = GameObject.Find(targetNameV).transform.position;
        }
    }

    protected virtual void FixedUpdate() {
        m_pos = transform.position;
        Move();
        if (targetNameV == "") {
            return;
        }
        m_targetPos = GameObject.Find(targetNameV).transform.position;
    }

    protected virtual void Move() {
        switch (type) {
            case typeOfBehaviours.Seek:
                EnemyBehaviour.seek(this);
                break;
            case typeOfBehaviours.Flee:
                EnemyBehaviour.flee(this);
                break;
        }
        rb2D.linearVelocity = m_currentVel;
    }
}
