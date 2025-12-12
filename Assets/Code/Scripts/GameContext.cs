using UnityEngine;
using TMPro;

public class GameContext : MonoBehaviour
{
  public static GameContext s_Instance { get; private set; }
  public TextMeshProUGUI r_ScoreCount;
  int m_Score = 0;

  public int GetScore() { return m_Score; }
  
  public void AddScore(int n) {
    m_Score += n;
    UpdateScoreUI();
  }

  void Awake() {
    if (s_Instance == null) s_Instance = this;
    else Destroy(gameObject);
  }

  void Start() {
    UpdateScoreUI();
  }

  void UpdateScoreUI() {
    r_ScoreCount.text = $"Score: {m_Score:D6}";
  }
}
