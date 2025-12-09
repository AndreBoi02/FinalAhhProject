
using UnityEngine;

public class CombatEvent : IEvent {
    public struct AttackPrepareEvent : IEvent {
        public GameObject attacker;
        public AttackSystem.AttackType attackType;
        public Vector3 targetPosition;
        public bool isPlayer;
    }

    public struct AttackExecuteEvent : IEvent {
        public GameObject attacker;
        public AttackSystem.AttackType attackType;
        public Vector3 position;
        public Vector3 targetPosition;
        public bool isPlayer;
    }

    public struct WeaponChangedEvent : IEvent {
        public GameObject entity;
        public int weaponIndex;
        public bool isPlayer;
    }

    public struct OnNextWeapon : IEvent {
        public GameObject entity;
    }
    public struct OnPrevWeapon : IEvent {
        public GameObject entity;
    }
}
