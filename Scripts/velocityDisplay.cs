using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class velocityDisplay : MonoBehaviour
{
    public Rigidbody rb;
    public Text text;

    public MovementController mc;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $" {mc.isOnFloor}, {mc.IsAccelerating()} X = {Math.Round(rb.velocity.x,2)}, Y = {Math.Round(rb.velocity.y,2)}, Z = {Math.Round(rb.velocity.z,2)}";
    }
}
