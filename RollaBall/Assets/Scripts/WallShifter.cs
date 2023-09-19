using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShifter : MonoBehaviour
{
    private static Vector3 X_SPEED = new Vector3(10, 0, 0);

    public bool positive;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= 5)
        {
            positive = false;
        }
        if (transform.position.x <= -5)
        {
            positive = true;
        }
        if (positive)
        {
            transform.position += X_SPEED * Time.deltaTime;
        }
        if (!positive)
        {
            transform.position -= X_SPEED * Time.deltaTime;
        }
    }
}
