using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBtn : MonoBehaviour
{
    public void Skip()
    {
        LoadSceneManager.LoadScene(SceneName.EnchantScene);
    }
}
