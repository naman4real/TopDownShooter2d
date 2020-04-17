using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingBody : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isHit = false;
    private float health=100f;
    private int count = 0;
    private float timer;
    private float flySpeed = 100f;
    private float scaleSpeed = 7f;
    private float eulerZ;
    private float eulerSpeed = 360f * 4;
    public bool spawnLotsOfBlood = false;
    public Vector3 flyDirection;
    private bool collided = false;
    public bool destroyed=false;
    private Pathfinding pf;

    [SerializeField] private Transform gunEndPoint;
    void Start()
    {
        pf = GameObject.Find("TempTesting").GetComponent<Pathfinding>();
    }
      
    // Update is called once per frame
    void Update()
    {
        
        if (health <=0f)
        {
            EntitySpawner.gameDict.Remove(name);
            if (!collided)
            {
                spawnLotsOfBlood = true;

            }

            var pos = transform.position;


            flyDirection = (transform.position - gunEndPoint.position).normalized;

            pos.x += flyDirection.x * flySpeed * Time.deltaTime;
            pos.y += flyDirection.y * flySpeed * Time.deltaTime;

            transform.position = pos;


            transform.localScale += new Vector3(2f,2f,2f) * scaleSpeed * Time.deltaTime;

            //eulerZ += eulerSpeed * Time.deltaTime;
            //transform.localEulerAngles = new Vector3(0, 0, eulerZ);
            //Blood_Handler.Instance.SpawnBlood(5, transform.position,flyDirection*-1f);
            //BloodParticleSystemHandler.Instance.SpawnBlood(5, transform.position, flyDirection * -1f);





            timer += Time.deltaTime;
            if (timer > 1f)
            {
                destroyed = true;
                pf.enemyTransform.Remove(transform);
                Destroy(gameObject);
  
            }

        }
        else if (isHit)
        {
            count++;
            health = health - count*10;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{

    //        Debug.Log("hey there");
    //        Debug.Log(collision.gameObject.name);
    //        spawnLotsOfBlood = false;


    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    spawnLotsOfBlood = false;
    //    collided = true;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (health <= 0f)
        {
            spawnLotsOfBlood = false;
            collided = true;
            destroyed = true;
            pf.enemyTransform.Remove(transform);
            Destroy(gameObject);
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (health <= 0f)
        {
            spawnLotsOfBlood = false;
            collided = true;
            destroyed = true;
            pf.enemyTransform.Remove(transform);
            Destroy(gameObject);
        }
    }

}
