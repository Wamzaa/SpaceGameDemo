using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBehaviour : MonoBehaviour
{
    [Header("Component Variables")]
    private SpaceShipBehaviour ship;
    public float firePower;
    public float forwSpeed;
    public float latSpeed;
    public float compResist;
    public float weight;

    private float currentHealth;

    public void Init(SpaceShipBehaviour _ship)
    {
        currentHealth = compResist;
        ship = _ship;
    }

    public void TakeDamage(float damage)
    {
        ship.TakeDamage(Mathf.Min(currentHealth, damage));
        currentHealth -= damage;
        Debug.Log("- Damage Taken : " + damage + " -> " + Mathf.Min(currentHealth, damage) + " -");
        if(currentHealth < 0)
        {
            Destroy(gameObject);
        }
    }
}
