using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipSpawner : MonoBehaviour
{
    public static GameObject Spawn(SpaceShip ship, bool isAlly)
    {
        GameObject rootShip = new GameObject("SpaceShip");
        string tag = isAlly ? "Ally" : "Enemy";

        SpaceShipBehaviour shipBehaviour = rootShip.AddComponent<SpaceShipBehaviour>();

        GameObject frame = Instantiate(Resources.Load<GameObject>("Frames/" + ship.frame.name), rootShip.transform);
        frame.tag = tag;

        for(int i=0; i < ship.frame.components.Count; i++)
        {
            GameObject component = Instantiate(Resources.Load<GameObject>("Components/" + ship.frame.components[i]), frame.transform);
            component.transform.position = frame.transform.GetChild(0).GetChild(i).transform.position;
            component.tag = tag;
            ComponentBehaviour behaviour = component.GetComponent<ComponentBehaviour>();
            behaviour.Init(shipBehaviour);
        }

        shipBehaviour.ResetHealth();

        return rootShip;
    }
}
