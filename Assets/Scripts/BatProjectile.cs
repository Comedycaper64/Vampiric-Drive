using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatProjectile : MonoBehaviour
{
    [SerializeField] private float batSpeed = 30f;
    private Vector2 flightDirection;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.Translate(flightDirection);
    }

    private Vector2 GetMousePosition()
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 relativeMousePos = new Vector2(worldPos.x - transform.position.x, worldPos.y - transform.position.y);
        return relativeMousePos;
    }

    public void SetFlightDirection()
    {
        flightDirection = (GetMousePosition().normalized * batSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        var target = otherCollider.GetComponent<BatTarget>();

        if (target)
        {
            target.TakeDamage(1);
            Destroy(gameObject);
        }

    }
}
