using UnityEngine;

namespace Alakajam4 {
	public class GlowObjectCmd : MonoBehaviour {

		public Color glowColor;
		public float lerpFactor = 10f;

		public Renderer[] renderers { get; private set; }
		public Color currentColor { get; private set; }

		private Color targetColor;

		private void Start() {
			renderers = GetComponentsInChildren<Renderer>();
			GlowController.RegisterObject(this);
			currentColor = Color.clear;
		}

		public void GlowOn(Color color) {
			targetColor = color;
			enabled = true;
		}

		public void GlowOff() {
			targetColor = Color.clear;
			enabled = true;
		}

		private void Update() {
			currentColor = Color.Lerp(currentColor, targetColor, lerpFactor * Time.deltaTime);

			if (currentColor.Equals(targetColor)) {
				enabled = false;
			}
		}

		private void OnDestroy() {
			GlowController.UnregisterObject(this);
		}

	}
}
