﻿using UnityEngine;

namespace Alakajam4 {
	[RequireComponent(typeof(Collider))]
	public class TowerBlock : MonoBehaviour {

		public Element element;
		public float fallSpeed; // TODO replace by anim
		public float reactCooldown = .2f;
		public bool isInTransit { get; private set; }
		public bool isUnstable {
			get {
				return _isUnstable;
			}
			set {
				_isUnstable = value;
				if (_isUnstable) {
					glower.GlowOn(tower.unstableGlowColor);
				} else {
					glower.GlowOff();
				}
			}
		}
		private bool _isUnstable = false;
		public float instability = 0f;
		public bool isStabilised = false;
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
		private float instabilityChange = 0f;
		private ParticleSystem onDestroyEffect;
		private GlowObjectCmd glower;

		public virtual void Init(TowerFloor floor, Vector3Int pos) {
			this.floor = floor;
			tower = floor.tower;
			position = pos;
			isInTransit = false;
		}

		private void Awake() {
			onDestroyEffect = GetComponentInChildren<ParticleSystem>();
			glower = GetComponent<GlowObjectCmd>();
		}

		private void Update() {
			if (isStabilised) {
				instabilityChange = -tower.maxInstability / tower.timeToMaxInstability * Time.fixedDeltaTime;
				isStabilised = false;
			} else {
				instabilityChange = tower.maxInstability / tower.timeToMaxInstability * Time.fixedDeltaTime;
			}
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
			if (isUnstable) {
				instability += instabilityChange;
				transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, (Random.value - .5f) * instability, transform.rotation.eulerAngles.z);
				if (instability > tower.maxInstability) {
					DestroyBlock();
				} else if (instability <= 0f) {
					instability = 0f;
					isUnstable = false;
				}
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

	}
}
