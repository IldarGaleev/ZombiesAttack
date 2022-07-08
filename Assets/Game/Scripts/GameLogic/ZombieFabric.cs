using Assets.Game.Scripts.Helpers;
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
        public Zombie prefub;

        [Tooltip("Вероятность появления")]
        [Range(0f, 1f)]
        public float probability;
    }


    private GameObject[] _zombieSpawnPoints;
    private GameState _gameState;
    private Dictionary<Zombie, ObjectsPool<Zombie>> _zombiesPool = new Dictionary<Zombie, ObjectsPool<Zombie>>();

    [Tooltip("Разновидности зомби")]
    public List<Item> zombies = new List<Item>();

    [Tooltip("Радиус зоны спавна вокруг точки спавна")]
    public float spawnRadius = 0.5f;

    void Start()
    {
        zombies = zombies.OrderBy(x => x.probability).ToList();
        _gameState = GameObject.Find("GameState").GetComponent<GameState>();
        _zombieSpawnPoints = GameObject.FindGameObjectsWithTag("ZombieSpawn");

        foreach(Item item in zombies)
        {
            _zombiesPool.Add(item.prefub, new ObjectsPool<Zombie>(item.prefub, 3, this.transform));
        }

        CreateZombie();
    }

    void CreateZombie()
    {
        float randomValue = Random.value;
        Vector3 spawnArea = Random.insideUnitCircle * spawnRadius;
        int spawnPointId = Random.Range(0,_zombieSpawnPoints.Length);

        Zombie newPrefub = zombies.Last().prefub;
        foreach (var zombie in zombies)
        {
            if (randomValue <= zombie.probability)
            {
                newPrefub = zombie.prefub;
                break;
            }
        }

        var activeZombie = _zombiesPool[newPrefub].InstantiateToScene(_zombieSpawnPoints[spawnPointId].transform.position + spawnArea, Quaternion.identity);
        activeZombie.Restore(newPrefub);

        Invoke(nameof(CreateZombie), _gameState.ZombieSpawnInterval);

        Debug.Log($"Instantiate zombie: \"{newPrefub.name}\"");
    }


}
