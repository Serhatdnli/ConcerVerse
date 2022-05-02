using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] public InputField userName;

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        userName.interactable = false;
        Client.Instance.ConnectedToServer();
    }
}


