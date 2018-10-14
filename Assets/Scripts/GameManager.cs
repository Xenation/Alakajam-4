using UnityEngine;
using UnityEngine.UI;
using Xenon;
using Xenon.Processes;

namespace Alakajam4 {
	public class GameManager : Singleton<GameManager> {

		[Header("UI")]
		public RectTransform usualDisplay;
		public Text scoreText;
		public RectTransform gameEndDisplay;
		public Text gameEndScoretext;
		[Header("Score")]
		public int transformationScore = 10;
		public int explosionScore = 50;
		public int uncontrolledDestroyScore = 20;
		[Header("Others")]
		public OrbitalCamera orbCam;
		public Controller controller;
		public float gameEndScreenDuration = 3f;

		[System.NonSerialized]
		public int score = 0;

		private int neutralCount = 0;
		private bool gameEnded = false;
		private bool inGameOverScreen = false;
		private float gameOverStartTime = 0f;
		private ProcessManager procManager;

		private void Start() {
			procManager = new ProcessManager();
			Tower tower = Tower.I;
			tower.OnBlockDestroyedEvent += OnBlockDestroyed;

			for (int y = 0; y < tower.towerSize.y; y++) {
				for (int z = 0; z < tower.towerSize.z; z++) {
					for (int x = 0; x < tower.towerSize.x; x++) {
						Vector3Int pos = new Vector3Int(x, y, z);
						if (tower[pos].element == Element.Paruflore) {
							neutralCount++;
						}
					}
				}
			}
		}

		private void Update() {
			Tower tower = Tower.I;
			neutralCount = 0;
			for (int y = 0; y < tower.towerSize.y; y++) {
				for (int z = 0; z < tower.towerSize.z; z++) {
					for (int x = 0; x < tower.towerSize.x; x++) {
						Vector3Int pos = new Vector3Int(x, y, z);
						if (tower[pos] != null && tower[pos].element == Element.Paruflore) {
							neutralCount++;
						}
					}
				}
			}
			if (neutralCount == 0) {
				gameEnded = true;
			}

			procManager.UpdateProcesses(Time.deltaTime);

			scoreText.text = "Score: " + score;
			gameEndScoretext.text = scoreText.text;
			if (gameEnded) {
				if (!inGameOverScreen) {
					orbCam.locked = true;
					controller.locked = true;
					inGameOverScreen = true;
					gameOverStartTime = Time.time;
					DisplayEndGame();
				}
				if (Time.time - gameOverStartTime > gameEndScreenDuration) {
					SceneSwitchManager.I.LoadMenu();
				}
			}
		}

		public void OnTransformation() {
			//score += transformationScore;
		}

		public void OnExplosion() {
			score += explosionScore;
		}

		public void OnUncontrolledDestroy() {
			score += uncontrolledDestroyScore;
		}

		public void OnBlockDestroyed(TowerBlock block) {
			//if (block.element == Element.Paruflore) {
			//	neutralCount--;
			//	if (neutralCount == 0) {
			//		gameEnded = true;
			//	}
			//}
		}

		public void DestroyAllBtnPressed() {
			Tower.I.DestroyAllNeutrals();
		}

		public void DisplayEndGame() {
			Process usualDisapear = new MultipleFadeProcess(usualDisplay, .25f, 1f, 0f);
			usualDisapear.TerminateCallback += UsualDisapearTerminated;
			procManager.LaunchProcess(usualDisapear);
			gameEndDisplay.gameObject.SetActive(true);
			Process gameEndAppear = new MultipleFadeProcess(gameEndDisplay, .75f, 0f, 1f);
			gameEndAppear.TerminateCallback += GameEndAppearTerminated;
			procManager.LaunchProcess(gameEndAppear);
		}

		private void GameEndAppearTerminated() {

		}

		private void UsualDisapearTerminated() {
			usualDisplay.gameObject.SetActive(false);
		}

	}
}
