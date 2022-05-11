﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.Instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.Instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.Instance.myId);
            _packet.Write(UIManager.Instance.userName.text);

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.Instance.myId].transform.rotation);
            _packet.Write(GameManager.players[Client.Instance.myId].transform.position);
            _packet.Write(Movement.Instance.Animator.GetInteger("State"));

            SendUDPData(_packet);
        }
    }

    public static void SendMessage(string username, string message)
    {
        using (Packet _packet = new Packet((int)ClientPackets.message))
        {
            _packet.Write(username);
            _packet.Write(message);

            SendUDPData(_packet);
        }
    }
    #endregion
}
