using UnityEngine;

public class Crosshair : MonoBehaviour
{
  void Update() {
    var mouse = Input.mousePosition;
    mouse.z = 1;
    transform.position = Camera.main.ScreenToWorldPoint(mouse);
  }
}
