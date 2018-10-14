using UnityEngine;

namespace Alakajam4 {
	public class CreditsManager : MonoBehaviour {

		public void BtnMainMenuPressed() {
			SceneSwitchManager.I.LoadMenu();
		}

	}
}
