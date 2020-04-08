using Unity.Entities;
using UnityEngine;
using System.Collections.Generic;
using Unity.Transforms;
using Unity.Mathematics;
using CodeMonkey.Utils;
using Unity.Collections;

public class EntitySpawner : ComponentSystem
{
    Entity a, b, c, d, e, f, g;
    private Entity[] l;
    private System.Random rand = new System.Random();
    private Transform shellPosition;
    private bool flag = false;
    private playerAimWeapon aim;
    private RaycastHit2D hit;
    private Vector3 hitDirection;
    private Vector3 reverseHitDirection;
    private flyingBody fl;
    private Transform en;
    private float slowDownFactor=5f;



    protected override void OnStartRunning()
    {
        Debug.Log("running");
        shellPosition = GameObject.Find("ShellPosition").GetComponent<Transform>();
        aim = GameObject.Find("Player").GetComponent<playerAimWeapon>();
        fl = GameObject.Find("Enemy").GetComponent<flyingBody>();
        en = GameObject.Find("Enemy").GetComponent<Transform>();
        

    }
    protected override void OnUpdate()
    {
        //Debug.Log(fl.spawnLotsOfBlood);
        if(fl)
        {
            fl.isHit = false;

        }
        //if the enemy is dead, blow him away leaving a trail of blood
        if (en && fl.spawnLotsOfBlood)
        {
            Entities.ForEach((ref bloodToEntity pr) =>
            {
                a = pr.prefabEntity0;
                b = pr.prefabEntity1;
                c = pr.prefabEntity2;
                d = pr.prefabEntity3;
                e = pr.prefabEntity4;
                f = pr.prefabEntity5;
                g = pr.prefabEntity6;
                l = new Entity[] { a, b, c, d, e, f, g };
                for (int i = 0; i < 20; i++)
                {
                    Entity spawnedEntityBlood = EntityManager.Instantiate(l[rand.Next(l.Length)]);
                    EntityManager.AddComponent(spawnedEntityBlood, typeof(MoveSpeedComponent));
                    EntityManager.AddComponent(spawnedEntityBlood, typeof(RotationEulerXYZ));
                    var go = new GameObject();
                    var col = go.AddComponent<BoxCollider2D>();
                    EntityManager.AddComponentObject(spawnedEntityBlood, col);
                    EntityManager.SetComponentData(spawnedEntityBlood, new Translation
                    {
                        Value = en.transform.position + new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f), 0f)
                    });
                    EntityManager.SetComponentData(spawnedEntityBlood, new MoveSpeedComponent
                    {
                        moveSpeed = UnityEngine.Random.Range(900f,1000f)
                    }) ;
                    EntityManager.SetComponentData(spawnedEntityBlood, new RotationEulerXYZ
                    {
                        Value = new float3(0, 0, UnityEngine.Random.Range(0, 360f))

                    });
                }
                
                l = new Entity[] { };

            });

            Entities.ForEach((ref Translation tr, ref MoveSpeedComponent moveSpeedComponent, ref RotationEulerXYZ r) =>
            {
                if (moveSpeedComponent.moveSpeed > 0f)
                {
                    tr.Value.y += reverseHitDirection.y * moveSpeedComponent.moveSpeed * Time.DeltaTime;
                    //tr.Value.z += hitDirection.z * moveSpeedComponent.moveSpeed * Time.DeltaTime;
                    tr.Value.x += reverseHitDirection.x * moveSpeedComponent.moveSpeed * Time.DeltaTime;
                    r.Value.z += 360f * (moveSpeedComponent.moveSpeed / 40f) * Time.DeltaTime;
                }
                moveSpeedComponent.moveSpeed -= moveSpeedComponent.moveSpeed * 4f * Time.DeltaTime;
            });



        }




        // if the enemy is just hit on mouse click

        else if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("pressed");
            hit = Physics2D.Raycast(aim.gunEndPointTransform.position, aim.gunEndPointTransform.right);
            if (hit)
            {
                Debug.Log("hit");
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    fl.isHit = true;
                    hitDirection = (en.transform.position - aim.gunEndPointTransform.position).normalized;
                    reverseHitDirection = (aim.gunEndPointTransform.position - en.transform.position).normalized;
                    flag = true;
                    float3 mousePosition = UtilsClass.GetMouseWorldPosition();
                    Entities.ForEach((ref bloodToEntity pr) =>
                    {
                        a = pr.prefabEntity0;
                        b = pr.prefabEntity1;
                        c = pr.prefabEntity2;
                        d = pr.prefabEntity3;
                        e = pr.prefabEntity4;
                        f = pr.prefabEntity5;
                        g = pr.prefabEntity6;
                        l = new Entity[] { a, b, c, d, e, f, g };
                        for (int i = 0; i < 10; i++)
                        {
                            Entity spawnedEntityBlood = EntityManager.Instantiate(l[rand.Next(l.Length)]);
                            EntityManager.AddComponent(spawnedEntityBlood, typeof(MoveSpeedComponent));
                            EntityManager.AddComponent(spawnedEntityBlood, typeof(RotationEulerXYZ));
                            EntityManager.SetComponentData(spawnedEntityBlood, new Translation
                            {
                                Value = en.transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1.5f, 1.5f), 0f)
                            });
                            EntityManager.SetComponentData(spawnedEntityBlood, new MoveSpeedComponent
                            {
                                moveSpeed = UnityEngine.Random.Range(20f,30f)
                            });
                        }
                        
                        l = new Entity[] { };
                        
                    });
                    Debug.Log("enemy hit");
                }
            }


            //spawn shell entities
            Entities.ForEach((ref shellToEntity sh) =>
            {

                Entity spawnedEntityShell = EntityManager.Instantiate(sh.shell);
                EntityManager.SetComponentData(spawnedEntityShell, new Translation
                {
                    Value = shellPosition.position

                });
            });

            // blood particles animation
            
        }

        if (flag)
        {
            Entities.ForEach((ref Translation tr, ref MoveSpeedComponent moveSpeedComponent) =>
            {
                if (moveSpeedComponent.moveSpeed > 0f)
                {
                    tr.Value.y += hitDirection.y * moveSpeedComponent.moveSpeed * Time.DeltaTime;
                    //tr.Value.z += hitDirection.z * moveSpeedComponent.moveSpeed * Time.DeltaTime;
                    tr.Value.x += hitDirection.x * moveSpeedComponent.moveSpeed * Time.DeltaTime;
                }
                moveSpeedComponent.moveSpeed -= moveSpeedComponent.moveSpeed * slowDownFactor * Time.DeltaTime;
            });
        }




    }
}
