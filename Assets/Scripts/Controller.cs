using UnityEngine;

namespace Alakajam4 {
	[RequireComponent(typeof(Camera))]
	public class Controller : MonoBehaviour {

		public GameObject stabiliseEffectPrefab;

		private Camera cam;
		private ParticleSystem stabEffectSystem;
		private bool stabilizeInProgress = false;

		private void Awake() {
			cam = GetComponent<Camera>();
			stabEffectSystem = Instantiate(stabiliseEffectPrefab, transform).GetComponent<ParticleSystem>();
			stabEffectSystem.gameObject.SetActive(false);
		}

		private void Update() {

			Vector3 mPos = Input.mousePosition;
			Ray ray = cam.ScreenPointToRay(mPos);
			RaycastHit hit;
			if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hit, 200f, LayerMask.GetMask("Blocks"))) {
				hit.collider.GetComponent<TowerBlock>().isStabilised = true;
				if (!stabilizeInProgress) {
					stabilizeInProgress = true;
					stabEffectSystem.gameObject.SetActive(true);
					stabEffectSystem.Play(true);
				}
				stabEffectSystem.transform.position = hit.point;
				stabEffectSystem.transform.rotation = Quaternion.LookRotation(hit.normal);
			} else if (stabilizeInProgress) {
				stabilizeInProgress = false;
				stabEffectSystem.gameObject.SetActive(false);
				stabEffectSystem.Stop(true);
			}

		}

	}
}
