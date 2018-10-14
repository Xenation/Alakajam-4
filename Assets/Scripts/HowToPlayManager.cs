using UnityEngine;

namespace Alakajam4 {
	public class HowToPlayManager : MonoBehaviour {

		public void BtnPlayPressed() {
			SceneSwitchManager.I.LoadClassicGame();
		}

		public void BtnBackPressed() {
			SceneSwitchManager.I.LoadMenu();
		}

	}
}
