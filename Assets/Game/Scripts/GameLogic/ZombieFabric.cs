using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

public class ZombieFabric : MonoBehaviour
{
    [Serializable]
    public struct Item
    {
        [Tooltip("Префаб зомби")]
        public GameObject prefub;

        [Tooltip("Вероятность появления")]
        [Range(0f, 1f)]
        public float probability;
    }


    private GameObject[] _zombieSpawnPoints;
    private GameState _gameState;

    [Tooltip("Разновидности зомби")]
    public List<Item> zombies = new List<Item>();

    [Tooltip("Радиус зоны спавна вокруг точки спавна")]
    public float spawnRadius = 0.5f;

    void Start()
    {
        zombies = zombies.OrderBy(x => x.probability).ToList();
        _gameState = GameObject.Find("GameState").GetComponent<GameState>();
        _zombieSpawnPoints = GameObject.FindGameObjectsWithTag("ZombieSpawn");

        CreateZombie();
    }

    void CreateZombie()
    {
        float randomValue = Random.value;
        Vector3 spawnArea = Random.insideUnitCircle * spawnRadius;
        int spawnPointId = Random.Range(0,_zombieSpawnPoints.Length);

        GameObject newPrefub = zombies.Last().prefub;
        foreach (var zombie in zombies)
        {
            if (randomValue <= zombie.probability)
            {
                newPrefub = zombie.prefub;
                break;
            }
        }

        Instantiate(newPrefub, _zombieSpawnPoints[spawnPointId].transform.position + spawnArea, Quaternion.identity);

        Invoke(nameof(CreateZombie), _gameState.ZombieSpawnInterval);

        Debug.Log($"Instantiate zombie: \"{newPrefub.name}\"");
    }


}
