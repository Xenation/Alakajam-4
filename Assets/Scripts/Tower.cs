using System.Collections.Generic;
using UnityEngine;

namespace Alakajam4 {
	public class Tower : MonoBehaviour {

		[SerializeField] public Vector3Int towerSize;
		[SerializeField] public float blockSize;

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

		private void Start() {
			floors = new TowerFloor[towerSize.y];
			blocks = new TowerBlock[towerSize.x, towerSize.y, towerSize.z];
			GenerateRandomTower();
		}

		private void GenerateRandomTower() {
			for (int i = 0; i < towerSize.y; i++) {
				TowerFloor floor = CreateTowerFloor(i);
				floor.GenerateRandomBlocks();
				floors[i] = floor;
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

	}
}
