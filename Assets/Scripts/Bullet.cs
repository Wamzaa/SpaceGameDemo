using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float power;

    private float timer;

    private void Start()
    {
        timer = 0.0f;
    }

    private void Update()
    {
        transform.position = transform.position + 22.0f * Time.deltaTime * transform.up;

        timer += Time.deltaTime;
        if(timer > 20.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool hit = (collision.tag == "Ally" && this.gameObject.tag == "Enemy") || (collision.tag == "Enemy" && this.gameObject.tag == "Ally");
        Debug.Log(hit + " --- " + collision.name);
        if (hit)
        {
            ComponentBehaviour comp = collision.gameObject.GetComponent<ComponentBehaviour>();
            if(comp != null)
            {
                comp.TakeDamage(power);
            }
        }
    }
}
