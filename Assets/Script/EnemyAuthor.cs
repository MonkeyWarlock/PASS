using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAuthor : MonoBehaviour
{
    //public GameObject EnemyPrefab;

    //public float SpawnRate;

    //public float2 SpawnPos;

    public float EnemySpeed;

    class EnemyAuthorBaker : Baker<EnemyAuthor>
    {
        public override void Bake(EnemyAuthor authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            //AddComponent(entity, new EnemySpawnerData
            //{
            //    prefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
            //    SpawnPos = authoring.SpawnPos,
            //    NextSpawmTime = 0,
            //    SpawnRate = authoring.SpawnRate
            //});

            AddComponent(entity, new EnemyInfo { speed = authoring.EnemySpeed });
        }
    }
}
//public struct EnemySpawnerData : IComponentData
//{
//    public Entity prefab;
//    public float2 SpawnPos;
//    public float NextSpawmTime;
//    public float SpawnRate;
//}
public struct EnemyInfo : IComponentData
{
    public float speed;
}