using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMover : MonoBehaviour
{
    [Range(0, 100)]
    public float Speed = 1f;

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        transform.position += Vector3.right * horizontal * Speed * Time.deltaTime;
    }
}
