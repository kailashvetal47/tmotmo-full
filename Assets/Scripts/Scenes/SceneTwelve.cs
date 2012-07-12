using UnityEngine;

public class SceneTwelve : Scene {
	GameObject background, bottomDither;
	
	Sprite topHand, bottomPalm, bottomWrist;
	
	Sprite bottomHandFingers, indexOpen, indexClosed, littleFingerOpen,
		   middleFingerOpen, otherFingerOpen, thumbOpen;
	
	Sprite[] openFingers;
	int nextFinger = 0;
	
	ArmSwinger armSwinger;

	TouchSensor touchSensor;
	
	bool gripReleased = false;
	Metronome armMovement;

	public SceneTwelve(SceneManager manager) : base(manager) {
		touchSensor = new TouchSensor(new UnityInput());
	}
	
	public override void Setup () {
		timeLength = 8.0f;
		
		background = resourceFactory.Create(this, "TealBackground");
		bottomDither = resourceFactory.Create(this, "BottomDither");
		
		topHand = Sprite.create(this, "top_hand");
		topHand.setScreenPosition(150, 66);
		
		bottomPalm = Sprite.create(this, "hand_base_top");
		bottomPalm.setScreenPosition(100, -10);
		bottomPalm.setDepth(0);
		
		bottomWrist = Sprite.create(this, "hand_base_bottom");
		bottomWrist.setScreenPosition(100, -10);
		bottomWrist.setDepth(0);
		
		bottomHandFingers = Sprite.create(this, "bottom_hand_fingers");
		bottomHandFingers.setScreenPosition(143, 82);
		bottomHandFingers.setDepth(3);
		
		armSwinger = new ArmSwinger(Time.time, bottomWrist, bottomHandFingers);
		
		indexClosed = Sprite.create(this, "index_closed");
		indexClosed.setScreenPosition(186, 52);
		indexClosed.setDepth(2);

		indexOpen = Sprite.create(this, "index_open");
		indexOpen.setScreenPosition(190, 9);
		middleFingerOpen = Sprite.create(this, "middle_finger_open");
		middleFingerOpen.setScreenPosition(258, 22);
		otherFingerOpen = Sprite.create(this, "other_finger_open");
		otherFingerOpen.setScreenPosition(283, 42);
		littleFingerOpen = Sprite.create(this, "little_finger_open");
		littleFingerOpen.setScreenPosition(300, 70);
		thumbOpen = Sprite.create(this, "thumb_open");
		thumbOpen.setScreenPosition(110, 80);
		
		openFingers = new Sprite[] { thumbOpen, indexOpen, middleFingerOpen, otherFingerOpen, littleFingerOpen };
		initializeOpenFingers(openFingers);
	}

	public override void Update () {
		armSwinger.Update();
		
		if (gripReleased) {
			if (armMovement.currentTick(Time.time) > 2) {
				// cut to guy falling
				hideLargeSceneProps();
			}
			if (armMovement.isNextTick(Time.time)) {
				moveTopArmUp();
			}
			return;
		}

		if (openFingers.Length == nextFinger) {
			gripReleased = true;
			armMovement = new Metronome(Time.time, 0.1f);
			return;
		}
		
		if (touchSensor.insideSprite(topHand)) {
			openFingers[nextFinger].visible(true);
			
			if (nextFinger < 4) {
				armSwinger.swing(Time.time);
			}
			
			if (openFingers[nextFinger] == indexOpen) {
				indexClosed.visible(false);
			}
			
			nextFinger++;
		}
	}

	private void moveTopArmUp() {
		topHand.move(-2, 10);
		foreach(Sprite finger in openFingers) {
			finger.move(-2, 10);
		}
	}

	public override void Destroy () {
		GameObject.Destroy(background);
		GameObject.Destroy(bottomDither);
		Sprite.Destroy(topHand);
		Sprite.Destroy(bottomPalm);
		Sprite.Destroy(bottomWrist);
		Sprite.Destroy(bottomHandFingers);
		Sprite.Destroy(indexClosed);

		foreach(var finger in openFingers) {
			Sprite.Destroy(finger);
		}
	}
	
	private void hideLargeSceneProps() {
		topHand.visible(false);
		bottomPalm.visible(false);
		bottomWrist.visible(false);
		bottomHandFingers.visible(false);
		foreach(var finger in openFingers) {
			finger.visible(false);
		}
	}
	
	private void initializeOpenFingers(Sprite[] openFingers) {
		foreach(var finger in openFingers) {
			finger.setDepth(3); // put it on top of the hand
			finger.visible(false);
		}
	}
	
	class ArmSwinger {
		private const int swingLength = 16;
		private const float swingIncrement = 0.2f;

		bool swinging;
		float startedSwinging;
		Sprite bottomArm, bottomHand;
		GameObject swingPivot;
		Metronome swingInterval;
		
		public ArmSwinger(float startTime, Sprite bottomArm, Sprite bottomHand) {
			this.bottomArm = bottomArm;
			this.bottomHand = bottomHand;
			swingPivot = bottomArm.createPivotOnTopLeftCorner();
			
			swingInterval = new Metronome(startTime, 0.1f);
		}
		
		public void swing(float time) {
			swinging = true;
			startedSwinging = time;
		}
		
		public void Update() {
			if (swinging && swingInterval.isNextTick(Time.time)) {
				if (swingInterval.nextTick < swingLength)
					swingPivot.transform.Rotate(0f, 0f, swingIncrement);
				if (swingInterval.nextTick >= swingLength && swingInterval.nextTick < swingLength * 2)
					swingPivot.transform.Rotate(0f, 0f, -swingIncrement);
				if (swingInterval.nextTick == swingLength * 2) {
					swinging = false;
					swingInterval = new Metronome(Time.time, 0.1f);
					startedSwinging = Time.time;
				}

			}
		}
	}
}
