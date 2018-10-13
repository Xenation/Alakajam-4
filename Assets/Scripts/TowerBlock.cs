using UnityEngine;

namespace Alakajam4 {
	public class TowerBlock : MonoBehaviour {

		public Element element;
		public float fallSpeed; // TODO replace by anim
		public bool isInTransit { get; private set; }
		public Vector3Int position {
			get {
				return _position;
			}
			private set {
				tower[_position] = null;
				_position = value;
				tower[_position] = this;
				floor = tower.GetFloor(_position.y);
			}
		}
		private Vector3Int _position;
		
		private TowerFloor floor;
		private Tower tower;
		private Collider col;
		private float startHeight;
		private float destinationHeight;
		private float transitionProgress;

		public void Init(TowerFloor floor, Vector3Int pos) {
			this.floor = floor;
			tower = floor.tower;
			position = pos;
			isInTransit = false;
		}

		private void Awake() {
			col = GetComponent<Collider>();
		}

		private void FixedUpdate() {
			if (isInTransit) {
				transitionProgress += fallSpeed * Time.fixedDeltaTime;
				if (transitionProgress >= 1f) {
					transitionProgress = 1f;
					isInTransit = false;
				}
				transform.position = new Vector3(transform.position.x, Mathf.Lerp(startHeight, destinationHeight, transitionProgress), transform.position.z);
			}
		}

		public void StartDropTransition() {
			isInTransit = true;
			transitionProgress = 0f;
			startHeight = transform.position.y;
			destinationHeight = floor.transform.position.y + tower.blockSize / 2f;
		}

		public void DestroyBlock() {
			for (int y = position.y + 1; y < tower.towerSize.y; y++) {
				TowerBlock block = tower[new Vector3Int(position.x, y, position.z)];
				if (block == null) break;
				block.position += Vector3Int.down;
				block.StartDropTransition();
			}
			Destroy(gameObject);
		}

	}
}
