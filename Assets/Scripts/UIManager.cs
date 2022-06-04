using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] public InputField userName, messageInput;
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private Button danceBtn;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private GameObject messagePrefab;


    public VariableJoystick VariableJoystick { get => variableJoystick; }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        userName.interactable = false;
        Client.Instance.ConnectedToServer();
    }

    public void DanceBtnFunc(Movement _player)
    {
        danceBtn.onClick.AddListener(_player.Dance);
    }

    public void NewMessage(string _username, string _message)
    {
        GameObject message = Instantiate(messagePrefab);
        message.GetComponent<Text>().text = _username + " : " + _message;
        message.transform.SetParent(messagePanel.transform);
        message.GetComponent<RectTransform>().localScale = new Vector3(.5f, .5f, .5f);
    }

    public void SendMessage()
    {
        if (messageInput.text != null && messageInput.text != "")
        {
            ClientSend.SendMessage(Client.Instance.username, messageInput.text);
            messageInput.text = "";
        }
    }
}


