using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : Singleton<GameManager>
{

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    [SerializeField] private Camera orthoCamera;
    [SerializeField] private VideoPlayer vp;

    public Camera OrthoCamera { get => orthoCamera; }

    // Start is called before the first frame update
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.Instance.myId)
        {
            if (_username == "Sezen Aksu")
            {
                _player = Instantiate(localPlayerPrefab, new Vector3(0,4,150), Quaternion.Euler(new Vector3(0,180,0)));
                //_player.transform.localScale = new Vector3(4, 4, 4);
                UIManager.Instance.DanceBtnFunc(_player.GetComponent<Movement>());

            }
            else
            {
                _player = Instantiate(localPlayerPrefab, _position, _rotation);
                UIManager.Instance.DanceBtnFunc(_player.GetComponent<Movement>());
            }
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
            vp.Play();

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
