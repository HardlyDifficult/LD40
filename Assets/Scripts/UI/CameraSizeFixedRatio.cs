using System;
using UnityEngine;

public class CameraSizeFixedRatio : MonoBehaviour
{
  Camera camera;
  Rect rect;

  protected void Awake()
  {
    camera = GetComponent<Camera>();
    rect = camera.rect;
  }

  protected void Update()
  {
    float aspectRatio = (float)Screen.width / Screen.height;
    Rect r = camera.rect;
    r.width = rect.width / aspectRatio;
    camera.rect = r;
  }
}
