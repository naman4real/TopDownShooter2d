using System;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class effects : MonoBehaviour
{
    [SerializeField] playerAimWeapon p;
    public List<Sprite> bloodParticles;

    private void Start()
    {
        p.OnHit += Hit;
    }

    private void Hit(object sender, playerAimWeapon.OnHitEventArgs e)
    {
        //CreateBlood(e.shootPosition);
    }

    public void CreateBlood(Vector3 shootPosition)
    {
        GameObject blood = new GameObject();
        blood.transform.position = shootPosition;
        blood.transform.localScale = new Vector3(10f, 10f, 1f);
        SpriteRenderer s = blood.AddComponent<SpriteRenderer>();
        s.sprite = bloodParticles[UnityEngine.Random.Range(0, bloodParticles.Count)];
        s.sortingOrder = -5;

    }
}
