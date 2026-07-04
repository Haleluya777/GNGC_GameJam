using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour, IDataInitializable
{
    public Unit playerUnit;
    public List<GameObject> enemyList;
    public List<Transform> summonPos;

    void Update()
    {
        //Debug.Log(playerUnit.transform.position);
    }

    public void DataInitialize()
    {

    }

    public void SummonEnemy(int proccess)
    {
        Debug.Log("할렐루야");
        switch (proccess)
        {
            case 0:
                var obj = Instantiate(enemyList[0]);
                obj.transform.position = summonPos[0].position;
                LocalGameManager.instance.gameProccessManager.monsterCount = 1;

                break;
        }
    }
}
