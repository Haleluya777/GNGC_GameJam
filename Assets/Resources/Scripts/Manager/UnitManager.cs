using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour, IDataInitializable
{
    public Unit playerUnit;

    void Update()
    {
        //Debug.Log(playerUnit.transform.position);
    }

    public void DataInitialize()
    {

    }
}
