using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
  public GameObject r_Enemy;
  const float c_SpawnPeriod = 5;
  const float c_SpawnRadiusPadding = 2;
  float m_NextSpawnTime;
  float m_SpawnRadius;

  void Start() {
    m_NextSpawnTime = Time.time + c_SpawnPeriod;
    {// Spawn radius
      var hh = Camera.main.orthographicSize;
      var hw = hh * Camera.main.aspect;
      m_SpawnRadius = Mathf.Sqrt(hw*hw + hh*hh) + c_SpawnRadiusPadding;      
    }
  }

  void Update() {
    var t = Time.time;
    if (t >= m_NextSpawnTime) {
      for (int i = 0; i < Random.Range(3, 0); ++i) SpawnEnemy();
      m_NextSpawnTime = t + c_SpawnPeriod;
    }
  }

  void SpawnEnemy() {
    var phi = Random.Range(0, 2*Mathf.PI);
    var pos = new Vector3(
      Mathf.Cos(phi) * m_SpawnRadius,
      Mathf.Sin(phi) * m_SpawnRadius,
      0
    );
    Instantiate(r_Enemy, pos, Quaternion.identity);
  }
}
