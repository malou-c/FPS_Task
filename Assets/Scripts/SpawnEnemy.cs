using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour, IKillEnemyHandler
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private MeshCollider _plane;
    [SerializeField] private int _amountSpawn;
    private int _amountEnemy;
    private List<GameObject> _enemies = new List<GameObject>();
    
    void Start()
    {
        GlobalEventsManager.KillEnemyEvent.AddListener(OnKillEnemy);
        for (int i = 0; i < _amountSpawn; i++)
        {
            var enemy = Instantiate(_enemy, gameObject.transform);
            _enemies.Add(enemy);
            enemy.SetActive(false);
        }
        SpawnRange(_amountSpawn);
    }

    public IEnumerator SpawnCoroutine()
    {
        bool isSpawn = false;
        Transform planeTransform = _plane.transform;
        Vector3 planeExtents = _plane.bounds.extents;
        float xPlane = planeTransform.position.x;
        float zPlane = planeTransform.position.z;
        while (!isSpawn)
        {
            // Рандомная точка на plane
            float x = Random.Range(xPlane - Random.Range(0, planeExtents.x), xPlane + Random.Range(0, planeExtents.x));
            float z = Random.Range(zPlane - Random.Range(0, planeExtents.z), zPlane + Random.Range(0, planeExtents.z));
            float y = 20f;

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(x, y, z), Vector3.down, out hit, 40f))
            {
                if (hit.collider.tag == "Map")
                {
                    // Если есть выключенные противники берутся из пула, если нету создается новый и сохраняется в пул
                    // Примечание: в данной реализации приложения не используется, сделано для расширения и гибкой работы
                    // программы
                    var enemy = _enemies.FirstOrDefault(x => !x.activeSelf);
                    if (enemy != null)
                    {
                        enemy.GetComponent<Transform>().position = hit.point;
                        enemy.SetActive(true);
                    }
                    else
                    {
                        var newEnemy = Instantiate(_enemy, hit.point, Quaternion.identity);
                        _enemies.Add(newEnemy);
                    }
                    isSpawn = true;
                    _amountEnemy++;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    public void SpawnRange(int amount)
    {
        for (int i = 0; i < amount; i++)
            StartCoroutine(SpawnCoroutine());
    }

    public void OnKillEnemy()
    {
        _amountEnemy--;
        if (_amountEnemy <= 0)
            SpawnRange(_amountSpawn);
    }

    public void Restart()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.activeSelf)
            {
                enemy.SetActive(false);
                _amountEnemy--;
            }
        }
        SpawnRange(_amountSpawn);
    }
}
