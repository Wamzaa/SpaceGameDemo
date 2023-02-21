using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
public class SpaceShip
{
    public ShipFrame frame;

    public float GetShipResist()
    {
        float totalResist = 0.0f;
        foreach(ShipComponent comp in frame.components)
        {
            totalResist += comp.compResist; 
        }
        return totalResist;
    }

    public Vector2 GetShipSpeed()
    {
        Vector2 totalSpeed = Vector2.zero;
        foreach(ShipComponent comp in frame.components)
        {
            totalSpeed.x += comp.forwSpeed;
            totalSpeed.y += comp.latSpeed;
        }
        totalSpeed = (1.0f / Mathf.Sqrt(GetShipWeight())) * totalSpeed;
        return totalSpeed;
    }

    public float GetShipFirePower() 
    {
        float totalFirePower = 0.0f;
        foreach(ShipComponent comp in frame.components) 
        {
            totalFirePower += comp.firePower;
        }
        return totalFirePower;
    }

    public float GetShipWeight()
    {
        float totalWeight = 0.0f;
        foreach (ShipComponent comp in frame.components)
        {
            totalWeight += comp.weight;
        }
        return totalWeight;
    }
}

[Serializable]
public class ShipFrame
{
    public string name;
    public int nbComponents;
    public List<ShipComponent> components;
}

[Serializable]
public class ShipComponent
{
    public string name;
    public float firePower;
    public float forwSpeed;
    public float latSpeed;
    public float compResist;
    public float weight;
}
