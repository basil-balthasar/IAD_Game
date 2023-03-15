using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
public class SimpleFPPN : NetworkBehaviour
{
    #region lookVariables
    [Header("Look")]
    Vector2 look;

    public float sensX = 1;
    public float sensY = 1;

    float xRotation;
    float yRotation;

    public Transform cam;
    #endregion

    #region moveVariables
    [Header("Movement")]
    public float moveSpeed = 100f;
    Vector2 move;
    Vector3 moveDirection;

    Rigidbody rb;
    #endregion

    public override void OnNetworkSpawn(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update(){
        if(!IsOwner) return;
    }

    public void OnLook(InputAction.CallbackContext value){
        look = value.ReadValue<Vector2>();

        float lookX = look.x * Time.deltaTime * sensX;
        float lookY = look.y * Time.deltaTime * sensY;

        yRotation += lookX;
        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void OnMovement(InputAction.CallbackContext value){
        move = value.ReadValue<Vector2>();

        moveDirection = cam.forward * move.x + cam.right * move.y;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }
}