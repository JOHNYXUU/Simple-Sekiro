using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDestorySelf : MonoBehaviour {

    public float mCountDown = 5.0f;

    // Update is called once per frame
    void Update()
    {
        mCountDown -= Time.deltaTime;

        if (mCountDown <= 0)
            Object.Destroy(gameObject);
    }
}
