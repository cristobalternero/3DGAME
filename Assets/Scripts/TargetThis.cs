using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetThis : MonoBehaviour
{
    public Transform m_gun;

    // Update is called once per frame
    void Update()
    {
        m_gun.LookAt(transform);
    }
}
