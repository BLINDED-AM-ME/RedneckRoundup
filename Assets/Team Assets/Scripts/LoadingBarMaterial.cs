using UnityEngine;
using System.Collections;

public class LoadingBarMaterial : MonoBehaviour {

	public GameComponentManager manager;
	public Material loading_barMaterial;

	private float _target = 0.5f;
	private float _current = 0.5f;

	// Use this for initialization
	void Start () {

		_current = 0.5f;
		_target = _current;

		loading_barMaterial.SetTextureOffset("_Stencil", new Vector2(_current,0));
	
	}
	
	// Update is called once per frame
	void Update () {

		_target = (manager.PercentComplete/100.0f); // convert to 0-1
		_current = Mathf.Min(_current + 0.05f, _target);

		loading_barMaterial.SetTextureOffset("_Stencil", new Vector2(0.5f - _current,0));

	
	}


}
