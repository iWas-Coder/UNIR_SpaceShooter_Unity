using UnityEngine;

public class Enemy : MonoBehaviour
{
  public GameObject r_Bullet;
  public Transform r_FirePoint;
  public AudioClip r_ShootSound;
  GameObject r_Player;
  AudioSource r_VariableAudioSource;
  const float c_ShootInitialGracePeriod = 3.3f;
  const float c_ShootSpeed = 20;
  const float c_MoveSpeed = 2;
  float m_NextShootTime;

  void Start() {
    r_Player = GameObject.FindWithTag("Player");
    if (r_Player == null) Destroy(gameObject);
    {// AudioSource
      var obj = GameObject.FindWithTag("VariableAudioSource");
      if (obj == null) return;
      r_VariableAudioSource = obj.GetComponent<AudioSource>();
    }
    m_NextShootTime = Time.time + c_ShootInitialGracePeriod;
  }

  void Update() {
    Aim();
    Move();
    {// Shooting
      var t = Time.time;
      if (t >= m_NextShootTime) {
        Shoot();
        m_NextShootTime = t + Random.Range(1, 3);
      }
    }
  }

  void OnCollisionEnter2D(Collision2D dst) {
    if (dst.gameObject.CompareTag("Player")) {
      {// Player damage
        var scr = dst.gameObject.GetComponent<Player>();
        if (scr != null) scr.TakeDMG();
      }
      {// Enemy explosion
        var scr = GetComponent<Explosion>();
        if (scr != null) scr.Explode();
      }
    }
  }

  void Aim() {
    var dir = r_Player.transform.position - transform.position;
    var phi = Mathf.Atan2(dir.y, dir.x) - Mathf.PI/2;
    var rot = Quaternion.Euler(0, 0, phi * Mathf.Rad2Deg);
    transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5 * Time.deltaTime);
  }

  void Move() {
    var rb = GetComponent<Rigidbody2D>();
    if (rb != null) {
      rb.linearVelocity = transform.up * c_MoveSpeed;
    }
  }

  void Shoot() {
    if (!r_Player.activeSelf) return;
    Utils.PlaySoundWithRandomPitch(r_VariableAudioSource, r_ShootSound);
    var bullet = Instantiate(r_Bullet, r_FirePoint.position, r_FirePoint.rotation);
    var rb = bullet.GetComponent<Rigidbody2D>();
    if (rb != null) {
      rb.linearVelocity = r_FirePoint.up * c_ShootSpeed;
    }
    Destroy(bullet, 3);
  }
}
