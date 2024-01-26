using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] [Range(1,10)] float Basespeed = 1;
    [SerializeField][Range(1, 20)] float Shiftspeed = 5;
    float speed;
    Controls _Controls;
    Vector2 offset = Vector2.zero;
    void Awake()
    {
        speed = Basespeed;
        _Controls = new Controls();
        _Controls.actionmap.Shift.performed += ctx => speed = Basespeed + Shiftspeed;
        _Controls.actionmap.Shift.canceled += ctx => speed = Basespeed;
    }
    void Update()
    {
        move();
        rotate();
    }
    private void rotate()
    {
        
    }
    private void move()
    {
        _Controls.actionmap.Move.performed += ctx => offset = ctx.ReadValue<Vector2>().normalized;
        transform.position += offset.y * Time.deltaTime * speed * transform.forward;
        transform.position += offset.x * Time.deltaTime * speed * transform.right;
    }
    private void OnEnable()
    {
        _Controls.Enable();
    }
    private void OnDisable()
    {
        _Controls.Disable();
    }
}
