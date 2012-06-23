using UnityEngine;
using System;
using System.Collections.Generic;

class SceneSeven : Scene {
	BigHeadProp bigHeadProp;
	MouthAnimator mouthAnimator;
	Sprite mouthLeft, mouthRight;

	public SceneSeven(SceneManager manager) : base(manager) {
		bigHeadProp = new BigHeadProp(resourceFactory);
	}

	public override void Setup () {
		timeLength = 4.0f;
		endScene();
		
		bigHeadProp.Setup();

		var mouthLeftGameObject = resourceFactory.Create(this, "MouthLeft-WeNeedThisTaste");
		mouthLeft = mouthLeftGameObject.GetComponent<Sprite>();
		mouthLeft.setWorldPosition(-29.5f, -56f, -5f);

		var mouthRightGameObject = resourceFactory.Create(this, "MouthRight-WeNeedThisTaste");
		mouthRight = mouthRightGameObject.GetComponent<Sprite>();
		mouthRight.setWorldPosition(10f, -56f, -5f);

		mouthAnimator = new MouthAnimator(mouthLeft, mouthRight);
	}

	public override void Update () {
		mouthAnimator.Update(Time.time);
	}

	public override void Destroy () {
		bigHeadProp.Destroy();
		mouthAnimator.Destroy();
	}
	
	
	class MouthAnimator : Repeater {
		Sprite mouthLeft;
		Sprite mouthRight;
		int sceneFrame = 0;
		
		const int totalFramesInScene = 16;		
		
		public MouthAnimator(Sprite mouthLeft, Sprite mouthRight) : base(0.25f) {
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
			if (sceneFrame >= 1 && sceneFrame <= 3) return sayWe();
			if (sceneFrame >= 4 && sceneFrame <= 5) return sayNeed();
			if (sceneFrame >= 9 && sceneFrame <= 10) return sayThis();
			if (sceneFrame >= 11 && sceneFrame <= 14) return sayTaste();
			
			return new List<Sprite>();
		}
		
		public void Destroy() {
			GameObject.Destroy(mouthLeft.gameObject);
			GameObject.Destroy(mouthRight.gameObject);
		}
		
		private void incrementFrame() {
			sceneFrame = (sceneFrame + 1) % totalFramesInScene;
		}
		

		private ICollection<Sprite> initialMouthFrame()
		{
			setMouthFrame(0);
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> sayWe() {
			if (sceneFrame == 1)	{
				setMouthFrame(1);
			}
			else nextMouthFrame();
			
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> sayNeed() {
			if (sceneFrame == 4)	{
				setMouthFrame(3);
			}
			else nextMouthFrame();
			return new List<Sprite>{ mouthLeft, mouthRight };
		}
		
		private ICollection<Sprite> sayThis() {
			if (sceneFrame == 9) {
				setMouthFrame(6);
			}
			else nextMouthFrame();
			return new List<Sprite>{ mouthLeft, mouthRight };
		}
		
		private ICollection<Sprite> sayTaste() {
			if (sceneFrame == 11) {
				setMouthFrame(8);
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
