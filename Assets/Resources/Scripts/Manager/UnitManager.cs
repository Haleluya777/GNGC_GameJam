using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitManager : MonoBehaviour, IDataInitializable
{
    public Unit playerUnit;
    public GameObject playerPrefab;
    public GameObject boss;
    public List<GameObject> enemyList;
    public List<Transform> summonPos;
    public List<Unit> activeEnemies = new List<Unit>(); // 활성화된 적 리스트

    void Update()
    {
        //Debug.Log(playerUnit.transform.position);
    }

    public void DataInitialize()
    {

    }

    // 모든 활성화된 적을 파괴하는 메소드
    public void DestroyAllEnemies()
    {
        // 리스트를 뒤에서부터 순회하며 제거 (리스트 변경에 안전)
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] != null)
            {
                Destroy(activeEnemies[i].gameObject);
            }
        }
        activeEnemies.Clear(); // 리스트 비우기
    }

    public void SummonEnemy(int proccess)
    {
        Debug.Log("할렐루야");
        switch (proccess)
        {
            case 0: //1단계.
                {
                    var obj = Instantiate(enemyList[0]);
                    obj.transform.position = summonPos[0].position;
                    activeEnemies.Add(obj.GetComponent<Unit>()); // 리스트에 추가
                    LocalGameManager.instance.gameProccessManager.monsterCount = 1;
                    break;
                }


            case 1: //2단계.
                {
                    var goal = Instantiate(enemyList[2]);
                    goal.transform.position = summonPos[2].position;
                    activeEnemies.Add(goal.GetComponent<Unit>()); // 리스트에 추가
                    break;
                }

            case 2: //3단계.
                {
                    var obj = Instantiate(enemyList[0]);
                    var obj2 = Instantiate(enemyList[0]);
                    obj.transform.position = summonPos[0].position;
                    obj2.transform.position = summonPos[2].position;
                    activeEnemies.Add(obj.GetComponent<Unit>()); // 리스트에 추가
                    activeEnemies.Add(obj2.GetComponent<Unit>()); // 리스트에 추가
                    LocalGameManager.instance.gameProccessManager.monsterCount = 2;
                    break;
                }

            case 3: //4단계
                {
                    Sequence sequence = DOTween.Sequence();

                    sequence.AppendCallback(() => LocalGameManager.instance.playerUiManager.FadeInOut());
                    var proccessManager = LocalGameManager.instance.gameProccessManager;
                    proccessManager.proccess++;
                    LocalGameManager.instance.gameProccessManager.GameProccess(proccessManager.proccess);
                    break;
                }
            case 4: //5단계
                {
                    var obj = Instantiate(enemyList[0]);
                    var obj1 = Instantiate(enemyList[0]);
                    var obj2 = Instantiate(enemyList[0]);
                    var obj3 = Instantiate(enemyList[0]);

                    obj.transform.position = summonPos[0].position;
                    obj1.transform.position = summonPos[2].position;
                    obj2.transform.position = summonPos[3].position;
                    obj3.transform.position = summonPos[4].position;

                    activeEnemies.Add(obj.GetComponent<Unit>()); // 리스트에 추가
                    activeEnemies.Add(obj1.GetComponent<Unit>()); // 리스트에 추가
                    activeEnemies.Add(obj2.GetComponent<Unit>()); // 리스트에 추가
                    activeEnemies.Add(obj3.GetComponent<Unit>()); // 리스트에 추가

                    LocalGameManager.instance.gameProccessManager.monsterCount = 4;
                    break;
                }

            case 5: //보스전
                {
                    var obj = Instantiate(boss);
                    obj.transform.position = summonPos[0].position;
                    activeEnemies.Add(obj.GetComponent<Unit>()); // 리스트에 추가
                    break;
                }
        }
    }
}