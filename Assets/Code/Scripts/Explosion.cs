using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
  public Sprite r_Explosion;
  public AudioClip r_ExplosionSound;
  AudioSource r_VariableAudioSource;
  const float c_ExplosionDuration = .2f;

  void Start() {
    var obj = GameObject.FindWithTag("VariableAudioSource");
    if (obj == null) return;
    r_VariableAudioSource = obj.GetComponent<AudioSource>();
  }

  public void Explode() {
    var rb = GetComponent<Rigidbody2D>();
    var col = GetComponent<Collider2D>();
    if (rb != null) rb.linearVelocity = Vector2.zero;
    if (col != null) col.enabled = false;
    this.enabled = false;
    StartCoroutine(ExplosionStep());
    Utils.PlaySoundWithRandomPitch(r_VariableAudioSource, r_ExplosionSound);
  }

  IEnumerator ExplosionStep() {
    var spr = GetComponent<SpriteRenderer>();
    if (spr != null) spr.sprite = r_Explosion;
    yield return new WaitForSeconds(c_ExplosionDuration);
    if (CompareTag("Player")) gameObject.SetActive(false);
    else Destroy(gameObject);
  }
}
