using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBehaviour : MonoBehaviour
{
    [Header("Component Variables")]
    public float firePower;
    public float forwSpeed;
    public float latSpeed;
    public float compResist;
    public float weight;

    private GameObject ship;
    private float currentHealth;
}
