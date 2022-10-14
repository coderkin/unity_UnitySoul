using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollider : MonoBehaviour
{
    Collider shieldCollider;

    private void Awake()
    {
        shieldCollider = GetComponent<BoxCollider>();
    }

    public void SetShieldColliderEnable(bool enable)
    {
        shieldCollider.enabled = enable;
    }
}
