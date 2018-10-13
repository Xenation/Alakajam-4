using UnityEngine;

namespace Alakajam4 {
	public class TowerFloor : MonoBehaviour {

		[System.NonSerialized]
		public Tower tower;

		private TowerBlock[,,] blocks;
		private int level;

		public void Init(Tower tower, TowerBlock[,,] blocks, int level) {
			this.tower = tower;
			this.blocks = blocks;
			this.level = level;
		}

		public void GenerateRandomBlocks() {
			for (int z = 0; z < tower.towerSize.z; z++) {
				for (int x = 0; x < tower.towerSize.x; x++) {
					blocks[x, level, z] = CreateRandomTowerBlock(new Vector3Int(x, level, z), tower.blockSize);
				}
			}
		}

		private TowerBlock CreateRandomTowerBlock(Vector3Int pos, float blockSize) {
			Element element = ElementsManager.I.GetRandomElement();
			GameObject go = Instantiate(element.GetPrefab(), transform);
			go.transform.localPosition = new Vector3(pos.x * blockSize, blockSize / 2, pos.z * blockSize);
			TowerBlock block = go.GetComponent<TowerBlock>();
			block.Init(this, pos);
			return block;
		}

	}
}
