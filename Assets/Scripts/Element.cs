using System.Collections.Generic;

namespace Alakajam4 {
	public enum Element : int {
		None = 0,
		Elem1 = 1,
		Elem2 = 2,
		Elem3 = 3,
		Elem4 = 4,
		ExplodeElement = 5
	}
	public static class ElementExt {

		public static readonly int[,] reactionMatrice = {
			{0, 5, 4, 2},
			{5, 0, 1, 3},
			{4, 1, 0, 0},
			{2, 3, 0, 0}
		};

		public static Element ReactWith(this Element elem, Element other) {
			return (Element) reactionMatrice[(int) elem, (int) other];
		}
	}
}
