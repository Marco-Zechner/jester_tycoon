using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] [Range(1, 10)] float Sensitivity = 1;
    [SerializeField] [Range(1,10)] float Basespeed = 1;
    [SerializeField][Range(1, 20)] float Shiftspeed = 5;
    
    float speed;
    Controls _Controls;
    Vector2 Moveoffset = Vector2.zero;
    Vector2 Rotateoffset = Vector2.zero;

    Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        speed = Basespeed;
        _Controls = new Controls();
        _Controls.actionmap.Shift.performed += ctx => speed = Basespeed + Shiftspeed;
        _Controls.actionmap.Shift.canceled += ctx => speed = Basespeed;
    }
    void Update()
    {
        move();        
        if (_Controls.actionmap.Viewer.IsPressed())rotate();
        else 
        { 
            Cursor.visible = true; 
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void rotate()
    {
        
        if(Cursor.lockState != CursorLockMode.Locked){ StartCoroutine(Hidecursor(0.2f)); }
        _Controls.actionmap.Mouse.performed += ctx => Rotateoffset = ctx.ReadValue<Vector2>() * Sensitivity / 10f;
        //transform.Rotate(new Vector3(-Rotateoffset.y,Rotateoffset.x, 0));
        rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(-Rotateoffset.y, Rotateoffset.x, 0)));

    }
    IEnumerator Hidecursor(float delay) 
    {
        yield return new WaitForSeconds(delay);
        if (_Controls.actionmap.Viewer.IsPressed()) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void move()
    {
        _Controls.actionmap.Move.performed += ctx => Moveoffset = ctx.ReadValue<Vector2>().normalized;
        rb.position += Moveoffset.y * Time.deltaTime * speed * transform.forward;
        rb.position += Moveoffset.x * Time.deltaTime * speed * transform.right;
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
