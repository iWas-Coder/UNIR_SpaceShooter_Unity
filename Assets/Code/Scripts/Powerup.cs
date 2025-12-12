using UnityEngine;

public class Powerup {
  bool m_Claimed = false;
  int m_TargetScore;

  public Powerup(int tgt_score) {
    this.m_TargetScore = tgt_score;
  }

  public bool Claim() {
    var score = GameContext.s_Instance.GetScore();
    if (!m_Claimed && score >= m_TargetScore) {
      m_Claimed = true;
      return true;
    }
    return false;
  }
}
