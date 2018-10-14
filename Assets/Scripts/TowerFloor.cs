using System.Collections.Generic;
using UnityEngine;

namespace Alakajam4 {
	public class TowerFloor : MonoBehaviour {

		[System.NonSerialized]
		public Tower tower;
		
		private int level;

		public void Init(Tower tower, TowerBlock[,,] blocks, int level) {
			this.tower = tower;
			this.level = level;
		}

		public void GenerateRandomBlocks() {
			for (int z = 0; z < tower.towerSize.z; z++) {
				for (int x = 0; x < tower.towerSize.x; x++) {
					Vector3Int pos = new Vector3Int(x, level, z);
					tower[pos] = CreateRandomTowerBlock(pos, tower.blockSize);
				}
			}
		}

		private TowerBlock CreateRandomTowerBlock(Vector3Int pos, float blockSize) {
			return CreateTowerBlock(ElementsManager.I.GetRandomElement(), pos, blockSize);
		}

		private TowerBlock CreateTowerBlock(Element elem, Vector3Int pos, float blockSize) {
			GameObject go = Instantiate(elem.GetPrefab(), transform);
			go.transform.localPosition = new Vector3(pos.x * blockSize, blockSize / 2, pos.z * blockSize);
			TowerBlock block = go.GetComponent<TowerBlock>();
			block.Init(this, pos);
			return block;
		}

		public void GenerateNonReactiveFloor() {
			Vector3Int originPos = new Vector3Int(0, level, 0);
			TowerBlock origin = CreateRandomTowerBlock(originPos, tower.blockSize);
			GenerateNonReactiveAdjacents(origin);
		}

		private void GenerateNonReactiveAdjacents(TowerBlock block) {
			HashSet<Element> nonReactives = block.element.GetAllNonReactive();
			List<Vector3Int> validAdj = block.GetValidAdjacentPositions();
			for (int i = 0; i < validAdj.Count; i++) {
				if (tower[validAdj[i]] != null) continue;
				TowerBlock[] adjAdj = new TowerBlock[4];
				tower.GetFloorAdjacents(validAdj[i], ref adjAdj);
				for (int j = 0; j < adjAdj.Length; j++) {
					if (adjAdj[j] == null) continue;
					nonReactives.IntersectWith(adjAdj[j].element.GetAllNonReactive());
				}
				TowerBlock adj = CreateTowerBlock(nonReactives.GetRandom(), validAdj[i], tower.blockSize);
				GenerateNonReactiveAdjacents(adj);
			}
		}

	}
}
