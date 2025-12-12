using UnityEngine;

struct Utils {
  public static void PlaySoundWithRandomPitch(AudioSource src, AudioClip sound) {
    if (src == null || sound == null) return;
    int[] semitones = {-4, -2, 0, 2, 4, 7, 9};
    var semitone = semitones[Random.Range(0, semitones.Length - 1)];
    src.pitch = Mathf.Pow(2, (float) semitone/12);
    src.PlayOneShot(sound);
  }
}
