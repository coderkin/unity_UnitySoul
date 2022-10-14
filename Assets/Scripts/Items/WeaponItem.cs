using UnityEngine;
namespace CF
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;
        public bool isShield;

        [Header("Idle Animations")]
        public string right_hand_idle;
        public string left_hand_idle;

        [Header("Damage")]
        public float baseDamage;
        public float heavyMutiplier;
        public float parryMutiplier;

        [Header("One Handed Attack Animations")]
        public string oh_light_attack_1;
        public string oh_light_attack_2;
        public string oh_light_attack_3;
        public string oh_heavy_attack_1;

        [Header("Stamina Costs")]
        public int baseStamina;
        public float lightAttackMutiplier;
        public float heavyAttackMutiplier;
    }
}
