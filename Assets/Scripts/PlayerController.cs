using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class PlayerController : MonoBehaviour
{
    private bool[] inputs;
    private PlayerMovements playerInputs;

    public PlayerMovements PlayerInputs => playerInputs;

    [SerializeField] private Transform cameraTransform;


    private void Awake()
    {
        playerInputs = new PlayerMovements();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    private void Start()
    {
        inputs = new bool[8];
    }

    private void Update()
    {

        if (playerInputs.Movement.Forward.IsPressed())
        {
            inputs[0] = true;
        }
        if (playerInputs.Movement.Backward.IsPressed())
        {
            inputs[1] = true;
        }
        if (playerInputs.Movement.Left.IsPressed())
        {
            inputs[2] = true;
        }
        if (playerInputs.Movement.Right.IsPressed())
        {
            inputs[3] = true;
        }
        if (playerInputs.Parkour.Jumping.triggered)
        {
            inputs[4] = true;
        }
        if (playerInputs.Parkour.Sprinting.IsPressed())
        {
            inputs[5] = true;
        }
        if (playerInputs.Shoot.ShootBullet.IsPressed())
        {
            inputs[6] = true;
        }
        if (playerInputs.Shoot.AimGun.IsPressed())
        {
            inputs[7] = true;
        }
    }

    private void FixedUpdate()
    {
        SendInput();

        for(int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = false;
        }
    }

    private void SendInput()
    {
        Message message = Message.Create(MessageSendMode.Unreliable, (ushort)ClientToServerId.input);
        message.AddBools(inputs, false);
        message.AddVector3(cameraTransform.forward);

        NetworkManager.Singleton.Client.Send(message);
    }
}
