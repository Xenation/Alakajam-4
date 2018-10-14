using UnityEngine;
using UnityEngine.SceneManagement;
using Xenon;

namespace Alakajam4 {
	public class SceneSwitchManager : Singleton<SceneSwitchManager> {

		public string mainMenu = "Menu";
		public string classicGame = "GameClassic";
		public string credits = "Credits";
		public string howToPlay = "HowToPlay";

		public void Awake() {
			if (I != this) {
				Destroy(gameObject);
			} else {
				DontDestroyOnLoad(gameObject);
			}
		}

		public void LoadMenu() {
			SceneManager.LoadScene(mainMenu);
		}

		public void LoadClassicGame() {
			SceneManager.LoadScene(classicGame);
		}

		public void LoadCredits() {
			SceneManager.LoadScene(credits);
		}

		public void LoadHowToPlay() {
			SceneManager.LoadScene(howToPlay);
		}

		public void QuitGame() {
			Application.Quit();
		}

	}
}
