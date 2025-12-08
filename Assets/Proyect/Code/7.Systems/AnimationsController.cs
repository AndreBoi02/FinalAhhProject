using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(AnimatorController))]
public class AnimationsController : MonoBehaviour {
    [SerializeField] Animator animator;

    EventBinding<AnimationEvent> animationEvent;
    EventBinding<RunEvent> runningEvent;
    EventBinding<DeathEvent> deathEvent;

    private void OnEnable() {
        deathEvent = new EventBinding<DeathEvent>(PlayDeath);
        EventBus<DeathEvent>.Register(deathEvent);

        runningEvent = new EventBinding<RunEvent>(PlayRunning);
        EventBus<RunEvent>.Register(runningEvent);

        animationEvent = new EventBinding<AnimationEvent>(PlayAttack1);
        EventBus<AnimationEvent>.Register(animationEvent);

        animationEvent = new EventBinding<AnimationEvent>(PlayAttack2);
        EventBus<AnimationEvent>.Register(animationEvent);

        animationEvent = new EventBinding<AnimationEvent>(PlayAttack3);
        EventBus<AnimationEvent>.Register(animationEvent);
    }

    private void OnDisable() {
        EventBus<DeathEvent>.Deregister(deathEvent);
        EventBus<RunEvent>.Deregister(runningEvent);
        EventBus<AnimationEvent>.Deregister(animationEvent);
    }

    void PlayDeath(DeathEvent animationEvent) {
        animator.SetBool("isDead", animationEvent.isDead);
    }

    void PlayRunning(RunEvent animationEvent) {
        animator.SetBool("isRunning", animationEvent.isRunnig);
    }
    
    void PlayAttack1(AnimationEvent animationEvent) {
        animator.SetBool("isAttacking1", animationEvent.OnAttacking1);
    }
    
    void PlayAttack2(AnimationEvent animationEvent) {
        animator.SetBool("isAttacking2", animationEvent.OnAttacking2);
    }
    
    void PlayAttack3(AnimationEvent animationEvent) {
        animator.SetBool("isAttacking3", animationEvent.OnAttacking3);
    }

}
