using UnityEngine;

public class WanderBehaviour : SteeringBehaviour {
    protected ISteeringBehaviour m_steeringBehaviour;

    /// <summary>
    /// Generates semi-random movement by creating and pursuing wandering points.
    /// </summary>
    /// <param name="t_agent">The agent executing wander behavior. Requires:
    /// <para>- Configured steering variables (m_displacement, m_radius, m_proximity)</para>
    /// <para>- Current velocity (for direction calculation)</para>
    /// <para>- Valid position reference</para>
    /// </param>
    /// <remarks>
    /// Implementation details:
    /// 1. Uses current velocity direction as movement baseline
    /// 2. Creates a wandering point by:
    ///    a. Projecting a point forward (displacement distance along velocity)
    ///    b. Adding random offset within wander radius
    /// 3. Only generates new point when close to current target (proximity check)
    /// 4. Delegates actual movement to Seek behavior
    /// 
    /// Parameter roles:
    /// - m_displacement: Forward projection distance from current position (how far ahead to place the base point)
    /// - m_radius: Circular area around displacement point where random targets can appear
    /// - m_proximity: Distance threshold for generating new wandering points
    /// 
    /// Design notes:
    /// - The fixed Y value (0.07f) ensures targets stay near ground level
    /// - Combines deterministic movement (current direction) with randomness
    /// - Creates organic, non-repetitive wandering patterns
    /// </remarks>
    public override void Execute(Agent t_agent) {
        m_steeringBehaviour = new SeekBehaviour();
        float distance = Vector3.Distance(t_agent.GetCurrentPos(), t_agent.GetTargetPos());
        m_steeringBehaviour.Execute(t_agent);
        if (distance >= t_agent.GetSteeringVars().m_proximity) {
            return;
        }
        Vector3 newWanderPos = t_agent.GetCurrentVel().normalized;
        newWanderPos = new Vector3(
            newWanderPos.x * t_agent.GetSteeringVars().m_displacement,
            .07f, 
            newWanderPos.z * t_agent.GetSteeringVars().m_displacement);
        Vector3 vRandom = RandomVector();
        vRandom = new Vector3(
            vRandom.x * t_agent.GetSteeringVars().m_radius,
            .07f, 
            vRandom.z * t_agent.GetSteeringVars().m_radius);
        newWanderPos += vRandom + t_agent.GetCurrentPos();
        t_agent.SetTargetPos(newWanderPos);
    }
}
