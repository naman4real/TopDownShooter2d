using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject front, back, right, left;
    public Animator enemyAnimator;
    [SerializeField] private GameObject player;
    void Start()
    {

        right.SetActive(false);
        left.SetActive(false);
        front.SetActive(true);
        back.SetActive(false);


    }

    void Update()
    {

        Turn();

    }
    void Turn()
    {
        Vector3 aimDirection = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        if (angle >= -45f && angle < 0f)
        {
            enemyAnimator.SetFloat("Horizontal", 1f);
            enemyAnimator.SetFloat("Vertical", -1f);
            enemyAnimator.SetFloat("Speed", new Vector2(1f,-1f).sqrMagnitude);

            right.SetActive(true);
            left.SetActive(false);
            front.SetActive(false);
            back.SetActive(false);

            enemyAnimator.SetBool("isFront", false);
            enemyAnimator.SetBool("isBack", false);
            enemyAnimator.SetBool("isLeft", false);
            enemyAnimator.SetBool("isRight", true);

        }
        else if (angle >= 0f && angle < 45f)
        {
            enemyAnimator.SetFloat("Horizontal", 1f);
            enemyAnimator.SetFloat("Vertical", 1f);
            enemyAnimator.SetFloat("Speed", new Vector2(1f, 1f).sqrMagnitude);

            if (angle == 0f) enemyAnimator.SetFloat("Vertical", 0);

            right.SetActive(true);
            left.SetActive(false);
            front.SetActive(false);
            back.SetActive(false);

            enemyAnimator.SetBool("isFront", false);
            enemyAnimator.SetBool("isBack", false);
            enemyAnimator.SetBool("isLeft", false);
            enemyAnimator.SetBool("isRight", true);


        }
        else if (angle >= 45f && angle < 90f)
        {

            enemyAnimator.SetFloat("Horizontal", 1f);
            enemyAnimator.SetFloat("Vertical", 1f);
            enemyAnimator.SetFloat("Speed", new Vector2(1f, 1f).sqrMagnitude);



            right.SetActive(false);
            left.SetActive(false);
            front.SetActive(false);
            back.SetActive(true);

            enemyAnimator.SetBool("isFront", false);
            enemyAnimator.SetBool("isBack", true);
            enemyAnimator.SetBool("isLeft", false);
            enemyAnimator.SetBool("isRight", false);


        }

        else if (angle >= 90f && angle < 135f)
        {

            enemyAnimator.SetFloat("Horizontal", -1f);
            enemyAnimator.SetFloat("Vertical", 1f);
            enemyAnimator.SetFloat("Speed", new Vector2(-1f, 1f).sqrMagnitude);

            if (angle == 90f) enemyAnimator.SetFloat("Horizontal", 0);

            right.SetActive(false);
            left.SetActive(false);
            front.SetActive(false);
            back.SetActive(true);

            enemyAnimator.SetBool("isFront", false);
            enemyAnimator.SetBool("isBack", true);
            enemyAnimator.SetBool("isLeft", false);
            enemyAnimator.SetBool("isRight", false);


        }
        else if (angle >= 135f && angle <= 180)
        {
            enemyAnimator.SetFloat("Horizontal", -1f);
            enemyAnimator.SetFloat("Vertical", 1f);
            enemyAnimator.SetFloat("Speed", new Vector2(-1f, 1f).sqrMagnitude);

            if (angle == 180f) enemyAnimator.SetFloat("Vertical", 0);

            right.SetActive(false);
            left.SetActive(true);
            front.SetActive(false);
            back.SetActive(false);

            enemyAnimator.SetBool("isFront", false);
            enemyAnimator.SetBool("isBack", false);
            enemyAnimator.SetBool("isLeft", true);
            enemyAnimator.SetBool("isRight", false);


        }
        else if (angle > -180f && angle <= -135f)
        {

            enemyAnimator.SetFloat("Horizontal", -1f);
            enemyAnimator.SetFloat("Vertical", -1f);
            enemyAnimator.SetFloat("Speed", new Vector2(-1f, -1f).sqrMagnitude);


            right.SetActive(false);
            left.SetActive(false);
            front.SetActive(true);
            back.SetActive(false);

            enemyAnimator.SetBool("isFront", true);
            enemyAnimator.SetBool("isBack", false);
            enemyAnimator.SetBool("isLeft", false);
            enemyAnimator.SetBool("isRight", false);

        }
        else if (angle > -135f && angle <= -90f)
        {

            enemyAnimator.SetFloat("Horizontal", -1f);
            enemyAnimator.SetFloat("Vertical", -1f);
            enemyAnimator.SetFloat("Speed", new Vector2(-1f, -1f).sqrMagnitude);

            if (angle == -90f) enemyAnimator.SetFloat("Horizontal", 0f);


            right.SetActive(false);
            left.SetActive(false);
            front.SetActive(true);
            back.SetActive(false);

            enemyAnimator.SetBool("isFront", true);
            enemyAnimator.SetBool("isBack", false);
            enemyAnimator.SetBool("isLeft", false);
            enemyAnimator.SetBool("isRight", false);

        }

        else if (angle > -90f && angle <= -45f)
        {

            enemyAnimator.SetFloat("Horizontal", 1f);
            enemyAnimator.SetFloat("Vertical", -1f);
            enemyAnimator.SetFloat("Speed", new Vector2(1f, -1f).sqrMagnitude);

            if (angle == -90f) enemyAnimator.SetFloat("Horizontal", 0f);


            right.SetActive(false);
            left.SetActive(false);
            front.SetActive(true);
            back.SetActive(false);

            enemyAnimator.SetBool("isFront", true);
            enemyAnimator.SetBool("isBack", false);
            enemyAnimator.SetBool("isLeft", false);
            enemyAnimator.SetBool("isRight", false);

        }
    }
}
