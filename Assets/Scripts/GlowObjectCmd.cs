using UnityEngine;

namespace Alakajam4 {
	public class GlowObjectCmd : MonoBehaviour {

		public Color glowColor;
		public bool useLerp = false;
		public float lerpFactor = 10f;

		public Renderer[] renderers { get; private set; }
		public Color currentColor { get; private set; }
		public bool visible { get; private set; }

		private Color targetColor;

		private void Start() {
			renderers = GetComponentsInChildren<Renderer>();
			GlowController.RegisterObject(this);
			currentColor = Color.clear;
			enabled = false;
			visible = false;
		}

		public void GlowOn(Color color) {
			if (useLerp) {
				targetColor = color;
				enabled = true;
			} else {
				currentColor = color;
				visible = true;
			}
		}

		public void GlowOff() {
			if (useLerp) {
				targetColor = Color.clear;
				enabled = true;
			} else {
				currentColor = Color.clear;
				visible = false;
			}
		}

		private void Update() {
			if (useLerp) {
				currentColor = Color.Lerp(currentColor, targetColor, lerpFactor * Time.deltaTime);
			}

			if (currentColor.a < 0.05f) {
				visible = false;
			} else {
				visible = true;
			}
			if (currentColor.a > targetColor.a - 0.05f && currentColor.a < targetColor.a + 0.05f) {
				enabled = false;
			}
		}

		private void OnDestroy() {
			GlowController.UnregisterObject(this);
		}

	}
}
