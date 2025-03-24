using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatcharResult : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) gameObject.SetActive(false);
    }
}
