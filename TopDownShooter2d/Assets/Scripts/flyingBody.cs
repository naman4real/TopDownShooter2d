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
    private float flySpeed = 300f;
    private float scaleSpeed = 7f;
    private float eulerZ;
    private float eulerSpeed = 360f * 4;
    public bool spawnLotsOfBlood = false;
    public Vector3 flyDirection;
    private Vector3 pos;
    private bool collided = false;

    [SerializeField] private Transform gunEndPoint;
    void Start()
    {
        pos = transform.position;
    }
      
    // Update is called once per frame
    void Update()
    {
        if (health <=0f)
        {
            if (!collided)
            {
                spawnLotsOfBlood = true;

            }
            flyDirection = (transform.position - gunEndPoint.position).normalized;
            pos.x += flyDirection.x* flySpeed * Time.deltaTime;
            pos.y += flyDirection.y * flySpeed * Time.deltaTime;
            transform.position = pos;

            transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;

            eulerZ += eulerSpeed * Time.deltaTime;
            transform.localEulerAngles = new Vector3(0, 0, eulerZ);
            //Blood_Handler.Instance.SpawnBlood(5, transform.position,flyDirection*-1f);
            //BloodParticleSystemHandler.Instance.SpawnBlood(5, transform.position, flyDirection * -1f);





            timer += Time.deltaTime;
            if (timer > 1f)
            {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spawnLotsOfBlood = false;
        collided = true;
    }

}
