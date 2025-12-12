using UnityEngine;

public class Bullet : MonoBehaviour
{
  const float c_DestructionGracePeriod = .05f;
  float m_SpawnTime;

  void Start() {
    m_SpawnTime = Time.time;
  }
  
  void OnTriggerEnter2D(Collider2D dst) {
    if (Time.time <= m_SpawnTime + c_DestructionGracePeriod) return;
    if (dst.CompareTag("Enemy")) {
      GameContext.s_Instance.AddScore(10);
      var scr = dst.GetComponent<Explosion>();
      if (scr != null) scr.Explode();
      Destroy(gameObject);
    }
    else if (dst.CompareTag("Player")) {
      var scr = dst.GetComponent<Player>();
      if (scr != null) scr.TakeDMG();
      Destroy(gameObject);
    }
  }
}
