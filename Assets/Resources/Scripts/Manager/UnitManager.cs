using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour, IDataInitializable
{
    public Unit playerUnit;

    public GameObject obj;

    void Update()
    {
        //Debug.Log(playerUnit.transform.position);
    }

    public void DataInitialize()
    {

    }

    public void SummonEnemy(GameObject prefab)
    {
        obj = null;
        obj = Instantiate(prefab);
    }

    public void SetEnemyPosition(Transform pos)
    {
        obj.transform.position = pos.position;
    }
}
