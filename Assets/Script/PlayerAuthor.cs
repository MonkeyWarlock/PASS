using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAuthor : MonoBehaviour
{
    public float playerSpeed;
    public float playerRotationSpeed;

    public GameObject bulletPrefab;

    private class PlayerAuthorBaker : Baker<PlayerAuthor>
    {
        public override void Bake(PlayerAuthor authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerMovementInfo() { speed = authoring.playerSpeed, RotationSpeed = authoring.playerRotationSpeed});

            AddComponent(entity, new PlayerPos());

            AddComponent(entity, new PlayerTag());

            AddComponent(entity, new BulletPrefab { bullet = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic) });

            AddComponent(entity, new ShootBullet { shoot = false });


        }
    }
}
public struct PlayerMovementInfo : IComponentData
{
    public float speed;
    public float RotationSpeed;
}

public struct PlayerPos : IComponentData
{
    public float3 pos;
}

public struct PlayerTag : IComponentData
{

}

public struct BulletPrefab : IComponentData
{
    public Entity bullet;
}

public struct ShootBullet : IComponentData
{
    public bool shoot;
}
