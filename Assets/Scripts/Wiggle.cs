using UnityEngine;

class Wiggle : Repeater {
	GameObject centerPivot;
	float sceneStart;
	float sceneLength;
	
	bool doWiggle;
	
	const int zoomTicks = 5;
	const int wiggleTicks = 15;

	public Wiggle(float startTime, float sceneLength, Sprite sprite) : base(0.05f, 0, startTime) {
		sceneStart = startTime;
		this.sceneLength = sceneLength;
		
		centerPivot = sprite.createPivotOnCenter();
		doWiggle = false;
	}
	
	public override void OnTick() {
		float time = Time.time;
		if (time - sceneStart >= sceneLength && (time - sceneStart) % sceneLength <= interval) {
			wiggleNow(time);
		}
		
		if (!doWiggle) return;
		
		if (currentTick < zoomTicks) {
			zoomIn();
		} else if (currentTick < zoomTicks + wiggleTicks) {
			wiggle();
		} else if (currentTick < zoomTicks + wiggleTicks + zoomTicks) {
			centerPivot.transform.rotation = Quaternion.identity;
			zoomOut();
		} else {
			doWiggle = false;
		}
	}
	
	public void wiggleNow(float wiggleTime) {
		if (doWiggle) return; // already wiggling
		Reset(wiggleTime);
		doWiggle = true;
	}
	
	public void Destroy() {
		GameObject.Destroy(centerPivot);
	}
	
	private void zoomIn() {
		zoomFor(currentTick);
	}

	private void zoomOut() {
		int zoomOutTicks = currentTick - zoomTicks - wiggleTicks;
		zoomFor(zoomTicks - zoomOutTicks);
	}
	
	private void zoomFor(int tick) {
		centerPivot.transform.localScale = Vector3.one * (1f + tick / (float) zoomTicks / 24f);
	}
	
	private void wiggle() {
		float angle = Mathf.PingPong(currentTick - zoomTicks / 64f * Mathf.PI, Mathf.PI / 16f);
		centerPivot.transform.Rotate(Vector3.back, angle, Space.Self);
	}
}
