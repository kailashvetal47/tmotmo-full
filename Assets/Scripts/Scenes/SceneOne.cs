using UnityEngine;
using System;

class SceneOne : Scene {
	public GameObject background;
	public Sprite same;
	public Sprite notSame;
	public Sprite circle;
	public Sprite triangle;
	
	Wiggler wiggler;
	UnityInput input;
	TouchSensor sensor;

	Cycler notSameCycler;
	Cycler circleCycler;
	Cycler triangleCycler;

	const int triangleWaitTime = 4;

 	// animate both shapes at the same frequency
	const float shapeSpeed = 0.5f;

	public SceneOne(SceneManager manager) : base(manager) {
		timeLength = 8.0f;
		input = new UnityInput();
	}

	public override void LoadAssets() {
		background = resourceFactory.Create("TitleScene/BackgroundQuad");
		background.active = false;

		same = resourceFactory.Create(this, "Same").GetComponent<Sprite>();
		notSame = resourceFactory.Create(this, "NotSame").GetComponent<Sprite>();
		circle = resourceFactory.Create(this, "Circle").GetComponent<Sprite>();
		triangle = resourceFactory.Create(this, "Triangle").GetComponent<Sprite>();

		same.visible(false);
		notSame.visible(false);
		circle.visible(false);
		triangle.visible(false);
	}

	public override void Setup(float startTime) {
		background.active = true;
		same.visible(true);
		notSame.visible(true);
		circle.visible(true);
		triangle.visible(true);

		same.setCenterToViewportCoord(0.35f, 0.66f);
		notSame.setCenterToViewportCoord(0.7f, 0.66f);
		circle.setCenterToViewportCoord(0.3f, 0.33f);
		triangle.setCenterToViewportCoord(0.7f, 0.33f);
		
		// hide the triangle to start
		triangle.visible(false);
		
		circleCycler = new Cycler(shapeSpeed, 0, startTime);
		circleCycler.AddSprite(circle);
		
		notSameCycler = new DelayedCycler(0.2f, 4, 1.2f, startTime);
		notSameCycler.AddSprite(notSame);

		wiggler = new Wiggler(startTime, timeLength, new[] {circle, triangle});

		sensor = new TouchSensor(input);
	}

	public override void Destroy() {
		Sprite.Destroy(circle);
		Sprite.Destroy(triangle);
		Sprite.Destroy(same);
		Sprite.Destroy(notSame);
		GameObject.Destroy(background);
		wiggler.Destroy();
	}

	public override void Update () {
		float now = Time.time;
		wiggler.Update(now);
		notSameCycler.Update(now);

		if (solved) return;

		if (circle.belowFinger(sensor)
		    && triangle.belowFinger(sensor)
		    && triangleShowing()) {
			Handheld.Vibrate();
			wiggler.wiggleNow(now);
			solvedScene();
			prompt.solve(this, "stop shapes from changing");
			return;
		} else if (circle.belowFinger(sensor)) {
			prompt.hint("stop circle from changing");
		} else if (triangle.belowFinger(sensor)) {
			prompt.hint("stop triangle from changing");
		}

		AnimateShapes(now);

		// if touched circle, draw its bright first frame
		if (sensor.changeInsideSprite(Camera.main, circle)) {
			circle.setFrame(0);
			circle.Animate();
		}

		// if touched triangle, ditto
		if (sensor.changeInsideSprite(Camera.main, triangle) && triangleShowing()) {
			triangle.setFrame(0);
			triangle.Animate();
		}
	}

	bool triangleShowing() {
		return triangleCycler != null;
	}

	void AnimateShapes(float time) {
		if (!triangleShowing() && circleCycler.Count() >= triangleWaitTime) {
			triangleCycler = new DelayedCycler(shapeSpeed, 6, 1f);
			triangle.visible(true);
			triangleCycler.AddSprite(triangle);
		}

		circleCycler.Update(time);

		if (triangleShowing()) {
			triangleCycler.Update(time);
		}
	}
}
