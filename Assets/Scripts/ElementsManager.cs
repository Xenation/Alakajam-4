using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Xenon;

namespace Alakajam4 {
	[ExecuteInEditMode]
	public class ElementsManager : Singleton<ElementsManager> {

		[System.Serializable]
		public struct ElemDefinition {
			public Element element;
			public GameObject prefab;
			public float spawnChance;
		}

		[SerializeField]
		private ElemDefinition[] elements;

		private void Update() {
#if UNITY_EDITOR
			if (!EditorApplication.isPlaying && elements != null) {
				for (int i = 0; i < elements.Length; i++) {
					elements[i].element = (Element) i;
				}
			}
#endif
		}

		public GameObject GetPrefab(Element elem) {
			return elements[(int) elem].prefab;
		}

		public Element GetRandomElement() {
			float rnd = Random.value;
			float currentProb = 0f;
			int elemIndex;
			for (elemIndex = 0; elemIndex < elements.Length; elemIndex++) {
				if (rnd < currentProb + elements[elemIndex].spawnChance) {
					break;
				}
				currentProb += elements[elemIndex].spawnChance;
			}
			if (elemIndex == elements.Length) elemIndex = 5;
			return (Element) elemIndex;
		}

		public float GetProbability(Element elem) {
			return elements[(int) elem].spawnChance;
		}

	}
}
