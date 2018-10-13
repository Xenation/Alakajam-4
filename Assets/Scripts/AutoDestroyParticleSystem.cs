using UnityEngine;

namespace Alakajam4 {
	[RequireComponent(typeof(ParticleSystem))]
	public class AutoDestroyParticleSystem : MonoBehaviour {

		private ParticleSystem system;

		private void Awake() {
			system = GetComponent<ParticleSystem>();
		}

		private void Update() {
			if (!system.IsAlive()) {
				Destroy(gameObject);
				return;
			}
		}

	}
}
