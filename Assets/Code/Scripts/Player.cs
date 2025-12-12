using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{ 
  public GameObject r_Crosshair;
  public Transform r_FirePoint;
  public GameObject r_Bullet;
  public GameObject r_GameOverMSG;
  public TextMeshProUGUI r_LivesCount;
  public TextMeshProUGUI r_AmmoCount;
  public GameObject r_NotificationMSG;
  public TextMeshProUGUI r_NotificationText;
  public AudioSource r_VariableAudioSource;
  public AudioSource r_ConstantAudioSource;
  public AudioClip r_ShootSound;
  public AudioClip r_RechargeSound;
  public AudioClip r_PowerupSound;
  public AudioClip r_GameOverSound;
  const float c_ShootSpeed = 20;
  const float c_MoveSpeed = 5;
  const float c_InvulnerableDuration = 2;
  const float c_InvulnerableFlashInterval = .1f;
  const float c_RechargeDelay = 3;
  const float c_GameOverDelay = 3;
  const int c_AmmoCapacityInitial = 5;
  int m_Health = 3;
  bool m_Invulnerable = false;
  bool m_Recharging = false;
  int m_AmmoCapacity = c_AmmoCapacityInitial;
  int m_AmmoCount = c_AmmoCapacityInitial;
  Powerup[] m_Powerups_ExtraLife = {
    new Powerup(100),
    new Powerup(300)
  };
  Powerup[] m_Powerups_ExtraAmmo = {
    new Powerup(200),
    new Powerup(400)
  };
  
  void Update() {
    Move();
    Shoot();
    ClaimPowerups();
    UpdateUI();
  }

  void UpdateUI() {
    if (m_Health < 0) m_Health = 0;
    r_LivesCount.text = $"Lives: {m_Health}";
    if (m_AmmoCount == 0) r_AmmoCount.text = "Ammo: 0...";
    else r_AmmoCount.text = $"Ammo: {m_AmmoCount}";
  }

  public void TakeDMG() {
    if (m_Invulnerable) return;
    --m_Health;
    if (m_Health > 0) StartCoroutine(InvulnerableFlash());
    else Die();
  }

  IEnumerator InvulnerableFlash() {
    m_Invulnerable = true;
    var col = GetComponent<Collider2D>();
    var spr = GetComponent<SpriteRenderer>();
    if (col != null) col.enabled = false;
    var end_t = Time.time + c_InvulnerableDuration;
    while (Time.time < end_t) {
      if (spr != null) spr.enabled = !spr.enabled;
      yield return new WaitForSeconds(c_InvulnerableFlashInterval);
    }
    if (spr != null) spr.enabled = true;
    if (col != null) col.enabled = true;
    m_Invulnerable = false;
  }

  void Die() {
    var scr = GetComponent<Explosion>();
    if (scr != null) scr.Explode();
    r_GameOverMSG.SetActive(true);
    Invoke("GameOver", c_GameOverDelay);
    r_ConstantAudioSource.PlayOneShot(r_GameOverSound);
  }

  void GameOver() {
    SceneManager.LoadScene("MainMenu");
  }

  void Move() {
    {// Position
      var dir = new Vector3(
        Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical"),
        0
      );
      transform.position += dir.normalized * c_MoveSpeed * Time.deltaTime;
    }
    {// Orientation
      var v = r_Crosshair.transform.position - transform.position;
      var phi = Mathf.Atan2(v.y, v.x) - Mathf.PI/2;
      transform.rotation = Quaternion.Euler(0, 0, phi * Mathf.Rad2Deg);
    }
  }

  void Shoot() {
    if (m_Recharging || !Input.GetMouseButtonDown(0)) return;
    Utils.PlaySoundWithRandomPitch(r_VariableAudioSource, r_ShootSound);
    var bullet = Instantiate(r_Bullet, r_FirePoint.position, r_FirePoint.rotation);
    var rb = bullet.GetComponent<Rigidbody2D>();
    if (rb != null) rb.linearVelocity = r_FirePoint.up * c_ShootSpeed;
    --m_AmmoCount;
    if (m_AmmoCount <= 0) StartRecharge();
    Destroy(bullet, 3);
  }

  void StartRecharge() {
    if (m_Recharging) return;
    m_Recharging = true;
    Invoke("EndRecharge", c_RechargeDelay);
    r_ConstantAudioSource.PlayOneShot(r_RechargeSound);
  }

  void EndRecharge() {
    m_AmmoCount = m_AmmoCapacity;
    m_Recharging = false;
  }

  void ClaimPowerups() {
    foreach (var i in m_Powerups_ExtraLife) {
      if (i.Claim()) {
        ++m_Health;
        r_ConstantAudioSource.PlayOneShot(r_PowerupSound);
        StartCoroutine(PrintNotification("Got extra life!"));
      }
    }
    foreach (var i in m_Powerups_ExtraAmmo) {
      if (i.Claim()) {
        ++m_AmmoCapacity;
        r_ConstantAudioSource.PlayOneShot(r_PowerupSound);
        StartCoroutine(PrintNotification("Got extra ammo capacity!"));
      }
    }
  }

  IEnumerator PrintNotification(string msg) {
    r_NotificationMSG.SetActive(true);
    r_NotificationText.text = msg;
    yield return new WaitForSeconds(3);
    r_NotificationMSG.SetActive(false);
  }
}
