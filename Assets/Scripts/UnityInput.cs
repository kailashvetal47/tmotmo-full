using System;
using UnityEngine;

/// Wraps UnityEngine.Input so it can be injected
public class UnityInput : AbstractInput {
	public int touchCount {
		get { return Input.touchCount; }
	}
	
	public Vector3 mousePosition {
		get { return Input.mousePosition; }
	}
		
	public Touch GetTouch(int index) {
		return Input.GetTouch(index);
	}
	
	public bool hasMoved(int touchIndex) {
		return Input.GetTouch(touchIndex).phase == TouchPhase.Moved;
	}
	
	public bool GetMouseButtonUp(int index) {
		return Input.GetMouseButtonUp(index);
	}

	public bool GetMouseButton(int index) {
		return Input.GetMouseButton(index);
	}
}
