using System.Collections;
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

    public static void PlayerMovement(Vector3 _position,Quaternion _rotation, int _animState)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_position);
            _packet.Write(_rotation);
            _packet.Write(_animState);

            SendUDPData(_packet);
        }
    }

    public static void SendMessage(string username, string message)
    {
        using (Packet _packet = new Packet((int)ClientPackets.message))
        {
            _packet.Write(username);
            _packet.Write(message);

            SendTCPData(_packet);
        }
    }

    public static void DanceMusic(int _id)
    {
        using (Packet _packet = new Packet((int)ClientPackets.danceMusic))
        {
            _packet.Write(_id);
            SendUDPData(_packet);
        }
    }

    #endregion
}
