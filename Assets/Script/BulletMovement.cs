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

public partial struct BulletMovement : ISystem
{

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BulletInfo>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer buffer = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);

        foreach ((RefRW<LocalTransform> bulletTransform, RefRW<BulletInfo> bulletInfo, Entity entity) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRW<BulletInfo>>().WithEntityAccess())
        {
            bulletInfo.ValueRW.timer -= SystemAPI.Time.DeltaTime;

            bulletTransform.ValueRW.Position += bulletTransform.ValueRO.Up() * bulletInfo.ValueRO.speed * SystemAPI.Time.DeltaTime;

            if (bulletInfo.ValueRO.timer < 0)
            {
                buffer.DestroyEntity(entity);
            }
        }

        buffer.Playback(state.EntityManager);
        buffer.Dispose();
    }
}
