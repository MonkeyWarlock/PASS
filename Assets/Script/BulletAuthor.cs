using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BulletAuthor : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletTimer;

    private class BulletAuthorBaker : Baker<BulletAuthor>
    {
        public override void Bake(BulletAuthor authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BulletInfo() { speed = authoring.bulletSpeed, timer = authoring.bulletTimer });
        }
    }
}

public struct BulletInfo : IComponentData
{
    public float speed;
    public float timer;
}