using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Ayayaya");
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Destroy(this.gameObject);
        }
    }
}
