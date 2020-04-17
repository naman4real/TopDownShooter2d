using System;
using System.Collections;
using UnityEngine;
using CodeMonkey.Utils;

public class playerAimWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    public event EventHandler<OnShootEventArgs> OnShoot;
    public event EventHandler<OnHitEventArgs> OnHit;

    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
        public Vector3 shellPosition;
    }

    public class OnHitEventArgs : EventArgs
    {

        public Vector3 shootPosition;
    }
    public Transform aimTransform;
    public Transform gunEndPointTransform;
    public Transform shellTransform;
    public Animator animatorAim;
    private SoundManager s;
 
    void Start()
    {
        s = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Aiming();
        Shooting();
      
    }
    private void Aiming()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        Vector3 aimDirection = (mouseWorldPosition -transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = new Vector3(1f,1f,1f);
        if (angle>90 || angle < -90)
        {
            aimLocalScale.y = -1f;
        }
        else
        {
            aimLocalScale.y = 1f;
        }
        aimTransform.localScale = aimLocalScale;
    }
    private void Shooting()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            animatorAim.SetTrigger("Shoot");

            s.PlayOneShot("PistolShot");
   
            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                gunEndPointPosition = gunEndPointTransform.position,
                shootPosition = mousePosition,
                shellPosition = shellTransform.position,
            }) ;


            OnHit?.Invoke(this, new OnHitEventArgs
            {
                shootPosition = mousePosition,

            });

        }

    }



 

}
