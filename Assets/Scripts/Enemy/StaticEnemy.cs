using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : EnemyBehaviour
{
    protected override SpaceShip GenerateShipData()
    {
        ShipFrame frame = new ShipFrame();
        frame.name = "basic1frame";
        List<string> listComp = new List<string>();
        listComp.Add("rafal-A12");
        frame.components = listComp;
        SpaceShip ship = new SpaceShip();
        ship.frame = frame;

        return ship;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Destroy(this.gameObject);
        }
    }
}
