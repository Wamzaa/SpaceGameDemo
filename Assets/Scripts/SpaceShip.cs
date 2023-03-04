using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

[Serializable]
public class SpaceShip
{
    public ShipFrame frame;
}

[Serializable]
public class ShipFrame
{
    public string name;
    public List<string> components;
}

