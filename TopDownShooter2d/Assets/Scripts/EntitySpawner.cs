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
    private Vector3 hitDirection;
    private Vector3 reverseHitDirection;
    private flyingBody fl;
    private Transform en;
    private float slowDownFactor=5f;
    private SoundManager s;

    public RaycastHit2D hit;
    public static Dictionary<string, GameObject> gameDict;




    protected override void OnStartRunning()
    {
        
        s = GameObject.Find("SoundManager").GetComponent<SoundManager>();
       
        gameDict = new Dictionary<string, GameObject>()
        {
            {"Enemy", GameObject.Find("Enemy") },
            {"Enemy (1)", GameObject.Find("Enemy (1)") },
            {"Enemy (2)", GameObject.Find("Enemy (2)") },
            {"Enemy (3)", GameObject.Find("Enemy (3)") },
        };
    
        shellPosition = GameObject.Find("ShellPosition").GetComponent<Transform>();
        aim = GameObject.Find("Player").GetComponent<playerAimWeapon>();
        //fl = GameObject.Find("Enemy").GetComponent<flyingBody>();
        //en = GameObject.Find("Enemy").transform.Find("bloodSpawner").GetComponent<Transform>();
        

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
                for (int i = 0; i < 25; i++)
                {
                    Entity spawnedEntityBlood = EntityManager.Instantiate(l[rand.Next(l.Length)]);
                    EntityManager.AddComponent(spawnedEntityBlood, typeof(MoveSpeedComponent));
                    EntityManager.AddComponent(spawnedEntityBlood, typeof(RotationEulerXYZ));
                    EntityManager.SetComponentData(spawnedEntityBlood, new Translation
                    {
                        Value = en.position + new Vector3( UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f,2f), 0f)
                    }); ;
                    EntityManager.SetComponentData(spawnedEntityBlood, new MoveSpeedComponent
                    {
                        moveSpeed = UnityEngine.Random.Range(10f,30f)
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
                    tr.Value.y += hitDirection.y * moveSpeedComponent.moveSpeed * Time.DeltaTime;
                    //tr.Value.z += hitDirection.z * moveSpeedComponent.moveSpeed * Time.DeltaTime;
                    tr.Value.x += hitDirection.x * moveSpeedComponent.moveSpeed * Time.DeltaTime;
                    r.Value.z += 360f * (moveSpeedComponent.moveSpeed / 5f) * Time.DeltaTime;
                }
                moveSpeedComponent.moveSpeed -= moveSpeedComponent.moveSpeed * 3f * Time.DeltaTime;
            });



        }

        //else if (!fl.spawnLotsOfBlood)
        //{
        //    Entities.ForEach((ref Translation tr, ref MoveSpeedComponent moveSpeedComponent, ref RotationEulerXYZ r) =>
        //    {
        //        tr.Value.y += 0f;
        //        tr.Value.x += 0f;
                
        //    });
        //}


        // if the enemy is just hit on mouse click

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("pressed");
            hit = Physics2D.Raycast(aim.gunEndPointTransform.position, aim.gunEndPointTransform.right);
            if (hit)
            {
                Debug.Log("hit");
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                
                //Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.tag=="enemy")
                {
                    
                    fl = gameDict[hit.transform.gameObject.name].GetComponent<flyingBody>();
                    en = gameDict[hit.transform.gameObject.name].transform.Find("bloodSpawner").GetComponent<Transform>();
                    fl.isHit = true;
                    hitDirection = (en.position - aim.gunEndPointTransform.position).normalized;
                    reverseHitDirection = (aim.gunEndPointTransform.position - en.position).normalized;
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
                                Value = en.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1.2f, -0.3f), 0f)
                            });
                            EntityManager.SetComponentData(spawnedEntityBlood, new MoveSpeedComponent
                            {
                                moveSpeed = UnityEngine.Random.Range(10f,15f)
                            });
                        }
                        
                        l = new Entity[] { };
                        s.PlayOneShot("Hit");

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
