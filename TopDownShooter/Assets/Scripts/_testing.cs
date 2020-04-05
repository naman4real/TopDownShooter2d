using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

public class _testing : MonoBehaviour
{
    [SerializeField] private Mesh mesh;
    [SerializeField]private  Material material;
    private void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype entityArchetype = entityManager.CreateArchetype(
          //typeof(Scale),
          typeof(RenderBounds),
          //typeof(WorldRenderBounds),
          typeof(LocalToWorld),
          typeof(Translation),
          typeof(RenderMesh)
          
          //typeof(ChunkWorldRenderBounds)


        );
        NativeArray<Entity> entityArray = new NativeArray<Entity>(1, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entityArray);
        for (int i = 0; i < entityArray.Length; i++)
        {
            Entity entity = entityArray[i];
            entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = mesh,
                material = material,

            });

        }
        entityArray.Dispose();

            


         
    }
}
