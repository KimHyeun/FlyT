using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public Transform[] spawnPoint;

    float line_1_Y_Value;
    float line_2_Y_Value;
    float line_3_Y_Value;

    public float[] LineYValues;

    public GameObject foodObj;
    public GameObject enemyObj;

    private void Start()
    {
        line_1_Y_Value = 3f;
        line_2_Y_Value = 0f;
        line_3_Y_Value = -3f;

        LineYValues = new float[] { line_1_Y_Value, line_2_Y_Value, line_3_Y_Value };

        StartCoroutine(SpawnRandomObject());
    }

    IEnumerator SpawnRandomObject()
    {
        while (true) // 무한 반복
        {
            // 랜덤한 위치 선택
            int randomIndex = Random.Range(0, spawnPoint.Length);
            Transform spawnPosition = spawnPoint[randomIndex];

            int ran = Random.Range(0, 3);

            // 랜덤으로 음식 또는 적을 선택하여 스폰
            if (ran == 0)
            {
                Instantiate(foodObj, spawnPosition.position, Quaternion.identity);
            }

            else if (ran == 1) 
            {
                Instantiate(enemyObj, spawnPosition.position, Quaternion.identity);
            }

            else if (ran == 2)
            {
                int foodIndex = Random.Range(0, spawnPoint.Length);
                int enemyIndex = Random.Range(0, spawnPoint.Length);

                while (foodIndex == enemyIndex)
                {
                    enemyIndex = Random.Range(0, spawnPoint.Length);
                }

                Instantiate(foodObj, spawnPoint[foodIndex].position, Quaternion.identity);
                Instantiate(enemyObj, spawnPoint[enemyIndex].position, Quaternion.identity);
            }


            yield return new WaitForSeconds(Random.Range(0.4f, 0.9f));
        }
    }
}
