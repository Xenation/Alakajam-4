using UnityEngine;

namespace Alakajam4 {
	[RequireComponent(typeof(Camera))]
	public class Controller : MonoBehaviour {

		private Camera cam;

		private void Awake() {
			cam = GetComponent<Camera>();
		}

		private void Update() {
			
			if (Input.GetMouseButton(0)) {
				Vector3 mPos = Input.mousePosition;
				Ray ray = cam.ScreenPointToRay(mPos);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 200f, LayerMask.GetMask("Blocks"))) {
					hit.collider.GetComponent<TowerBlock>().isStabilised = true;
				}
			}

		}

	}
}
