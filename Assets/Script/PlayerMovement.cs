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

public partial class PlayerMovement : SystemBase
{
    private PlayerInput Input;
    private Entity Player;


    protected override void OnCreate()
    {
        RequireForUpdate<PlayerMovementInfo>();

        Input = new PlayerInput();
        Input.Enable();

        Input.PlayerActions.Shoot.performed += SpawnBullet;
    }

    protected override void OnStartRunning()
    {
        Player = SystemAPI.GetSingletonEntity<PlayerTag>();
    }

    [BurstCompile]
    private void SpawnBullet(InputAction.CallbackContext callbackContext)
    {
        foreach ((RefRW<ShootBullet> shootThisFrame, RefRO<PlayerTag> tag) in
                 SystemAPI.Query<RefRW<ShootBullet>, RefRO<PlayerTag>>())
        {
            shootThisFrame.ValueRW.shoot = true;
        }
    }


    [BurstCompile]
    protected override void OnUpdate()
    {

        float2 MoveInput = this.Input.PlayerActions.MoveAction.ReadValue<Vector2>();

        //foreach ((RefRW<LocalTransform> transform, RefRO<PlayerMovementInfo> playerMovementSpeed) in
        //         SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerMovementInfo>>())
        //{
        //    transform.ValueRW.Position.xy += transform.ValueRW.Up().xy *
        //        playerMovementSpeed.ValueRO.speed * MoveInput.y * SystemAPI.Time.DeltaTime;


        //    quaternion rotation = transform.ValueRO.Rotation;
        //    rotation = math.mul(rotation, quaternion.AxisAngle(new float3(0, 0, 1),
        //        MoveInput.x * SystemAPI.Time.DeltaTime * -playerMovementSpeed.ValueRO.RotationSpeed));
        //    transform.ValueRW.Rotation = rotation;

        //    SystemAPI.SetSingleton(new PlayerPos { pos = transform.ValueRO.Position });
        //}


        PlayerMovementJob playerMovementJob = new PlayerMovementJob
        {
            MoveInput = this.Input.PlayerActions.MoveAction.ReadValue<Vector2>(),
            DeltaTime = SystemAPI.Time.DeltaTime
        };

        playerMovementJob.Schedule();
    }

    public partial struct PlayerMovementJob : IJobEntity
    {
        public float2 MoveInput;
        public float DeltaTime;
        public void Execute(ref LocalTransform transform, in PlayerMovementInfo playerMovement, ref PlayerPos playerPos)
        {
            transform.Position.xy += transform.Up().xy *
                playerMovement.speed * MoveInput.y * DeltaTime;


            quaternion rotation = transform.Rotation;
            rotation = math.mul(rotation, quaternion.AxisAngle(new float3(0, 0, 1),
                MoveInput.x * DeltaTime * -playerMovement.RotationSpeed));
            transform.Rotation = rotation;

            playerPos.pos = transform.Position;
        }
    }
}
