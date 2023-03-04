using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    private SpaceShip ship;

    private float shipResist;
    private float shipFirePower;
    private Vector2 shipSpeed;
    private float shipWeight;

    private void Start()
    {
        ship = GenerateShipData();

        GameObject rootShip = SpaceShipSpawner.Spawn(ship);
        rootShip.transform.position = this.transform.position;
        rootShip.transform.parent = this.transform;

        shipFirePower = 0.0f;
        shipSpeed = Vector2.zero;
        shipResist = 0.0f;
        shipWeight = 0.0f;
        ComponentBehaviour[] components = rootShip.GetComponentsInChildren<ComponentBehaviour>();
        foreach (ComponentBehaviour component in components)
        {
            shipFirePower += component.firePower;
            shipSpeed += new Vector2(component.forwSpeed, component.latSpeed);
            shipResist += component.compResist;
            shipWeight += component.weight;
        }
    }

    protected abstract SpaceShip GenerateShipData();
}
