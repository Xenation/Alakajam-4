using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xenon.Processes;

namespace Alakajam4 {
	public class MultipleFadeProcess : InterpolateProcess {

		private List<Graphic> graphics = new List<Graphic>();

		public MultipleFadeProcess(Transform graphicsRoot, float duration, float startValue, float endValue) : base(duration, startValue, endValue) {
			graphicsRoot.GetComponentsInChildren(true, graphics);
		}
		
		public override void TimeUpdated() {
			float alpha = CurrentValue;
			foreach (Graphic graphic in graphics) {
				Color color = graphic.color;
				color.a = alpha;
				graphic.color = color;
			}
		}

	}
}
