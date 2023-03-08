using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipBehaviour : MonoBehaviour
{
    public float currentHealth;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth < 0)
        {
            // you are dead
        }
    }

    public void ResetHealth()
    {
        currentHealth= 0;
        ComponentBehaviour[] components = gameObject.GetComponentsInChildren<ComponentBehaviour>();
        foreach(ComponentBehaviour component in components)
        {
            currentHealth += component.compResist;
        }
        Debug.Log("--- health reset : " + currentHealth + " ---"); 
    }
}
