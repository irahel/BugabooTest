using UnityEngine;
using Unity.Netcode;
using System;

public class Spawn : NetworkBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Vector2 spawnIntervalRange = new Vector2(1f, 3f);

    [SerializeField] private Transform minSpawnX;
    [SerializeField] private Transform maxSpawnX;
    [SerializeField] private Transform minSpawnZ;
    [SerializeField] private Transform maxSpawnZ;

    private Vector2 _minSpawnArea;
    private Vector2 _maxSpawnArea;

    private bool _isSpawnEnabled = false;

    public override void OnNetworkSpawn()
    {
        if (!IsHost) Destroy(gameObject);
    }

    public void Update()
    {
        if (_isSpawnEnabled || !IsHost) return;

        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            _isSpawnEnabled = true;
            StartSpawn();
        }
    }

    private void StartSpawn()
    {
        _minSpawnArea = GetSpawnAreaVector(minSpawnX, minSpawnZ);
        _maxSpawnArea = GetSpawnAreaVector(maxSpawnX, maxSpawnZ);

        float randomTime = UnityEngine.Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
        Invoke("SpawnDelay", randomTime);
    }

    private void SpawnDelay()
    {
        Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(_minSpawnArea.x, _maxSpawnArea.x), 0f, UnityEngine.Random.Range(_minSpawnArea.y, _maxSpawnArea.y));
        GameObject go = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();

        float randomTime = UnityEngine.Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
        Invoke("SpawnDelay", randomTime);
    }


    private Vector2 GetSpawnAreaVector(Transform pointX, Transform pointZ)
    {
        return new Vector2(pointX.position.x, pointZ.position.z);
    }
}
