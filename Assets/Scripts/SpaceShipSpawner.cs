using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipSpawner : MonoBehaviour
{
    public static GameObject Spawn(SpaceShip ship)
    {
        GameObject rootShip = new GameObject("SpaceShip");

        GameObject frame = Instantiate(Resources.Load<GameObject>("Frames/" + ship.frame.name), rootShip.transform);

        for(int i=0; i < ship.frame.components.Count; i++)
        {
            ShipComponent comp = ship.frame.components[i];
            GameObject component = Instantiate(Resources.Load<GameObject>("Components/" + comp.name), frame.transform);
            component.transform.position = frame.transform.GetChild(0).GetChild(i).transform.position;
        }

        return rootShip;
    }
}
