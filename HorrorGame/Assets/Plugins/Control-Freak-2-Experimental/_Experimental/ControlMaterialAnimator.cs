using UnityEngine;


namespace ControlFreak2
{

public class ControlMaterialAnimator : ControlFreak2.Internal.ComponentBase
	{
	public bool		separateAlphaProperty = false;
	public string	alphaPropertyName = "_Alpha";
	
	public string	colorPropertyName = "_MainColor";
		
	public bool		disableRendererOnZeroAlpha = true;

	private Renderer				targetRenderer;
	private MaterialPropertyBlock	matProps;

	private int						colorPropId;
	private int						alphaPropId;

	private Color					lastSetColor;

	

/*
	// -------------------
	private class MaterialState
		{
		public MaterialPropertyBlock propBlock;

		public float baseAlpha;
		public Color baseColor;	// TODO : use this when setting material color (baseColor * controlColor)

		public Material sharedMaterial;
		public Material customMaterial;
		public Color sharedColor;
		}
*/		

	// -----------------
	override protected void OnInitComponent()
		{
		if (this.separateAlphaProperty)
			this.alphaPropId = Shader.PropertyToID(this.alphaPropertyName);
		else 
			this.alphaPropId = -1;

		this.colorPropId = Shader.PropertyToID(this.colorPropertyName);

		this.matProps = new MaterialPropertyBlock();
		
		this.matProps.SetColor(this.colorPropId, Color.white);

		if (this.alphaPropId >= 0)
			this.matProps.SetFloat(this.alphaPropId, 1.0f);

		this.lastSetColor = Color.white;

	
		this.targetRenderer = this.GetComponent<Renderer>();

		if (this.targetRenderer == null)
			{
#if UNITY_EDITOR
			Debug.LogError("ControlMaterialAnimator (" + this.name + ") have no Renderer to work with!");
#endif
			}
		else
			{
			this.targetRenderer.SetPropertyBlock(this.matProps);	
			}
		}

		

	// -------------------
	override protected void OnDestroyComponent()
		{
		}

	// -------------------
	override protected void OnEnableComponent()		{ }
	override protected void OnDisableComponent()	{ }
	


	// --------------------
	public void SetColor(Color color)
		{
		if (!this.CanBeUsed() || (this.targetRenderer == null))
			return;
			
		if (this.lastSetColor == color)	
			return;

		if (this.disableRendererOnZeroAlpha)
			this.targetRenderer.enabled = (color.a > 0.001f);
	
		this.matProps.SetColor(this.colorPropId, color);

		if (this.separateAlphaProperty && (this.alphaPropId >= 0))
			this.matProps.SetFloat(this.alphaPropId, color.a);

		this.targetRenderer.SetPropertyBlock(this.matProps);	
		}
		

	// TODO : test renderpasses when using PropBlock and clearing it.

	// if curColor == sharedMaterial.color) this.material = sharedMaterial
	}
}

