using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private PlayerController controller;

    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float clampAngle = 85f;

    private float verticalRotation;
    private float horizontalRotation;

    private void OnValidate()
    {
        if(player == null)
        {
            player = transform.parent.gameObject;
        }

        if(controller == null)
        {
            controller = transform.parent.gameObject.GetComponent<PlayerController>();
        }
    }

    private void Start()
    {
        verticalRotation = transform.localEulerAngles.x;
        horizontalRotation = player.transform.localEulerAngles.y;
    }

    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            Look();
        if (Input.GetKey(KeyCode.Escape))
            ToggleCursor();
            

        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);
    }

    private void Look()
    {
        float mouseX = controller.PlayerInputs.Look.LookRotationX.ReadValue<float>();
        float mouseY = controller.PlayerInputs.Look.LookRotationY.ReadValue<float>();

        verticalRotation += mouseY * -sensitivity * Time.deltaTime;
        horizontalRotation += mouseX * sensitivity * Time.deltaTime;

        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        player.transform.localRotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }

    private void ToggleCursor()
    {
        Cursor.visible = !Cursor.visible;

        if (Cursor.lockState == CursorLockMode.None)
            Cursor.lockState = CursorLockMode.Locked;
        else if (Cursor.lockState == CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.None;
    }
}
