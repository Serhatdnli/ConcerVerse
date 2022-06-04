using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string msg = _packet.ReadString();
        int myId = _packet.ReadInt();

        Debug.Log($"Message from server : {msg}");
        Client.Instance.myId = myId;
        ClientSend.WelcomeReceived();

        Client.Instance.udp.Connect(((IPEndPoint)Client.Instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int id = _packet.ReadInt();
        string username = _packet.ReadString();
        Vector3 position = _packet.ReadVector3();
        Quaternion rotation = _packet.ReadQuaternion();

        GameManager.Instance.SpawnPlayer(id, username, position, rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int id = _packet.ReadInt();
        Vector3 position = _packet.ReadVector3();
        int animatorState = _packet.ReadInt();

        GameManager.players[id].transform.position = position;
        GameManager.players[id].GetComponent<Animator>()?.SetInteger("State", animatorState);
    }

    public static void PlayerRotation(Packet _packet)
    {
        int id = _packet.ReadInt();
        Quaternion rotation = _packet.ReadQuaternion();

        GameManager.players[id].transform.rotation = rotation;
    }

    public static void ReadMessageFromServer(Packet _packet)
    {
        string username = _packet.ReadString();
        string message = _packet.ReadString();

        UIManager.Instance.NewMessage(username, message);
    }

    public static void DanceMusic(Packet _packet)
    {
        int id = _packet.ReadInt();
        GameManager.players[id].GetComponent<PlayerManager>().DanceAudioStart();
    }
    public static void DestroyPlayer(Packet _packet)
    {
        int id = _packet.ReadInt();
        Destroy(GameManager.players[id].gameObject);
        GameManager.players.Remove(id);
    }
}
