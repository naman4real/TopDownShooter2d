using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridTesting : MonoBehaviour
{
    // Start is called before the first frame update
    private Grid grid;
    void Start()
    {
        grid = new Grid(20, 10, 10f, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        if (Input.GetMouseButtonDown(0))
        {
            var value= grid.GetValue(mouseWorldPosition);
            grid.SetValue(mouseWorldPosition,value +5);

        }

    }
}
