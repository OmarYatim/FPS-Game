using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TargetMove))]
public class TargetMoveGUI : Editor
{
    public override void OnInspectorGUI()
    {
        var myScript = target as TargetMove;

        myScript.CanMove = GUILayout.Toggle(myScript.CanMove, "Can Move");
        if(myScript.CanMove)
        {
            myScript.MoveDistance = EditorGUILayout.Slider("Move Distance",myScript.MoveDistance, 1, 25);
            myScript.Speed = EditorGUILayout.FloatField("Speed",myScript.Speed);
        }
    }
}
#endif
public class TargetMove : MonoBehaviour
{
    [HideInInspector] public bool CanMove;
    [HideInInspector] public float MoveDistance = 10;
    [HideInInspector] public float Speed = 5.0f;

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

            if (distance >= Mathf.Floor(MoveDistance))
            {
                MoveDirection = (InitialPosition - transform.position).normalized;
            }
        }
    }
}
