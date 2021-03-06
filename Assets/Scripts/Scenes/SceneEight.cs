using UnityEngine;
using System;
using System.Collections.Generic;

class SceneEight : AbstractScene {
	public Confetti confetti;

	BigHeadProp bigHeadProp;
	GameObject faceRightParent;
	MouthAnimator mouthAnimator;
	Sprite mouthLeft, mouthRight;
	Wiggler wiggler;
	
	Vector3 previousMousePosition;

	public SceneEight(SceneManager manager) : base(manager) {
		timeLength = 4.0f;
		permitUnloadResources = false;
		bigHeadProp = new BigHeadProp(resourceFactory);
		confetti = new Confetti();
	}

	public override void LoadAssets() {
		mouthLeft = resourceFactory.Create(this, "MouthLeft-ItsInside").GetComponent<Sprite>();
		mouthRight = resourceFactory.Create(this, "MouthRight-ItsInside").GetComponent<Sprite>();
		mouthLeft.visible(false);
		mouthRight.visible(false);
	}

	public override void Setup (float startTime) {
		bigHeadProp.Setup();
		faceRightParent = bigHeadProp.faceRight.createPivotOnTopLeftCorner();

		mouthLeft.setWorldPosition(-29.5f, -56f, -5f);
		
		mouthRight.gameObject.transform.parent = faceRightParent.transform;
		mouthRight.setWorldPosition(10f, -56f, -5f);
		
		mouthLeft.visible(true);
		mouthRight.visible(true);

		mouthAnimator = new MouthAnimator(startTime, mouthLeft, mouthRight);
		
		var pivot = bigHeadProp.faceRight.createPivotOnTopLeftCorner();
		mouthRight.transform.parent = pivot.transform;
		wiggler = new Wiggler(startTime, timeLength, new[] {pivot});
	}

	public override void Update () {
		float now = Time.time;
		
		mouthAnimator.Update(now);
		wiggler.Update(now);
		
		if (fullyTilted() && !confetti.pouring) {
			messagePromptCoordinator.solve(this, "tilt head");
			solvedScene();
			confetti.Pour(now);
		}
		
		if (confetti.pouring) {
			confetti.Update(now);
		}
		
		if (confetti.finishedPouring) {
			endScene();
			wiggler.wiggleNow(now);
		}

		if(completed) return;
		setLocationToTouch();
	}

	public override void Destroy () {
		GameObject.Destroy(faceRightParent);
		bigHeadProp.Destroy();
		mouthAnimator.Destroy();
		wiggler.Destroy();
	}

	void setLocationToTouch() {
		Vector3 movementDelta = Vector3.zero;
		
		if (Application.isEditor && input.GetMouseButton(0)) {
			movementDelta = input.mousePosition - previousMousePosition;
		}
		previousMousePosition = input.mousePosition;
		
		if (input.touchCount > 0 && input.GetTouch(0).phase == TouchPhase.Moved) {
			if (!bigHeadProp.faceRight.Contains(Camera.main, input.GetTouch(0).position)) return;
			movementDelta = new Vector3(input.GetTouch(0).deltaPosition.x, input.GetTouch(0).deltaPosition.y, 0f);
		}
		moveToLocation(movementDelta);
	}
	
	void moveToLocation(Vector3 movementDelta) {
		if (fullyTilted()) return;

		float squareMagnitude = movementDelta.x + movementDelta.y;
		if (squareMagnitude < 0) return;
		
		faceRightParent.transform.Rotate(new Vector3(0f, 0f, squareMagnitude));
	}

	bool fullyTilted() {
		return faceRightParent.transform.rotation.eulerAngles.z >= 45;
	}
	
	class MouthAnimator : Repeater {
		Sprite mouthLeft;
		Sprite mouthRight;
		int sceneFrame = 0;
		
		const int totalFramesInScene = 24;		
		
		// the speed is eight note triplets because of the lilting rhythm of the lyrics
		public MouthAnimator(float startTime, Sprite mouthLeft, Sprite mouthRight) : base(0.16666666f, 0, startTime) {
			this.mouthLeft = mouthLeft;
			this.mouthRight = mouthRight;
		}
		
		public override void OnTick() {
			var sprites = getSpritesFor(sceneFrame);

			foreach(var sprite in sprites) {
				sprite.Animate();
			}

			incrementFrame();
		}
		
		private ICollection<Sprite> getSpritesFor(int sceneFrame) {
			if (sceneFrame == 0) return initialMouthFrame();
			if (sceneFrame >= 2 && sceneFrame <= 3) return sayIts();
			if (sceneFrame >= 4 && sceneFrame <= 6) return sayInside();
			if (sceneFrame >= 9 && sceneFrame <= 10) return saySo();
			if (sceneFrame >= 12 && sceneFrame <= 16) return sayUnder();
			if (sceneFrame >= 17 && sceneFrame <= 19) return sayStand();
			
			return new List<Sprite>();
		}
		
		public void Destroy() {
			Sprite.Destroy(mouthLeft);
			Sprite.Destroy(mouthRight);
		}
		
		private void incrementFrame() {
			sceneFrame = (sceneFrame + 1) % totalFramesInScene;
		}
		

		private ICollection<Sprite> initialMouthFrame()
		{
			setMouthFrame(15);
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> sayIts() {
			if (sceneFrame == 1)	{
				setMouthFrame(0);
			}
			else nextMouthFrame();
			
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> sayInside() {
			if (sceneFrame == 4)	{
				setMouthFrame(2);
			}
			else if (sceneFrame == 6) {
				setMouthFrame(2);
			}
			else if (sceneFrame == 7) {
				setMouthFrame(4);
			}
			else nextMouthFrame();
			
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> saySo() {
			if (sceneFrame == 9)	{
				setMouthFrame(5);
			}
			else nextMouthFrame();
			return new List<Sprite>{ mouthLeft, mouthRight };
		}
		
		private ICollection<Sprite> sayUnder() {
			if (sceneFrame == 12) {
				setMouthFrame(7);
			}
			else nextMouthFrame();
			return new List<Sprite>{ mouthLeft, mouthRight };
		}
		
		private ICollection<Sprite> sayStand() {
			if (sceneFrame == 17) {
				setMouthFrame(11);
			}
			else nextMouthFrame();
			return new List<Sprite>{ mouthLeft, mouthRight };
		}
		
		private void setMouthFrame(int index) {
			mouthLeft.setFrame(index);
			mouthRight.setFrame(index);
		}
		
		private void nextMouthFrame() {
			mouthLeft.nextFrame();
			mouthRight.nextFrame();
		}
	}
}
