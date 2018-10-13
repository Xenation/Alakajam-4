using UnityEngine;

namespace Alakajam4 {
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class GlowComposite : MonoBehaviour {

		[Range(0f, 10f)]
		public float intensity = 2;

		private Material compositeMat;

		private void OnEnable() {
			compositeMat = new Material(Shader.Find("Hidden/GlowComposite"));
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination) {
			compositeMat.SetFloat("_Intensity", intensity);
			Graphics.Blit(source, destination, compositeMat, 0);
		}

	}
}
