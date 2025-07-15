using UnityEngine;

public class SeekBehaviour : SteeringBehaviour {
    /// <summary>
    /// Drives the agent toward its target with intelligent deceleration.
    /// </summary>
    /// <param name="t_agent">The agent executing the behavior. Requires:
    /// <para>- Valid Transform and Rigidbody</para>
    /// <para>- Configured maxSpeed/slowingRadius</para>
    /// </param>
    /// <remarks>
    /// Implements standard seek with arrival threshold braking:
    /// 1. Computes ideal velocity vector to target
    /// 2. Applies force-based steering (mass-sensitive)
    /// 3. Triggers arrival deceleration within slowing radius
    /// 
    /// Design Note: Tune slowingRadius for "personality" (aggressive/gentle stops).
    /// </remarks>
    public override void Execute(Agent t_agent) {
        Vector3 desiredVel = t_agent.GetTargetAgent().m_pos - t_agent.m_pos;
        float distance = Vector3.Distance(t_agent.m_pos, t_agent.GetTargetAgent().m_pos);
        BaseBehaviour(desiredVel, t_agent);
        Arrive(t_agent, distance);
    }
}
