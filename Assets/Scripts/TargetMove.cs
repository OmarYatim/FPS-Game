using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    [SerializeField] private bool CanMove;
    [SerializeField] private float MoveDistance;
    [SerializeField] private float Speed;

    private Vector3 InitialPosition;
    Vector3 MoveDirection = Vector3.right;
    Rigidbody TargetRB;

    private void Start()
    {
        InitialPosition = transform.position;
        TargetRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(CanMove)
            TargetRB.velocity = MoveDirection * Speed;
    }
    private void Update()
    {
        if (CanMove)
        {
            float distance = Vector3.Distance(transform.position, InitialPosition);
            distance = Mathf.Floor(distance);
            Debug.Log(distance, gameObject);

            if (distance >= Mathf.Floor(MoveDistance))
            {
                MoveDirection = (InitialPosition - transform.position).normalized;
            }
        }
    }
}
