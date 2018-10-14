using System.Collections.Generic;
using UnityEngine;

namespace Alakajam4 {
	public enum Element : int {
		None = 0,
		Hyladron = 1,
		Trihogene = 2,
		Kyrium = 3,
		Jaziode = 4,
		Paruflore = 5,
		ExplodeElement = 6
	}
	public static class ElementExt {

		public static readonly int[][] reactionMatrice = {
			/*new int[] {0, 0, 0, 0, 0, 0, 0},
			new int[] {0, 0, 6, 0, 2, 0, 0},
			new int[] {0, 6, 0, 0, 3, 0, 0},
			new int[] {0, 0, 0, 0, 0, 0, 0},
			new int[] {0, 2, 3, 0, 0, 0, 0},
			new int[] {0, 0, 0, 0, 0, 0, 0},
			new int[] {0, 0, 0, 0, 0, 0, 0}*/

            new int[] {0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 3, 4, 6, 0, 0},
            new int[] {0, 3, 0, 0, 1, 0, 0},
            new int[] {0, 4, 0, 0, 2, 0, 0},
            new int[] {0, 6, 1, 2, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0}
        };

		public static readonly Element[] specialElements = {
			Element.None, Element.ExplodeElement
		};

		public static Element ReactWith(this Element elem, Element other) {
			return (Element) reactionMatrice[(int) elem][(int) other];
		}

		public static GameObject GetPrefab(this Element elem) {
			return ElementsManager.I.GetPrefab(elem);
		}

		public static HashSet<Element> GetAllNonReactive(this Element elem) {
			HashSet<Element> nonReactive = new HashSet<Element>();
			for (int i = 1; i < reactionMatrice[(int) elem].Length - 1; i++) {
				if ((Element) reactionMatrice[(int) elem][i] == Element.None) {
					nonReactive.Add((Element) i);
				}
			}
			return nonReactive;
		}

		public static bool IsSpecial(this Element elem) {
			for (int i = 0; i < specialElements.Length; i++) {
				if (elem == specialElements[i]) return true;
			}
			return false;
		}

		public static Element GetRandom(this HashSet<Element> elemSet) {
			List<Element> elements = new List<Element>(elemSet);
			float[] probabilities = new float[elements.Count];
			float probMult = 0f;
			for (int i = 0; i < elements.Count; i++) {
				probabilities[i] = ElementsManager.I.GetProbability(elements[i]);
				probMult += probabilities[i];
			}
			probMult = 1f / probMult;

			float rnd = Random.value;
			float currentProb = 0f;
			int elemIndex;
			for (elemIndex = 0; elemIndex < elements.Count; elemIndex++) {
				if (rnd < currentProb + probabilities[elemIndex] * probMult) {
					break;
				}
				currentProb += probabilities[elemIndex] * probMult;
			}
			if (elemIndex == elements.Count) elemIndex = 5;
			return elements[elemIndex];
		}

	}
}
