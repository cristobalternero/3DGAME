using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasReferences : MonoBehaviour
{
    public static CanvasReferences Instance;
    public TextMeshProUGUI ammunitionDisplay;

    void Awake()
    {
        Instance = this;
    }
}
