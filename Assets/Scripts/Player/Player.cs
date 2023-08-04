using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using TMPro;

public class Player : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;

    private CameraManager _cameraManager;

    private bool _camAnimEnded = false;

    public NetworkVariable<FixedString32Bytes> Name = new NetworkVariable<FixedString32Bytes>();
    public event System.Action<FixedString32Bytes> ChangedNameEvent;
    private TextMeshPro _nameTMP;

    [SerializeField] private Transform Player1Position;
    [SerializeField] private Transform Player2Position;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) RespawnServerRpc();
        ChooseSkin();
        Name.OnValueChanged += NameChanged;
        _nameTMP = GetComponentInChildren<TextMeshPro>();
        if (IsOwner)
        {
            setName();
            _nameTMP.gameObject.SetActive(false);
        }
    }

    private void CamAnimStart(Transform trans, Transform playerTrans)
    {
        _cameraManager = Camera.main.GetComponent<CameraManager>();
        _cameraManager.SetPlayerReference(trans, playerTrans);
    }

    [ServerRpc]
    private void RespawnServerRpc(ServerRpcParams serverRpcParams = default)
    {
        RespawnClientRpc();
    }
    [ClientRpc]
    private void RespawnClientRpc()
    {
        if (!IsOwner) return;
        Player1Position = GameObject.FindGameObjectWithTag("P1S").transform;
        Player2Position = GameObject.FindGameObjectWithTag("P2S").transform;
        switch (playersInGame())
        {
            case 1:
                transform.position = Player1Position.position;
                CamAnimStart(Player1Position, transform);
                break;
            case 2:
                transform.position = Player2Position.position;
                CamAnimStart(Player2Position, transform);
                break;
        }

    }

    private void NameChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        ChangedNameEvent?.Invoke(newValue.ToString());
    }

    private void OnEnable()
    {
        ChangedNameEvent += ChangeNameTMP;
    }

    private void OnDisable()
    {
        ChangedNameEvent -= ChangeNameTMP;
    }

    private void ChangeNameTMP(FixedString32Bytes value)
    {
        _nameTMP.text = value.ToString();
    }

    private void setName()
    {
        string _nameInPrefs = PlayerPrefs.GetString("Name");
        _nameInPrefs = string.IsNullOrEmpty(_nameInPrefs) ? "He Who Must Not Be Named" : _nameInPrefs;

        setNameServerRpc(_nameInPrefs);
    }

    [ServerRpc(RequireOwnership = false)]
    public void setNameServerRpc(FixedString32Bytes _name)
    {
        Name.Value = _name;
    }

    private void ChooseSkin()
    {
        GameObject skin1 = transform.Find("Skin1").gameObject;
        GameObject skin2 = transform.Find("Skin2").gameObject;

        if (playersInGame() == 1)
        {
            skin1.SetActive(true);
            skin2.SetActive(false);
            return;
        }
        skin1.SetActive(false);
        skin2.SetActive(true);
    }

    private int playersInGame()
    {
        string targetTagPlayer = "Player";
        GameObject[] playersWithSameName = GameObject.FindGameObjectsWithTag(targetTagPlayer);
        return playersWithSameName.Length;
    }

    void Update()
    {
        if (_nameTMP.text != Name.Value.ToString())
        {
            _nameTMP.text = Name.Value.ToString();
        }

        if (!IsOwner || !_camAnimEnded) return;

        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");

        if (_verticalInput != 0f)
        {
            Vector3 moveDirection = transform.forward * _verticalInput;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
        if (_horizontalInput != 0f)
        {
            Vector3 rotationDirection = Vector3.up * _horizontalInput;
            transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
        }
    }

    public void EndCamAnim()
    {
        _camAnimEnded = true;
    }
}
