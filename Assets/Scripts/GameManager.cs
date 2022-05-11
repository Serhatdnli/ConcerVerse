using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    [SerializeField] private Camera orthoCamera;

    public Camera OrthoCamera { get => orthoCamera; }

    // Start is called before the first frame update
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.Instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
            UIManager.Instance.DanceBtnFunc(_player.GetComponent<Movement>());
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        PlayerManager pm = _player.GetComponent<PlayerManager>();
        pm.id = _id;
        pm.username = _username;
        Client.Instance.username = _username;//düzelebilir
        players.Add(_id, pm);

    }
}

public enum PlayerStates
{
    Listening,
    Walking,
    Dancing
}
