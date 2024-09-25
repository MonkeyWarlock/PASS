using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public partial struct EnemyMovement : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyInfo>();
        state.RequireForUpdate<LocalTransform>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //Vector3 playerPos = SystemAPI.GetSingleton<PlayerPos>().pos;

        //foreach ((RefRW<LocalTransform> enemyTransform, RefRO<EnemyInfo> enemyInfo) in
        //         SystemAPI.Query<RefRW<LocalTransform>, RefRO<EnemyInfo>>())
        //{
        //    Vector3 Movedirection = playerPos - (Vector3)enemyTransform.ValueRO.Position;

        //    Movedirection.Normalize();

        //    enemyTransform.ValueRW.Position += new float3(Movedirection * enemyInfo.ValueRO.speed * SystemAPI.Time.DeltaTime);
        //}

        EnemyMoveJob enemyMoveJob = new EnemyMoveJob
        {
            pos = SystemAPI.GetSingleton<PlayerPos>().pos,
            DeltaTime = SystemAPI.Time.DeltaTime
        };

        enemyMoveJob.Schedule();
    }

    public partial struct EnemyMoveJob : IJobEntity
    {
        public Vector3 pos;
        public float DeltaTime;

        public void Execute(ref LocalTransform transform, in EnemyInfo enemyInfo)
        {
            Vector3 Movedirection = pos - (Vector3)transform.Position;

            Movedirection.Normalize();

            transform.Position += new float3(Movedirection * enemyInfo.speed * DeltaTime);
        }
    }
}
