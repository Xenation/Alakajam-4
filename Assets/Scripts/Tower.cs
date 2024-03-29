﻿using UnityEngine;
using Xenon;

namespace Alakajam4 {
	public class Tower : Singleton<Tower> {

		public delegate void OnBlockDestroyed(TowerBlock block);
		public event OnBlockDestroyed OnBlockDestroyedEvent;

		public Vector3Int towerSize;
		public float blockSize;
		public float timeToExplode = 2f;
		public Color invalidGlowColor = Color.red;
		public Color validGlowColor = Color.green;

		public TowerBlock this[Vector3Int pos] {
			get {
				if (pos.x >= towerSize.x || pos.x < 0 || pos.y >= towerSize.y || pos.y < 0 || pos.z >= towerSize.z || pos.z < 0) {
					return null;
				}
				return blocks[pos.x, pos.y, pos.z];
			}
			set {
				if (pos.x >= towerSize.x || pos.x < 0 || pos.y >= towerSize.y || pos.y < 0 || pos.z >= towerSize.z || pos.z < 0) {
					return;
				}
				blocks[pos.x, pos.y, pos.z] = value;
			}
		}

		private TowerFloor[] floors;
		private TowerBlock[,,] blocks;

		private void Awake() {
			floors = new TowerFloor[towerSize.y];
			blocks = new TowerBlock[towerSize.x, towerSize.y, towerSize.z];
			GenerateNonReactiveTower();
		}

		private void Update() {
			ComputeReactions();
		}

		private void GenerateRandomTower() {
			for (int i = 0; i < towerSize.y; i++) {
				TowerFloor floor = CreateTowerFloor(i);
				floors[i] = floor;
				floor.GenerateRandomBlocks();
			}
		}

		private void GenerateNonReactiveTower() {
			for (int i = 0; i < towerSize.y; i++) {
				TowerFloor floor = CreateTowerFloor(i);
				floors[i] = floor;
				floor.GenerateNonReactiveFloor();
			}
		}

		private TowerFloor CreateTowerFloor(int level) {
			GameObject go = new GameObject("TowerFloor_" + level);
			go.transform.SetParent(transform);
			go.transform.localPosition = new Vector3(0f, level * blockSize, 0f);
			TowerFloor floor = go.AddComponent<TowerFloor>();
			floor.Init(this, blocks, level);
			return floor;
		}

		public TowerFloor GetFloor(int level) {
			if (level >= towerSize.y || level < 0) {
				return null;
			}
			return floors[level];
		}

		public void ComputeReactions() {
			TowerBlock[] adjacents = new TowerBlock[4];
			Element[] adjacentsReactions = new Element[4];
			for (int y = 0; y < towerSize.y; y++) {
				for (int z = 0; z < towerSize.z; z++) {
					for (int x = 0; x < towerSize.x; x++) {
						if (blocks[x, y, z] == null || !blocks[x, y, z].canReact) continue;
						blocks[x, y, z].GetFloorAdjacents(ref adjacents);

						Element result = Element.None;
						for (int i = 0; i < adjacents.Length; i++) {
							if (adjacents[i] == null || !adjacents[i].canReact) continue;
							Element adjResult = blocks[x, y, z].element.ReactWith(adjacents[i].element);
							if (result == Element.None || result == adjResult) {
								result = adjResult;
								adjacentsReactions[i] = result;
							}
						}

						if (result == Element.None) continue;
						blocks[x, y, z].TransformInto(result);
						for (int i = 0; i < adjacentsReactions.Length; i++) {
							if (adjacents[i] == null || adjacentsReactions[i] == Element.None) continue;
							adjacents[i].TransformInto(adjacentsReactions[i]);
						}
					}
				}
			}
		}

		public bool IsInBounds(Vector3Int pos) {
			return !(pos.x < 0 || pos.x >= towerSize.x || pos.y < 0 || pos.y >= towerSize.y || pos.z < 0 || pos.z >= towerSize.z);
		}

		public bool IsValidPositionForSpawn(Vector3Int pos) {
			return IsInBounds(pos) && blocks[pos.x, pos.y, pos.z] == null;
		}

		public void GetFloorAdjacents(Vector3Int position, ref TowerBlock[] adjArray) {
			adjArray[0] = this[position + new Vector3Int(0, 0, 1)];
			adjArray[1] = this[position + new Vector3Int(0, 0, -1)];
			adjArray[2] = this[position + new Vector3Int(1, 0, 0)];
			adjArray[3] = this[position + new Vector3Int(-1, 0, 0)];
		}

		public void NotifyBlockDestroy(TowerBlock block) {
			if (OnBlockDestroyedEvent != null) {
				OnBlockDestroyedEvent.Invoke(block);
			}
		}

		public void DestroyAllNeutrals() {
			for (int y = towerSize.y - 1; y >= 0; y--) {
				for (int z = towerSize.z - 1; z >= 0; z--) {
					for (int x = towerSize.x - 1; x >= 0; x--) {
						if (blocks[x, y, z] == null || blocks[x, y, z].element != Element.Paruflore) continue;
						blocks[x, y, z].DestroyBlock(false);
					}
				}
			}
		}

	}
}
