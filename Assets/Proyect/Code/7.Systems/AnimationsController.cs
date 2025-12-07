using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(AnimatorController))]
public class AnimationsController : MonoBehaviour {
    [SerializeField] Animator animator;

    EventBinding<AnimationEvent> animationEvent;

    private void OnEnable() {
        animationEvent = new EventBinding<AnimationEvent>(PlayDeath);
        EventBus<AnimationEvent>.Register(animationEvent);

        animationEvent = new EventBinding<AnimationEvent>(PlayRunning);
        EventBus<AnimationEvent>.Register(animationEvent);

        animationEvent = new EventBinding<AnimationEvent>(PlayAttack1);
        EventBus<AnimationEvent>.Register(animationEvent);

        animationEvent = new EventBinding<AnimationEvent>(PlayAttack2);
        EventBus<AnimationEvent>.Register(animationEvent);

        animationEvent = new EventBinding<AnimationEvent>(PlayAttack3);
        EventBus<AnimationEvent>.Register(animationEvent);
    }

    private void OnDisable() {
        EventBus<AnimationEvent>.Deregister(animationEvent);
    }

    void PlayDeath(AnimationEvent animationEvent) {
        animator.SetBool("isDead", animationEvent.OnDead);
    }

    void PlayRunning(AnimationEvent animationEvent) {
        animator.SetBool("isRunning", animationEvent.OnRunnig);
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
