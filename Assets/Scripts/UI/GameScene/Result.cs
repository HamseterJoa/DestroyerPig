using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private Text _Record;
    [SerializeField] private Text _KillPig;
    [SerializeField] private Text _UsedItem;
    [SerializeField] private Text _KillBoss;
    [SerializeField] private Text _Bonus;

    [SerializeField] private WaveManager _Wave;

    private void OnEnable()
    {
        ResultUpdate();
    }

    private void ResultUpdate()
    {
        _Record.text = _Wave._StageState.ToString();
        _KillPig.text = GameManager.getCharacterManager.killPiggetset.ToString();
        _UsedItem.text = GameManager.getCharacterManager.usedItemgetset.ToString();
        _KillBoss.text = GameManager.getCharacterManager.KillBossgetset.ToString();
        _Bonus.text = GameManager.getCharacterManager.bonusgetset.ToString();
    }
}
