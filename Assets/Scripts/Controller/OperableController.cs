using UnityEngine;
using System.Collections;

public abstract class OperableController : MonoBehaviour {
  abstract public void OnPointerEnter();
  abstract public void OnPointerExit();
  abstract public void OnPointerClick();
}