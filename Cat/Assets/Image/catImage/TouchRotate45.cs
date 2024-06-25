using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate45 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!GameControl.isGlowingStatueActive)
        {
            transform.Rotate(0f, 0f, 45f);
        }
    }
}
