using System;

namespace UnityEngine {
    public class GameObject : Object {
        public Transform transform;

		//public Rigidbody rigidbody;
		public  Camera camera;
		//public  Light light;
		//public  Animation animation;
		//public  ConstantForce constantForce;
		//public  Renderer renderer;
		public  AudioSource audio;
		//public  GUIText guiText;
		//public  NetworkView networkView;
		//public  GUITexture guiTexture;
		//public  Collider collider;
		//public  HingeJoint hingeJoint;
		//public  ParticleEmitter particleEmitter;
		public  int layer;
		public  bool active;
		public  string tag;
		public GameObject gameObject;
		
		public GameObject() {
		}

		public GameObject(string name) {
		}

        public virtual void SetActive(bool active)
        {
            throw new InvalidOperationException();
        }

        public bool activeSelf
        {
            get;
            set;
        }
		
		public virtual Component GetComponent (Type type) { return null; }
		public virtual T GetComponent<T> () where T : Component
		{
			return null;
		}
		public virtual Component GetComponent (string type)
		{
			throw new InvalidOperationException();
		}
		public virtual Component GetComponentInChildren (Type type)
		{
			throw new InvalidOperationException();
		}
		public virtual T GetComponentInChildren<T> () where T : Component
		{
			throw new InvalidOperationException();
		}
		public virtual Component[] GetComponents (Type type)
		{
			throw new InvalidOperationException();
		}
		public virtual T[] GetComponents<T> () where T : Component
		{
			throw new InvalidOperationException();
		}
		public virtual Component[] GetComponentsInChildren (Type type)
		{
			throw new InvalidOperationException();
		}
		public virtual Component[] GetComponentsInChildren (Type type, bool includeInactive)
		{
			throw new InvalidOperationException();
		}
		public virtual T[] GetComponentsInChildren<T> (bool includeInactive) where T : Component
		{
			throw new InvalidOperationException();
		}
		public virtual T[] GetComponentsInChildren<T> () where T : Component
		{
			throw new InvalidOperationException();
		}
		public void SetActiveRecursively (bool state) { throw new InvalidOperationException(); }
		public bool CompareTag (string tag) { throw new InvalidOperationException(); }
		public static  GameObject FindGameObjectWithTag (string tag) { throw new InvalidOperationException(); }
		public static GameObject FindWithTag (string tag)
		{
			throw new InvalidOperationException();
		}
		public static  GameObject[] FindGameObjectsWithTag (string tag) { throw new InvalidOperationException(); }

		/* SendMessageOption not yet defined
		 * 
		public  void SendMessageUpwards (string methodName, object value, SendMessageOptions options)
		{
			throw new InvalidOperationException();
		}
		public void SendMessageUpwards (string methodName, object value)
		{
			throw new InvalidOperationException();
		}
		public void SendMessageUpwards (string methodName)
		{
			throw new InvalidOperationException();
		}
		public void SendMessageUpwards (string methodName, SendMessageOptions options)
		{
			throw new InvalidOperationException();
		}
		public  void SendMessage (string methodName, object value, SendMessageOptions options);
		
		public void SendMessage (string methodName, object value)
		{
			throw new InvalidOperationException();
		}
		public void SendMessage (string methodName)
		{
			throw new InvalidOperationException();
		}
		public void SendMessage (string methodName, SendMessageOptions options)
		{
			throw new InvalidOperationException();
		}
		public  void BroadcastMessage (string methodName, object parameter, SendMessageOptions options);
		public void BroadcastMessage (string methodName, object parameter)
		{
			throw new InvalidOperationException();
		}
		public void BroadcastMessage (string methodName)
		{
			throw new InvalidOperationException();
		}
		public void BroadcastMessage (string methodName, SendMessageOptions options)
		{
			throw new InvalidOperationException();
		}
		*/
		public  Component AddComponent (string className) { throw new InvalidOperationException(); }
		public Component AddComponent (Type componentType)
		{
			throw new InvalidOperationException();
		}
		public T AddComponent<T> () where T : Component
		{
			throw new InvalidOperationException();
		}
		//public  void SampleAnimation (AnimationClip animation, float time) { throw new InvalidOperationException(); }
		public static GameObject Find (string name) { throw new InvalidOperationException(); }

		public static GameObject CreatePrimitive(PrimitiveType type) { throw new InvalidOperationException(); }

	}
}
