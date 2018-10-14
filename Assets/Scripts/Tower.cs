using System.Collections.Generic;
using UnityEngine;
using Xenon;

namespace Alakajam4 {
	public class Tower : Singleton<Tower> {

		public Vector3Int towerSize;
		public float blockSize;
		public float maxInstability = 15f;
		public float timeToMaxInstability = 10f;
		public float timeBetweenInstabilities = 20f;
		public float timeToExplode = 2f;
		public Color unstableGlowColor = Color.red;

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
		private float lastInstabilityTime = 0f;

		private void Start() {
			lastInstabilityTime = Time.time;
			floors = new TowerFloor[towerSize.y];
			blocks = new TowerBlock[towerSize.x, towerSize.y, towerSize.z];
			GenerateRandomTower();
		}

		private void Update() {
			ComputeReactions();
			if (Time.time - lastInstabilityTime > timeBetweenInstabilities) {
				lastInstabilityTime = Time.time;
				Vector3Int pos = new Vector3Int(Random.Range(0, towerSize.x), Random.Range(0, towerSize.y), Random.Range(0, towerSize.z));
				while (this[pos] == null || this[pos].element == Element.ExplodeElement) { // unsafe
					pos = new Vector3Int(Random.Range(0, towerSize.x), Random.Range(0, towerSize.y), Random.Range(0, towerSize.z));
				}
				this[pos].isUnstable = true;
			}
		}

		private void GenerateRandomTower() {
			for (int i = 0; i < towerSize.y; i++) {
				TowerFloor floor = CreateTowerFloor(i);
				floors[i] = floor;
				floor.GenerateRandomBlocks();
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

	}
}
