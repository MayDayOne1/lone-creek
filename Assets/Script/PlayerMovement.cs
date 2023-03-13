// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody _rb;
    private float _movementX;
    private float _movementY;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        Vector3 movementVector = value.Get<Vector2>();
        _movementX = movementVector.x;
        _movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(_movementX, 0f, _movementY);
        _rb.MovePosition(transform.position + movement * Time.deltaTime * _speed);
    }
}
