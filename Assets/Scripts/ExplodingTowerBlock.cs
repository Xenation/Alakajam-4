using UnityEngine;

namespace Alakajam4 {
	public class ExplodingTowerBlock : TowerBlock {

		private float spawnTime = 0f;

		public override void Init(TowerFloor floor, Vector3Int pos) {
			base.Init(floor, pos);
			spawnTime = Time.time;
		}

		protected override void SubUpdate() {
			if (Time.time - spawnTime > tower.timeToExplode) {
				TowerBlock[] adjacents = new TowerBlock[6];
				GetCrossFloorAdjacents(ref adjacents);
				for (int i = 0; i < adjacents.Length; i++) {
					if (adjacents[i] == null || adjacents[i].element == Element.ExplodeElement) continue;
					adjacents[i].DestroyBlock();
				}
				DestroyBlock();
			}
		}

		private void GetCrossFloorAdjacents(ref TowerBlock[] adjacents) {
			if (adjacents.Length < 6) return;
			GetFloorAdjacents(ref adjacents);
			adjacents[4] = tower[position + new Vector3Int(0, 1, 0)];
			adjacents[5] = tower[position + new Vector3Int(0, -1, 0)];
		}

	}
}
