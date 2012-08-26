using UnityEngine;
using System;

class SceneOne : Scene {
	public GameObject background;
	public GameObject same;
	public GameObject notSame;
	public GameObject circle;
	public GameObject triangle;
	
	Wiggle wiggle;

	Cycler notSameCycler;
	Cycler circleCycler;
	Cycler triangleCycler;

	int triangleWaitTime = 4;

 	// animate both shapes at the same frequency
	private float shapeSpeed = 0.5f;
	
	private UnityInput input;
	
	public SceneOne(SceneManager manager) : base(manager) {
		timeLength = 8.0f;
		input = new UnityInput();
	}

	public override void LoadAssets() {
		background = resourceFactory.Create("TitleScene/BackgroundQuad");
		same = resourceFactory.Create(this, "Same");
		notSame = resourceFactory.Create(this, "NotSame");
		circle = resourceFactory.Create(this, "Circle");
		triangle = resourceFactory.Create(this, "Triangle");

		background.active = false;
		same.active = false;
		notSame.active = false;
		circle.active = false;
		triangle.active = false;
	}

	public override void Setup(float startTime) {
		background.active = true;
		same.active = true;
		notSame.active = true;
		circle.active = true;
		triangle.active = true;

		same.GetComponent<Sprite>().setCenterToViewportCoord(0.35f, 0.66f);
		notSame.GetComponent<Sprite>().setCenterToViewportCoord(0.7f, 0.66f);
		var circleSprite = circle.GetComponent<Sprite>();
		var triangleSprite = triangle.GetComponent<Sprite>();
		circleSprite.setCenterToViewportCoord(0.3f, 0.33f);
		triangleSprite.setCenterToViewportCoord(0.7f, 0.33f);
		
		// hide the triangle to start
		triangle.active = false;
		
		circleCycler = new Cycler(shapeSpeed, 0, startTime);
		circleCycler.AddSprite(circle);
		
		notSameCycler = new DelayedCycler(0.2f, 4, 1.2f, startTime);
		notSameCycler.AddSprite(notSame);

		wiggle = new Wiggle(startTime, timeLength, new[] {circle.GetComponent<Sprite>(), triangle.GetComponent<Sprite>()});
	}

	public override void Destroy() {
		GameObject.Destroy(circle);
		GameObject.Destroy(triangle);
		GameObject.Destroy(same);
		GameObject.Destroy(notSame);
		GameObject.Destroy(background);
		wiggle.Destroy();
	}

	public override void Update () {
		float now = Time.time;
		wiggle.Update(now);
		notSameCycler.Update(now);

		var touch = new TouchSensor(input);

		bool editorTouched = Application.isEditor && input.GetMouseButtonUp(0);
		
		if (editorTouched ||
			(touch.insideSprite(Camera.main, circle.GetComponent<Sprite>()) &&
			 touch.insideSprite(Camera.main, triangle.GetComponent<Sprite>()))) {
			Handheld.Vibrate();
			wiggle.wiggleNow(now);
			endScene();
		} else {
			AnimateShapes();
		}
	}
	
	void AnimateShapes() {
		circleCycler.Update(Time.time);
		if (circleCycler.Count() == triangleWaitTime) {
			triangleCycler = new DelayedCycler(shapeSpeed, 6, 1f);
			triangle.active = true;
			triangleCycler.AddSprite(triangle);
		}
		if (triangleCycler != null) {
			triangleCycler.Update(Time.time);
		}
	}
}
