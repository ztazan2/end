using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Interval
    {
        public GameObject enemyPrefab;   // ������ ���� ������
        public float spawnInterval;      // ���� ���� ���� (�� ����)
    }

    public List<Interval> interval = new List<Interval>(); // ������ ���� ����Ʈ
    private Vector3 spawnPosition; // ������ ���� ���� ��ġ ����

    private void Start()
    {
        // ������ ���� ��ġ ����
        spawnPosition = new Vector3(6f, -1.4f, transform.position.z);

        // �� ���ֿ� ���� ������ �������� �����ϵ��� �ڷ�ƾ ����
        foreach (Interval timedEnemy in interval)
        {
            StartCoroutine(SpawnTimedEnemy(timedEnemy));
        }
    }

    private IEnumerator SpawnTimedEnemy(Interval timedEnemy)
    {
        while (true) // ���������� ���� �����ϱ� ���� ���� �ݺ�
        {
            // ������ ���ݸ�ŭ ��� �� ���� ����
            yield return new WaitForSeconds(timedEnemy.spawnInterval);

            if (timedEnemy.enemyPrefab != null)
            {
                Instantiate(timedEnemy.enemyPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Enemy prefab is missing.");
            }
        }
    }
}
