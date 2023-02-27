using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilController : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnPointerClick()
  {
    Debug.Log("Ceil Clicked");
    SceneTransitionManager.GetInstance().LoadScene("CorrectionScene");
  }
}
