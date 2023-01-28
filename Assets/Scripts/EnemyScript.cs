using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform movePoint1;
    [SerializeField] private Transform movePoint2;
    [SerializeField] private GameObject variousMovePoints;
    private Transform currentMovePoint;

    // Start is called before the first frame update
    void Start()
    {
        currentMovePoint = movePoint1;
        movePoint1.SetParent(variousMovePoints.transform);
        movePoint2.SetParent(variousMovePoints.transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentMovePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentMovePoint.position) <= .05f)
        {
            if (currentMovePoint == movePoint1)
            {
                currentMovePoint = movePoint2;
            }
            else
            {
                currentMovePoint = movePoint1;
            }
        }
    }
}
