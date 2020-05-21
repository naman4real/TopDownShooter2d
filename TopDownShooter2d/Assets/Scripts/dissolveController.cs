using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class dissolveController : MonoBehaviour
{

    public static float dissolveAmount=-0.1f;
    public bool isDisssolving=false;
    public float dissolveSpeed;
    private Material mat;

    [ColorUsageAttribute(true, true)]
    public Color outColor;
    [ColorUsageAttribute(true, true)]
    public Color inColor;


    private void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    isDisssolving = true;
        //}
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    isDisssolving = false;
        //}
        //if (isDisssolving)
        //{
        //    DissolveOut(dissolveSpeed, outColor);
        //}
        if (!isDisssolving)
        {
            DissolveIn(dissolveSpeed, inColor);
        }
        mat.SetFloat("_DissolveAmmount",dissolveAmount);
    }



    public void DissolveOut(float speed, Color color)
    {
        mat.SetColor("_DissolveColor", color);

        if (dissolveAmount > -0.1)
        {
            dissolveAmount -= Time.deltaTime * dissolveSpeed;
        }
    }
    public void DissolveIn(float speed, Color color)
    {
        mat.SetColor("_DissolveColor", color);

        if (dissolveAmount<1)
        {
            dissolveAmount += Time.deltaTime * dissolveSpeed;
        }
    }
}
