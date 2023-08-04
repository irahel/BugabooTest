using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public NetworkVariable<bool> _timerStarted = new(false);
    public static event System.Action<bool> StartTimerEvent;

    [SerializeField] private TextMeshProUGUI rankOne;
    [SerializeField] private TextMeshProUGUI rankTwo;

    private void Awake()
    {
        Invoke("startNetworkConfiguration", 1f);
    }
    private void startNetworkConfiguration()
    {
        int networkType = PlayerPrefs.GetInt("NetworkType");
        switch (networkType)
        {
            case 0:
                NetworkManager.Singleton.StartHost();
                break;
            case 1:
                NetworkManager.Singleton.StartClient();
                break;
        }
    }

    public override void OnNetworkSpawn()
    {
        _timerStarted.OnValueChanged += TimerChanged;
    }

    private void OnEnable()
    {
        Timer.TimerOverEvent += EndGameServerRpc;
    }

    private void OnDisable()
    {
        Timer.TimerOverEvent -= EndGameServerRpc;
    }

    [ServerRpc]
    private void EndGameServerRpc(bool value)
    {
        EndGameClientRpc();
    }

    [ClientRpc]
    private void EndGameClientRpc()
    {
        string targetTagPlayer = "Player";
        GameObject[] playersWithSameName = GameObject.FindGameObjectsWithTag(targetTagPlayer);

        PlayerPrefs.DeleteAll();
        string prefix = NetworkManager.Singleton.LocalClientId.ToString();

        //The player that has 1 more child is the local player, because of the camera
        if (playersWithSameName[0].GetComponentsInChildren<Transform>(true).Length < playersWithSameName[1].GetComponentsInChildren<Transform>(true).Length)
        {
            PlayerPrefs.SetInt(prefix + "-EnemyScore", playersWithSameName[0].GetComponent<PlayerScore>().score.Value);
            PlayerPrefs.SetInt(prefix + "-MyScore", playersWithSameName[1].GetComponent<PlayerScore>().score.Value);

            PlayerPrefs.SetString(prefix + "-EnemyName", playersWithSameName[0].GetComponent<Player>().Name.Value.ToString());
            PlayerPrefs.SetString(prefix + "-MyName", playersWithSameName[1].GetComponent<Player>().Name.Value.ToString());
        }
        else
        {
            PlayerPrefs.SetInt(prefix + "-EnemyScore", playersWithSameName[1].GetComponent<PlayerScore>().score.Value);
            PlayerPrefs.SetInt(prefix + "-MyScore", playersWithSameName[0].GetComponent<PlayerScore>().score.Value);

            PlayerPrefs.SetString(prefix + "-EnemyName", playersWithSameName[1].GetComponent<Player>().Name.Value.ToString());
            PlayerPrefs.SetString(prefix + "-MyName", playersWithSameName[0].GetComponent<Player>().Name.Value.ToString());
        }
        PlayerPrefs.Save();
        SceneManager.LoadScene("EndGame");
    }

    public void Update()
    {
        if (!IsHost || _timerStarted.Value) return;

        if (NetworkManager.Singleton.ConnectedClients.Count == 2) StartTimerServerRpc();
    }

    [ServerRpc]
    private void StartTimerServerRpc()
    {
        _timerStarted.Value = true;
    }

    private void TimerChanged(bool previousValue, bool newValue)
    {
        StartTimerEvent?.Invoke(_timerStarted.Value);
    }
}
