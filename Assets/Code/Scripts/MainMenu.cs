using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public AudioClip r_ClickSound;
  
  public void OnStartClick() {
    PlayClickSound();
    SceneManager.LoadScene("Gameplay");
  }

  public void OnExitClick() {
    PlayClickSound();
    Invoke("Exit", .5f);
  }

  void PlayClickSound() {
    var src = GetComponent<AudioSource>();
    if (src != null) src.PlayOneShot(r_ClickSound);
  }

  void Exit() {
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #endif
    Application.Quit();
  }
}
