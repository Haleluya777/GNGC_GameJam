using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour, IDataInitializable
{
    public Dictionary<string, ObjectPooler> poolDic = new Dictionary<string, ObjectPooler>();

    public void DataInitialize()
    {
        Debug.Log("초기화됨");
        poolDic.Add("Enemy", transform.GetChild(0).GetComponent<ObjectPooler>());
        poolDic.Add("Bullet", transform.GetChild(1).GetComponent<ObjectPooler>());
    }
}
