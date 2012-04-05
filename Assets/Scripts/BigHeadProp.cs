using System;
using UnityEngine;

public class BigHeadProp {
	GameObjectFactory<string> resourceFactory;
	
	GameObject background;
	public GameObject faceLeftObject { get; private set; }
	public GameObject faceRightObject { get; private set; }
	
	public Sprite faceLeft {
		get {
			return faceLeftObject.GetComponent<Sprite>();
		}
	}
	public Sprite faceRight {
		get {
			return faceRightObject.GetComponent<Sprite>();
		}
	}
	
	public BigHeadProp(GameObjectFactory<string> resourceFactory) {
		this.resourceFactory = resourceFactory;
	}

	public void Setup () {
		background = resourceFactory.Create(this, "PurpleQuad");

		faceLeftObject = resourceFactory.Create(this, "FaceLeft");
		var leftPosition = faceLeftObject.transform.position;
		leftPosition.x = -50f;
		leftPosition.y = -60f;
		faceLeftObject.transform.position = leftPosition;
		
		faceRightObject = resourceFactory.Create(this, "FaceRight");
		var rightPosition = faceRightObject.transform.position;
		rightPosition.x = 10f;
		rightPosition.y = -60f;
		faceRightObject.transform.position = rightPosition;
	}
	
	public void Destroy () {
		GameObject.Destroy(background);
		GameObject.Destroy(faceLeftObject);
		GameObject.Destroy(faceRightObject);
	}

}
