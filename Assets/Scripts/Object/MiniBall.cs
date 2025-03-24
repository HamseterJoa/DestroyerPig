using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "CornArea") gameObject.SetActive(false);
    }
}
