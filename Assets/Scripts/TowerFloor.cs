using UnityEngine;

namespace Alakajam4 {
	public class TowerFloor : MonoBehaviour {

		private TowerBlock[,] blocks;

		public void GenerateRandomBlocks(Vector2Int floorSize, float blockSize, GameObject blockPrefab) {
			blocks = new TowerBlock[floorSize.x, floorSize.y];
			for (int y = 0; y < floorSize.y; y++) {
				for (int x = 0; x < floorSize.x; x++) {
					blocks[x, y] = CreateRandomTowerBlock(blockPrefab, new Vector2Int(x, y), blockSize);
				}
			}
		}

		private TowerBlock CreateRandomTowerBlock(GameObject prefab, Vector2Int pos, float blockSize) {
			GameObject go = Instantiate(prefab, transform);
			go.transform.localPosition = new Vector3(pos.x * blockSize, blockSize / 2, pos.y * blockSize);
			TowerBlock block = go.GetComponent<TowerBlock>();
			block.element = (Element) Random.Range(1, 5);
			return block;
		}

	}
}
