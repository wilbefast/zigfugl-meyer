using UnityEngine;
using System.Collections;

public class CustomDepthViewer : MonoBehaviour
{
	public Renderer target;
	public ZigResolution TextureSize = ZigResolution.QQVGA_160x120;
	public Color32 BaseColor = Color.yellow;
	public bool UseHistogram = true;
	public int MaxDepth = 10000; //DO NOT MODIFY IN RUNTIME!!
	
	
	Texture2D texture;
	ResolutionData textureSize;
	float[] depthHistogramMap;
	Color32[] depthToColor;
	Color32[] outputPixels;
	Rect destination;
	
	// Use this for initialization
	void Start ()
	{
		if (target == null) 
		{
			target = renderer;
		}
		textureSize = ResolutionData.FromZigResolution (TextureSize);
		texture = new Texture2D (textureSize.Width, textureSize.Height);
		texture.wrapMode = TextureWrapMode.Clamp;
		depthHistogramMap = new float[MaxDepth];
		depthToColor = new Color32[MaxDepth];
		outputPixels = new Color32[textureSize.Width * textureSize.Height];
		ZigInput.Instance.AddListener (gameObject);

		if (null != target) 
		{
			target.material.mainTexture = texture;
		}
		
		destination = new Rect(Screen.width / 3, Screen.height / 2, 
														Screen.width / 3, 2 * Screen.height / 5);
	}

	void UpdateHistogram (ZigDepth depth)
	{
		int i, numOfPoints = 0;

		System.Array.Clear (depthHistogramMap, 0, depthHistogramMap.Length);
		short[] rawDepthMap = depth.data;

		int depthIndex = 0;
		// assume only downscaling
		// calculate the amount of source pixels to move per column and row in
		// output pixels
		int factorX = depth.xres / textureSize.Width;
		int factorY = ((depth.yres / textureSize.Height) - 1) * depth.xres;
		for (int y = 0; y < textureSize.Height; ++y, depthIndex += factorY) {
			for (int x = 0; x < textureSize.Width; ++x, depthIndex += factorX) {
				short pixel = rawDepthMap [depthIndex];
				if (pixel != 0) {
					depthHistogramMap [pixel]++;
					numOfPoints++;
				}
			}
		}
		depthHistogramMap [0] = 0;
		if (numOfPoints > 0) {
			for (i = 1; i < depthHistogramMap.Length; i++) {
				depthHistogramMap [i] += depthHistogramMap [i - 1];
			}
			depthToColor [0] = Color.black;
			for (i = 1; i < depthHistogramMap.Length; i++) {
				float intensity = (1.0f - (depthHistogramMap [i] / numOfPoints));
				//depthHistogramMap[i] = intensity * 255;
				depthToColor [i].r = (byte)(BaseColor.r * intensity);
				depthToColor [i].g = (byte)(BaseColor.g * intensity);
				depthToColor [i].b = (byte)(BaseColor.b * intensity);
				depthToColor [i].a = 255;//(byte)(BaseColor.a * intensity);
			}
		}
        

	}

	void UpdateTexture (ZigDepth depth)
	{
		short[] rawDepthMap = depth.data;
		int depthIndex = 0;
		int factorX = depth.xres / textureSize.Width;
		int factorY = ((depth.yres / textureSize.Height) - 1) * depth.xres;
		// invert Y axis while doing the update
		for (int y = textureSize.Height - 1; y >= 0; --y, depthIndex += factorY) {
			int outputIndex = y * textureSize.Width;
			for (int x = 0; x < textureSize.Width; ++x, depthIndex += factorX, ++outputIndex) {
				outputPixels [outputIndex] = depthToColor [rawDepthMap [depthIndex]];
			}
		}
		texture.SetPixels32 (outputPixels);
		texture.Apply ();
	}

	void Zig_Update (ZigInput input)
	{
		if (UseHistogram) {
			UpdateHistogram (ZigInput.Depth);
		} else {
			//TODO: don't repeat this every frame
			depthToColor [0] = Color.black;
			for (int i = 1; i < MaxDepth; i++) {
				float intensity = 1.0f - (i / (float)MaxDepth);
				//depthHistogramMap[i] = intensity * 255;
				depthToColor [i].r = (byte)(BaseColor.r * intensity);
				depthToColor [i].g = (byte)(BaseColor.g * intensity);
				depthToColor [i].b = (byte)(BaseColor.b * intensity);
				depthToColor [i].a = 255;//(byte)(BaseColor.a * intensity);
			}
		}
		UpdateTexture (ZigInput.Depth);
	}

	void OnGUI ()
	{
		if (null == target) 
		{
			GUI.DrawTexture(destination, texture);
		}
	}
}
