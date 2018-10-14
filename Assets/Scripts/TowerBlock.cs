using System.Collections.Generic;
using UnityEngine;

namespace Alakajam4 {
	[RequireComponent(typeof(Collider))]
	public class TowerBlock : MonoBehaviour {

		public Element element;
		public float fallSpeed; // TODO replace by anim
		public float reactCooldown = .2f;
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
		public bool canReact {
			get {
				return !isInTransit && Time.time - lastReactTime > reactCooldown;
			}
		}
		
		private Vector3Int _position;
		private TowerFloor floor;
		protected Tower tower;
		private float startHeight;
		private float destinationHeight;
		private float transitionProgress;
		private float lastReactTime = -1000f;
		private ParticleSystem onDestroyEffect;
		private GlowObjectCmd glower;

		public virtual void Init(TowerFloor floor, Vector3Int pos) {
			this.floor = floor;
			tower = floor.tower;
			_position = pos;
			tower[_position] = this;
			isInTransit = false;
		}

		private void Awake() {
			onDestroyEffect = GetComponentInChildren<ParticleSystem>();
			glower = GetComponent<GlowObjectCmd>();
		}

		private void Update() {
			SubUpdate();
		}

		protected virtual void SubUpdate() {
			
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
			onDestroyEffect.transform.SetParent(transform.parent);
			onDestroyEffect.Play();
			onDestroyEffect.GetComponent<AutoDestroyParticleSystem>().enabled = true;
		}

		public TowerBlock[] GetFloorAdjacents() {
			TowerBlock[] adjacents = new TowerBlock[4];
			adjacents[0] = tower[_position + new Vector3Int(0, 0, 1)];
			adjacents[1] = tower[_position + new Vector3Int(0, 0, -1)];
			adjacents[2] = tower[_position + new Vector3Int(1, 0, 0)];
			adjacents[3] = tower[_position + new Vector3Int(-1, 0, 0)];
			return adjacents;
		}

		public void GetFloorAdjacents(ref TowerBlock[] adjArray) {
			adjArray[0] = tower[_position + new Vector3Int(0, 0, 1)];
			adjArray[1] = tower[_position + new Vector3Int(0, 0, -1)];
			adjArray[2] = tower[_position + new Vector3Int(1, 0, 0)];
			adjArray[3] = tower[_position + new Vector3Int(-1, 0, 0)];
		}

		public List<Vector3Int> GetValidAdjacentPositions() {
			List<Vector3Int> validAdj = new List<Vector3Int>(4);
			Vector3Int forward = _position + new Vector3Int(0, 0, 1);
			Vector3Int backwards = _position + new Vector3Int(0, 0, -1);
			Vector3Int right = _position + new Vector3Int(1, 0, 0);
			Vector3Int left = _position + new Vector3Int(-1, 0, 0);
			if (tower.IsInBounds(forward)) {
				validAdj.Add(forward);
			}
			if (tower.IsInBounds(backwards)) {
				validAdj.Add(backwards);
			}
			if (tower.IsInBounds(right)) {
				validAdj.Add(right);
			}
			if (tower.IsInBounds(left)) {
				validAdj.Add(left);
			}
			return validAdj;
		}

		public void TransformInto(Element element) {
			GameObject go = Instantiate(element.GetPrefab(), floor.transform);
			go.transform.localPosition = new Vector3(_position.x * tower.blockSize, tower.blockSize / 2, _position.z * tower.blockSize);
			TowerBlock block = go.GetComponent<TowerBlock>();
			block.Init(tower.GetFloor(_position.y), position);
			block.MarkReacted();
			Destroy(gameObject);
		}

		public void MarkReacted() {
			lastReactTime = Time.time;
		}

		public void GlowValid() {
			glower.GlowOn(tower.validGlowColor);
		}

		public void GlowInvalid() {
			glower.GlowOn(tower.invalidGlowColor);
		}

		public void GlowOff() {
			glower.GlowOff();
		}

	}
}
