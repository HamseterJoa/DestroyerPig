using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Player player { get; set; }
    public Ball ball { get; set; }

    // 게임중 얻은 돈
    private static uint coinCount;

    // 게임중 얻은 보너스
    private static uint bonus;

    // 게임중 죽인 돼지수
    private static uint killPig;

    // 게임중 사용한 먹은 아이템 수
    private static uint usedItem;

    // 게임중 죽인 보스 수
    private static int KillBoss;

    public uint coinCountgetset
    { get { return coinCount; } set { coinCount = value; } }

    public uint bonusgetset
    { get { return bonus; } set { bonus = value; } }

    public uint killPiggetset
    { get { return killPig; } set { killPig = value; } }

    public uint usedItemgetset
    { get { return usedItem; } set { usedItem = value; } }

    public int KillBossgetset
    { get { return KillBoss; } set { KillBoss = value; } }

    private void Awake()
    {
        coinCount = 0;
        bonus = 0;
        killPig = 0;
        usedItem = 0;
        KillBoss = 0;
    }

}
