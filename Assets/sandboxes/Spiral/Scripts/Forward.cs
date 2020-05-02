using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forward : MonoBehaviour
{
    [SerializeField]
    private float speed = 0;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
