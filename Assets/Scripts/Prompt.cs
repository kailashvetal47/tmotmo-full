using UnityEngine;

public class Prompt {
	GameObject textLabel, blackBox;
	MessageBox messageBox;
	GUIText text;
	bool correct, enabled, solveScene;
	float startTime = 0f;

	Scene sceneToSolve;
	
	const float promptTime = 1.5f;
	const float boxTime = 2.0f;
	
	public Prompt() {
	}
	
	public void Setup() {
		buildBlackBox();

		textLabel = new GameObject("prompt text");
		textLabel.active = false;
		text = textLabel.AddComponent<GUIText>();
		textLabel.transform.position = new Vector3(0f, 0.06f, -9.5f);
		Font font = (Font) Resources.Load("sierra_agi_font/sierra_agi_font", typeof(Font));
		text.font = font;

		messageBox = new MessageBox(font);
	}

	void buildBlackBox() {
		blackBox = GameObject.CreatePrimitive(PrimitiveType.Plane);
		blackBox.active = false;
		blackBox.name = "prompt background";
		blackBox.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
		blackBox.transform.Rotate(new Vector3(270f, 0f, 0f));
		blackBox.transform.position = new Vector3(0, -95, -9);
		blackBox.transform.localScale = new Vector3(30f, 1f, 1.5f);
	}

	public void Update(float time) {
		if (!enabled) return;
		
		if (time > startTime + promptTime) {
			hide();
			messageBox.show();
			if (correct && solveScene) {
				sceneToSolve.endScene();
				solveScene = false;
			}
		}
		if (time > startTime + promptTime + boxTime) {
			hide();
			enabled = false;
			hideBoxes();
		}
	}

	public void Reset() {
		hide();
		hideBoxes();
		solveScene = false;
		enabled = false;
	}
	
	public void Destroy() {
		GameObject.Destroy(textLabel);
	}

	public void solve(Scene scene, string action) {
		solveScene = true;
		correct = true;
		sceneToSolve = scene;
		print(action, "OK");
	}

	public void progress(string action) {
		correct = true;
		print(action, "OK");
	}

	public void hint(string action) {
		hint(action, "Nope.");
	}

	public void hint(string action, string message) {
		correct = false;
		print(action, message);
	}

	private void print(string action, string message) {
		messageBox.setMessage(message);
		startTime = Time.time;
		hideBoxes();
		show();
		text.text = ">" + action + "_";
	}
	
	private void show() {
		enabled = true;
		blackBox.active = true;
		textLabel.active = true;
	}
	
	private void hide() {
		blackBox.active = false;
		textLabel.active = false;
	}

	void hideBoxes() {
		messageBox.hide();
	}
}

