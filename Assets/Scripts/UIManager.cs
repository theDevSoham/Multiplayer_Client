using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;

    public static UIManager Singleton
    {
        get => _singleton;

        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists. Destroying this value!");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private GameObject hudUI;

    [SerializeField] private TMP_Text displayHealth;
    [SerializeField] private TMP_InputField usernameField;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        hudUI.SetActive(false);
    }

    public void ConnectUIClicked()
    {
        usernameField.enabled = false;
        connectUI.SetActive(false);
        hudUI.SetActive(true);
        displayHealth.text = 100.ToString();

        NetworkManager.Singleton.Connect();
    }

    public void BackToMain()
    {
        usernameField.enabled = true;
        usernameField.text = "";
        connectUI.SetActive(true);
        hudUI.SetActive(false);
        displayHealth.text = "????";
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.name);
        message.AddString(usernameField.text);
        NetworkManager.Singleton.Client.Send(message);
    }
}
