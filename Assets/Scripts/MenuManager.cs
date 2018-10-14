using UnityEngine;

namespace Alakajam4 {
	public class MenuManager : MonoBehaviour {

		public void BtnPlayPressed() {
			SceneSwitchManager.I.LoadClassicGame();
		}

		public void BtnCreditsPressed() {
			SceneSwitchManager.I.LoadCredits();
		}

		public void BtnQuitPressed() {
			SceneSwitchManager.I.QuitGame();
		}

		public void BtnHowToPlayPressed() {
			SceneSwitchManager.I.LoadHowToPlay();
		}

	}
}
