/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this Code Monkey project
    I hope you find it useful in your own projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class HowToBulletTracer : MonoBehaviour {
    [SerializeField] playerAimWeapon p;
    [SerializeField] private Material weaponTracerMaterial;
    [SerializeField] private Sprite shootFlashSprite;
    [SerializeField] private Sprite shellSprite;

    private void Start()
    {
        p.OnShoot+=Shoot;
    }
    private void Shoot(object sender, playerAimWeapon.OnShootEventArgs e) {
        CreateWeaponTracer(e.gunEndPointPosition, e.shootPosition);       
        ShakeCamera(.15f, .05f);
        //CreateShootFlash(e.gunEndPointPosition);
        //SpawnShells(e.shellPosition);
    }



    private void SpawnShells(Vector3 shellPosition)
    {
        World_Sprite worldSprite = World_Sprite.Create(shellPosition, shellSprite);
    }
    private void CreateShootFlash(Vector3 spawnPosition) {
        World_Sprite worldSprite = World_Sprite.Create(spawnPosition, shootFlashSprite);
        FunctionTimer.Create(worldSprite.DestroySelf, .05f);
    }

    private void CreateWeaponTracer(Vector3 fromPosition, Vector3 targetPosition) {
        Vector3 dir = (targetPosition - fromPosition).normalized;
        float eulerZ = UtilsClass.GetAngleFromVectorFloat(dir) - 90;
        float distance = Vector3.Distance(fromPosition, targetPosition);
        Vector3 tracerSpawnPosition = fromPosition + dir * distance * .5f;
        Material tmpWeaponTracerMaterial = new Material(weaponTracerMaterial);
        tmpWeaponTracerMaterial.SetTextureScale("_MainTex", new Vector2(1f, distance / 128f));
        World_Mesh worldMesh = World_Mesh.Create(tracerSpawnPosition, eulerZ, 0.8f, distance, tmpWeaponTracerMaterial, null, 10000);

        int frame = 0;
        float framerate = .016f;
        float timer = framerate;
        worldMesh.SetUVCoords(new World_Mesh.UVCoords(0, 0, 16, 256));
        FunctionUpdater.Create(() =>
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                frame++;
                timer += framerate;
                if (frame >= 4)
                {
                    worldMesh.DestroySelf();
                    return true;
                }
                else
                {
                    worldMesh.SetUVCoords(new World_Mesh.UVCoords(16 * frame, 0, 16, 256));
                }
            }
            return false;
        });
    }



    public static void ShakeCamera(float intensity, float timer) {
        Vector3 lastCameraMovement = Vector3.zero;
        FunctionUpdater.Create(delegate () {
            timer -= Time.unscaledDeltaTime;
            Vector3 randomMovement = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * intensity;
            Camera.main.transform.localPosition = Camera.main.transform.localPosition - lastCameraMovement + randomMovement;
            lastCameraMovement = randomMovement;
            return timer <= 0f;
        }, "CAMERA_SHAKE");
    }

}
