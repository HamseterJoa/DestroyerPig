using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{

    // 공 팅겨주기
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Ball" || collision.transform.tag == "MiniBall") {
            
            Vector2 reflect = collision.transform.position - transform.position;

            float result = 0.0f;

            if (reflect.x > 0) result = 1.0f;
            else if (reflect.x < 0) result = -1.0f;

            collision.rigidbody.AddForce(new Vector2(100.0f * result, 1.0f));
        }
    }
}
