using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inflator : MonoBehaviour
{
    public bool growing = false;
    private bool shrinking = false;

    // Update is called once per frame
    void Update()
    {
        if (growing)
        {
            transform.localScale += new Vector3(0, 5, 0) * Time.deltaTime;
            if (transform.localScale.y >= 3)
            {
                growing = false;
                shrinking = true;
            }
        }
        if (shrinking)
        {
            transform.localScale -= new Vector3(0, 0.3f, 0) * Time.deltaTime;
            if (transform.localScale.y <= 0.5)
            {
                shrinking = false;
                transform.localScale = new Vector3(3, 0.5f, 3);
                this.gameObject.GetComponent<Collider>().isTrigger = true;
            }
        }
    }
}
