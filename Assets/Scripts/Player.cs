using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    [SerializeField] private Transform cameraTransform;

    public ushort Id { get; private set; }

    public bool IsLocal { get; private set; }

    public string Username { get; private set; }

    public int health { get; private set; }

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    private void Move(Vector3 camForward, Vector3 playerPosition)
    {
        transform.position = playerPosition;

        if (!IsLocal)
        {
            cameraTransform.forward = camForward;
        }
    }

    private void Rotate(Quaternion camRotation, Quaternion playerRotation)
    {
        transform.localRotation = playerRotation;

        if (!IsLocal)
        {
            cameraTransform.localRotation = camRotation;
        }
    }

    public static void Spawn(ushort id, int health, string username, Vector3 position)
    {
        Player player;
        if(id == NetworkManager.Singleton.Client.Id)
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }

        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.Id = id;
        player.Username = username;
        player.health = health;

        list.Add(id, player);
    }


    [MessageHandler((ushort) ServerToClientId.playerSpawned)]

    public static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetInt(), message.GetString(), message.GetVector3());
    }

    [MessageHandler((ushort)ServerToClientId.playerMovement)]

    public static void PlayerMovement(Message message)
    {
        if(list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.Move(message.GetVector3(), message.GetVector3());
        }
    }

    [MessageHandler((ushort)ServerToClientId.updateHealth)]

    public static void HealthUpdate(Message message)
    {
        if(list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.health = message.GetInt();
        }
    }
}
