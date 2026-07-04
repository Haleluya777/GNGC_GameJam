using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            var proccessManager = LocalGameManager.instance.gameProccessManager;
            proccessManager.proccess++;
            proccessManager.GameProccess(proccessManager.proccess);
            Destroy(this.gameObject);
        }
    }
}
