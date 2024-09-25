using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = System.Numerics.Quaternion;

public partial struct PlayerShooting : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<LocalTransform> transform, RefRO<BulletPrefab> bulletPrefab, RefRW<ShootBullet> shootBullet) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<BulletPrefab>, RefRW<ShootBullet>>())
        {
            if (shootBullet.ValueRO.shoot)
            {
                Entity newBullet = state.EntityManager.Instantiate(bulletPrefab.ValueRO.bullet);

                LocalTransform BulletTransform = transform.ValueRO;

                state.EntityManager.SetComponentData(newBullet, BulletTransform);


                shootBullet.ValueRW.shoot = false;
            }

        }
    }
}
