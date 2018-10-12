using System.Collections.Generic;
using UnityEngine;

namespace Alakajam4 {
	public class Tower : MonoBehaviour {

		[SerializeField] private Vector3Int towerSize;
		[SerializeField] private float blockSize;
		[SerializeField] private GameObject blockPrefab;

		private List<TowerFloor> floors;

		private void Start() {
			floors = new List<TowerFloor>(towerSize.y);
			GenerateRandomTower();
		}

		private void GenerateRandomTower() {
			for (int i = 0; i < towerSize.y; i++) {
				TowerFloor floor = CreateTowerFloor(i);
				floor.GenerateRandomBlocks(new Vector2Int(towerSize.x, towerSize.z), blockSize, blockPrefab);
			}
		}

		private TowerFloor CreateTowerFloor(int level) {
			GameObject go = new GameObject("TowerFloor_" + level);
			go.transform.SetParent(transform);
			go.transform.localPosition = new Vector3(0f, level * blockSize, 0f);
			TowerFloor floor = go.AddComponent<TowerFloor>();
			return floor;
		}

	}
}
