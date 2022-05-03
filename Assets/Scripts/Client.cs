using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Client : Singleton<Client>
{
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 7777;
    public int myId = 0;
    public TCP tcp;
    public UDP udp;

    public bool isConnected = false;
    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Start()
    {
        tcp = new TCP();
        udp = new UDP();
    }
    public void ConnectedToServer()
    {
        InitializeClientData();
        isConnected = true;
        tcp.Connect();
    }
    public void OnApplicationQuit()
    {
        Disconnect();
    }
    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize

            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(Instance.ip, Instance.port, ConnectCallBack, socket);
        }

        private void ConnectCallBack(IAsyncResult _result)
        {
            socket.EndConnect(_result);
            if (!socket.Connected)
            {
                return;
            }
            else
            {
                stream = socket.GetStream();

                receivedData = new Packet();
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallBack, null);
            }
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null
                        , null);
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Error sending data to server via TCP:{ex}");
                throw;
            }
        }

        private void ReceiveCallBack(IAsyncResult _result)
        {
            try
            {
                int _byteLenght = stream.EndRead(_result);
                if (_byteLenght <= 0)
                {
                    Instance.Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLenght];
                Array.Copy(receiveBuffer, _data, _byteLenght);

                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallBack, null);
            }
            catch (Exception _ex)
            {
                Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }
        private void Disconnect()
        {
            Instance.Disconnect();
            stream = null;
            receiveBuffer = null;
            receivedData = null;
            socket = null;
        }

    }
    public class UDP
    {
        public UdpClient socket;
        public IPEndPoint endPoint;

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(Instance.ip), Instance.port);
        }

        public void Connect(int _localPort)
        {
            socket = new UdpClient(_localPort);

            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallBack, null);

            using (Packet _packet = new Packet())
            {
                SendData(_packet);
            }
        }

        public void SendData(Packet _packet)
        {
            try
            {
                _packet.InsertInt(Instance.myId);
                if (socket != null && Instance.isConnected)
                {
                    socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                }

            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via UDP: {_ex}");
            }
        }

        public void ReceiveCallBack(IAsyncResult _result)
        {
            try
            {
                byte[] data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallBack, null);

                if (data.Length < 4)
                {
                    Instance.Disconnect();
                    return;
                }
                HandleData(data);
            }
            catch
            {
                Disconnect();
            }
        }

        private void HandleData(byte[] _data)
        {
            using (Packet _packet = new Packet(_data))
            {
                int packetLength = _packet.ReadInt();
                _data = _packet.ReadBytes(packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_data))
                {
                    int packetId = _packet.ReadInt();
                    Debug.Log(packetId + "  " + packetHandlers.Count);
                    packetHandlers[packetId](_packet);
                }
            });
        }
        private void Disconnect()
        {
            Instance.Disconnect();
            endPoint = null;
            socket = null;
        }
    }
    public void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ServerPackets.welcome, ClientHandle.Welcome },
                { (int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer},
                { (int)ServerPackets.playerPosition, ClientHandle.PlayerPosition},
                { (int)ServerPackets.playerRotation, ClientHandle.PlayerRotation}
            };
        print("Initialize Packets.");
    }
    private void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            tcp.socket.Close();
            udp.socket.Close();

            Debug.Log("Disconnected From Server.");
        }
    }
}
