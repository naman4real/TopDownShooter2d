using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    Rigidbody2D shadow,player;
    Vector2 movement;
    public GameObject front, back, right, left,sh;
    public Animator animatorFront;
    public Animator animatorBack;
    public Animator animatorRight;
    public Animator animatorLeft;
    private Vector2 pos;
    void Start()
    {
        shadow = sh.GetComponent<Rigidbody2D>();
        player = GetComponent<Rigidbody2D>();

        right.SetActive(false);
        left.SetActive(false);
        front.SetActive(true);
        back.SetActive(false);
        //for (int i = 0; i < 4; i++)
        //{
        //    front.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
        //}
    }

    // Update is called once per frame
    void Update()
    {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animatorFront.SetFloat("Horizontal", movement.x);
        animatorFront.SetFloat("Vertical", movement.y); 
        animatorFront.SetFloat("Speed", movement.sqrMagnitude);

        animatorBack.SetFloat("Horizontal", movement.x);
        animatorBack.SetFloat("Vertical", movement.y);
        animatorBack.SetFloat("Speed", movement.sqrMagnitude);

        animatorRight.SetFloat("Horizontal", movement.x);
        animatorRight.SetFloat("Vertical", movement.y);
        animatorRight.SetFloat("Speed", movement.sqrMagnitude);

        animatorLeft.SetFloat("Horizontal", movement.x);
        animatorLeft.SetFloat("Vertical", movement.y);
        animatorLeft.SetFloat("Speed", movement.sqrMagnitude);

        Turn();
     




    }
    void Turn()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        Vector3 aimDirection = (mouseWorldPosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;


        if (angle >= -45f && angle < 45f)
        {

            right.SetActive(true);
            left.SetActive(false);
            front.SetActive(false);
            back.SetActive(false);



            //for (int i = 0; i < 4; i++)
            //{
            //    front.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            //    right.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
            //    left.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            //    back.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;

            //}
        }
        else if (angle >= 45f && angle < 135f)
        {
            right.SetActive(false);
            left.SetActive(false);
            front.SetActive(false);
            back.SetActive(true);


            //for (int i = 0; i < 4; i++)
            //{
            //    front.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            //    right.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            //    left.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            //    back.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;

            //}
        }
        else if (angle >= 135f || angle < -135f)
        {


            right.SetActive(false);
            left.SetActive(true);
            front.SetActive(false);
            back.SetActive(false);


            //for (int i = 0; i < 4; i++)
            //{
            //    front.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            //    right.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            //    left.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
            //    back.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;

            //}
        }
        else if (angle >= -135f || angle < -45f)
        {
            right.SetActive(false);
            left.SetActive(false);
            front.SetActive(true);
            back.SetActive(false);


            //for (int i = 0; i < 4; i++)
            //{
            //    front.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
            //    right.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            //    left.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            //    back.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;

            //}
        }
    }
    private void FixedUpdate()
    {

        player.MovePosition(player.position + movement * moveSpeed * Time.fixedDeltaTime);
        for(int i = 0; i < 4; i++)
        {
            transform.GetChild(i).transform.position = this.transform.position;
        }

    }
}
