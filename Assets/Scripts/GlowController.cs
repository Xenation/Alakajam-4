using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Xenon;

namespace Alakajam4 {
	public class GlowController : Singleton<GlowController> {

		private CommandBuffer commandBuffer;

		private List<GlowObjectCmd> glowableObjects = new List<GlowObjectCmd>();
		private Material glowMat;
		private Material blurMaterial;
		private Vector2 blurTexelSize;

		private int prePassRenderTexID;
		private int blurPassRenderTexID;
		private int tmpRenderTexID;
		private int blurSizeID;
		private int glowColorID;

		private void Awake() {
			glowMat = new Material(Shader.Find("Hidden/GlowCmdShader"));
			blurMaterial = new Material(Shader.Find("Hidden/Blur"));

			prePassRenderTexID = Shader.PropertyToID("_GlowPrePassTex");
			blurPassRenderTexID = Shader.PropertyToID("_GlowBlurredTex");
			tmpRenderTexID = Shader.PropertyToID("_TempTex0");
			blurSizeID = Shader.PropertyToID("_BlurSize");
			glowColorID = Shader.PropertyToID("_GlowColor");

			commandBuffer = new CommandBuffer();
			commandBuffer.name = "Glowing Object Buffer";
			GetComponent<Camera>().AddCommandBuffer(CameraEvent.BeforeImageEffects, commandBuffer);
		}

		public static void RegisterObject(GlowObjectCmd glowObject) {
			if (I != null) {
				I.glowableObjects.Add(glowObject);
			}
		}

		public static void UnregisterObject(GlowObjectCmd glowObject) {
			if (I != null) {
				I.glowableObjects.Remove(glowObject);
			}
		}

		private void RebuildCommandBuffer() {
			commandBuffer.Clear();

			commandBuffer.GetTemporaryRT(prePassRenderTexID, Screen.width, Screen.height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, QualitySettings.antiAliasing);
			commandBuffer.SetRenderTarget(prePassRenderTexID);
			commandBuffer.ClearRenderTarget(true, true, Color.clear);

			//Debug.Log(string.Format("glowable obj count: {0}", glowableObjects.Count));
			for (int i = 0; i < glowableObjects.Count; i++) {
				if (glowableObjects[i].currentColor.Equals(Color.clear)) continue;
				commandBuffer.SetGlobalColor(glowColorID, glowableObjects[i].currentColor);
				for (int j = 0; j < glowableObjects[i].renderers.Length; j++) {
					//Debug.Log(string.Format("{0} length: {1}", glowableObjects[i].name, glowableObjects[i].renderers.Length));
					commandBuffer.DrawRenderer(glowableObjects[i].renderers[j], glowMat);
				}
			}

			commandBuffer.GetTemporaryRT(blurPassRenderTexID, Screen.width >> 1, Screen.height >> 1, 0, FilterMode.Bilinear);
			commandBuffer.GetTemporaryRT(tmpRenderTexID, Screen.width >> 1, Screen.height >> 1, 0, FilterMode.Bilinear);
			commandBuffer.Blit(prePassRenderTexID, blurPassRenderTexID);

			blurTexelSize = new Vector2(1.5f / (Screen.width >> 1), 1.5f / (Screen.height >> 1));
			commandBuffer.SetGlobalVector(blurSizeID, blurTexelSize);

			for (int i = 0; i < 4; i++) {
				commandBuffer.Blit(blurPassRenderTexID, tmpRenderTexID, blurMaterial, 0);
				commandBuffer.Blit(tmpRenderTexID, blurPassRenderTexID, blurMaterial, 1);
			}
		}

		private void Update() {
			RebuildCommandBuffer();
		}

	}
}
