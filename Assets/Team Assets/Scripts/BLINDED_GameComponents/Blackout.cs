using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class Blackout : MonoBehaviour//UnityStandardAssets.ImageEffects.PostEffectsBase
{

	public float intensity = 1.0f;
	public Color overlayColor = Color.black;
	public Shader overlayShader = null;
	private Material overlayMaterial = null;


	protected bool  supportHDRTextures = true;
	protected bool  supportDX11 = false;
	protected bool  isSupported = true;


	void Awake(){
		overlayShader = Resources.Load("Blackout_Shader") as Shader;
	}

	void Start(){

		CheckResources();

	}
	
	public bool CheckResources ()
	{
		CheckSupport (false);
		
		overlayMaterial = CheckShaderAndCreateMaterial (overlayShader, overlayMaterial);
		
		if(!isSupported)
			Debug.LogWarning ("The image effect " + ToString() + " has been disabled as it's not supported on the current platform.");

		return isSupported;
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		
		Vector4 UV_Transform = new  Vector4(1, 0, 0, 1);
		
		#if UNITY_WP8
		// WP8 has no OS support for rotating screen with device orientation,
		// so we do those transformations ourselves.
		if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
			UV_Transform = new Vector4(0, -1, 1, 0);
		}
		if (Screen.orientation == ScreenOrientation.LandscapeRight) {
			UV_Transform = new Vector4(0, 1, -1, 0);
		}
		if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
			UV_Transform = new Vector4(-1, 0, 0, -1);
		}
		#endif

		overlayMaterial.SetVector("_UV_Transform", UV_Transform);
		overlayMaterial.SetFloat ("_Intensity", intensity);
		overlayMaterial.SetColor ("_Overlay", overlayColor);
		Graphics.Blit(source, destination, overlayMaterial, 0);
	}

	protected bool CheckSupport (bool needDepth)
	{
		isSupported = true;
		supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
		supportDX11 = SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;
		
		if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures)
		{
			NotSupported ();
			return false;
		}
		
		if (needDepth && !SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.Depth))
		{
			NotSupported ();
			return false;
		}
		
		if (needDepth)
			GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
		
		return true;
	}

	protected void NotSupported ()
	{
		enabled = false;
		isSupported = false;
		return;
	}

	protected Material CheckShaderAndCreateMaterial ( Shader s, Material m2Create)
	{
		if (!s)
		{
			Debug.Log("Missing shader in " + ToString ());
			enabled = false;
			return null;
		}
		
		if (s.isSupported && m2Create && m2Create.shader == s)
			return m2Create;
		
		if (!s.isSupported)
		{
			NotSupported ();
			Debug.Log("The shader " + s.ToString() + " on effect "+ToString()+" is not supported on this platform!");
			return null;
		}
		else
		{
			m2Create = new Material (s);
			m2Create.hideFlags = HideFlags.DontSave;
			if (m2Create)
				return m2Create;
			else return null;
		}
	}
}