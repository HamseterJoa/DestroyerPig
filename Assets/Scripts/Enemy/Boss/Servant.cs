using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : MonoBehaviour
{
    // 토
    private Vector2 _PukePos;
    [SerializeField] private GameObject _Puke;

    private void EnablePuke()
    {
        _Puke.gameObject.SetActive(true);
    }

    private void DisablePuke()
    {
        _Puke.gameObject.SetActive(false);
    }

}
