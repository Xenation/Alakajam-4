using UnityEngine;

namespace Alakajam4 {
	[RequireComponent(typeof(Camera))]
	public class Controller : MonoBehaviour {

		public GameObject stabiliseEffectPrefab;

		private Camera cam;
		private ParticleSystem stabEffectSystem;
		private TowerBlock targetedBlock = null;

		private void Awake() {
			cam = GetComponent<Camera>();
			stabEffectSystem = Instantiate(stabiliseEffectPrefab, transform).GetComponent<ParticleSystem>();
			stabEffectSystem.gameObject.SetActive(false);
		}

		private void Update() {

			Vector3 mPos = Input.mousePosition;
			Ray ray = cam.ScreenPointToRay(mPos);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 200f, LayerMask.GetMask("Blocks"))) {
				TowerBlock block = hit.collider.GetComponent<TowerBlock>();
				if (block != targetedBlock) {
					if (targetedBlock != null) {
						targetedBlock.GlowOff();
					}
					targetedBlock = block;
					if (targetedBlock.element == Element.Paruflore) {
						targetedBlock.GlowValid();
					} else {
						targetedBlock.GlowInvalid();
					}
				}
				if (Input.GetMouseButtonDown(0)) {
					if (targetedBlock.element == Element.Paruflore) {
						targetedBlock.DestroyBlock();
					}
				}
			} else if (targetedBlock != null) {
				targetedBlock.GlowOff();
				targetedBlock = null;
			}

		}

	}
}
