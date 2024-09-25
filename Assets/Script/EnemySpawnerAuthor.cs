using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawnerAuthor : MonoBehaviour
{
    public GameObject EnemyPrefab;

    public float SpawnRate;

    public float2 SpawnPos;

    class EnemySpawnerBaker : Baker<EnemySpawnerAuthor>
    {
        public override void Bake(EnemySpawnerAuthor authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new EnemySpawnerData
            {
                prefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
                SpawnPos = authoring.SpawnPos,
                NextSpawmTime = 0,
                SpawnRate = authoring.SpawnRate
            });
        }
    }
}
public struct EnemySpawnerData : IComponentData
{
    public Entity prefab;
    public float2 SpawnPos;
    public float NextSpawmTime;
    public float SpawnRate;
}
