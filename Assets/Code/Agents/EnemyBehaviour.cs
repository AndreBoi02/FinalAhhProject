using UnityEngine;

public static class EnemyBehaviour {

    public static void seek(Agent t_agent) {
        Vector3 desiredVel = t_agent.aTarget.m_pos - t_agent.m_pos;
        float distance = distanceV(t_agent.m_pos, t_agent.aTarget.m_pos);
        baseBehaviour(desiredVel, t_agent);
        arrive(t_agent, distance);
    }

    public static void flee(Agent t_agent) {
        Vector3 desiredVel = t_agent.m_pos - t_agent.aTarget.m_pos;
        baseBehaviour(desiredVel, t_agent);
    }

    static void arrive(Agent t_agent, float t_distance) {
        if (t_distance <= t_agent.m_slowingFactor) {
            float slowing = t_distance / t_agent.m_slowingFactor;
            t_agent.m_currentVel *= slowing;
        }
    }

    static void baseBehaviour(Vector3 t_desiredVel, Agent t_agent) {
        t_desiredVel = t_desiredVel.normalized;
        t_desiredVel *= t_agent.m_maxVel;
        Vector3 steering = t_desiredVel - t_agent.m_currentVel;
        steering = truncateVec(steering, t_agent.m_maxForce);
        steering /= t_agent.rb2D.mass;
        t_agent.m_currentVel =
        truncateVec(t_agent.m_currentVel + steering, t_agent.m_maxSpeed);
    }

    static Vector3 truncateVec(Vector3 t_v, float t_limit) {
        Vector3 res = t_v;
        if (res.magnitude <= t_limit) {
            return res;
        }
        res = res.normalized;
        res *= t_limit;
        return res;
    }

    static float distanceV(Vector3 t_this, Vector3 t_other) {
        return Mathf.Sqrt(Mathf.Pow((t_this.x - t_other.x), 2) + Mathf.Pow((t_this.y - t_other.y), 2) + Mathf.Pow((t_this.z - t_other.z), 2));
    }
}
