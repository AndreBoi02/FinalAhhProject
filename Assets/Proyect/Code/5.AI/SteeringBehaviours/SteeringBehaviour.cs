using UnityEngine;

public abstract class SteeringBehaviour : ISteeringBehaviour {
    /// <summary>
    /// Decelerates an agent as it approaches its target within a slowing radius.
    /// </summary>
    /// <param name="t_agent">The agent executing the arrive behavior. Requires positive slowingRadius value.</param>
    /// <param name="t_distance">Current distance between agent and target. Must be >= 0.</param>
    /// <remarks>
    /// Implementation details:
    /// 1. Checks if agent is within slowing radius (distance ≤ slowingRadius)
    /// 2. Calculates slowing factor: distance / slowingRadius (range: 0-1)
    /// 3. Scales current velocity by slowing factor (full speed at radius edge, complete stop at target)
    /// </remarks>
    protected void Arrive(Agent t_agent, float t_distance) {
        if (t_distance <= t_agent.GetSteeringVars().slowingRadius) {
            float slowing = t_distance / t_agent.GetSteeringVars().slowingRadius;
            t_agent.SetCurrentVel(t_agent.GetCurrentVel() * slowing);
        }
    }

    /// <summary>
    /// Applies Newtonian steering forces to the agent.
    /// </summary>
    /// <param name="t_desiredVel">Ideal movement vector (pre-normalization)</param>
    /// <param name="t_agent">The agent receiving forces. Requires:
    /// <para>- Rigidbody component</para>
    /// <para>- Configured mass, maxSpeed, maxForce</para>
    /// </param>
    /// <remarks>
    /// Core steering pipeline:
    /// 1. Normalize and scale to maxSpeed
    /// 2. Compute steering force (ΔV = desired - current)
    /// 3. Clamp force to maxForce
    /// 4. Apply mass-division
    /// 5. Update velocity (clamped to maxSpeed)
    /// 
    /// Physics Note: Uses immediate velocity changes. For gradual acceleration, 
    /// integrate with ForceMode.Acceleration.
    /// </remarks>
    protected void CalculateSteer(Vector3 t_desiredVel, Agent t_agent) {
        // Convert to direction vector and scale to max speed
        t_desiredVel = t_desiredVel.normalized;
        t_desiredVel *= t_agent.GetSteeringVars().maxVel;

        // Calculate steering force (change needed)
        Vector3 steering = t_desiredVel - t_agent.GetCurrentVel();

        // Limit steering force magnitude
        steering = TruncateVec(steering, t_agent.GetSteeringVars().maxForce);

        // Apply inverse mass effect (heavier = less responsive)
        steering /= t_agent.GetRigidbody().mass;

        // Update velocity with steering force (clamped to max speed)
        t_agent.SetCurrentVel(TruncateVec(t_agent.GetCurrentVel() + steering, t_agent.GetSteeringVars().maxSpeed));
    }

    /// <summary>
    /// Limits a vector's magnitude while preserving its direction.
    /// </summary>
    /// <param name="t_v">Input vector to clamp</param>
    /// <param name="t_limit">Maximum allowed magnitude (must be positive)</param>
    /// <returns>Original vector if within limit, otherwise normalized vector * limit</returns>
    /// <remarks>
    /// Implementation details:
    /// 1. Validates limit is non-negative (returns Vector3.zero if invalid)
    /// 2. Checks squared magnitude for performance
    /// 3. Returns original vector if already within limits
    /// 4. Returns normalized vector * limit if exceeding bounds
    /// 
    /// Performance Note: Uses sqrMagnitude comparison to avoid expensive sqrt operations.
    /// Physics Note: Preserves vector direction while constraining magnitude.
    /// </remarks>
    protected Vector3 TruncateVec(Vector3 t_v, float t_limit) {
        if (t_limit < 0) {
            Debug.LogError("TruncateVec: maxLength cannot be negative");
            return Vector3.zero;
        }

        float sqrMag = t_v.sqrMagnitude;
        if (sqrMag <= t_limit * t_limit || sqrMag == 0f)
            return t_v;

        return t_v.normalized * t_limit;
    }

    protected Vector3 RandomVector() {
        Vector2 random = Random.insideUnitCircle;
        return new Vector3(random.x, 0, random.y);
    }

    public abstract void Execute(Agent t_agent);
}
