using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    private SpawnPoint[] _spawnPoints;

    private void Awake()
    {
        instance = this;
        _spawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

    public Transform GetSpawnPoint(int spawnPointNumber)
    {
        return _spawnPoints[spawnPointNumber].transform;
    }
}
