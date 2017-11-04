using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewPattern", menuName = "Tiles/Tiles Pattern")]
public class TilesPattern : ScriptableObject {

	public int rowNo = 3;
	public List<TileRow> pattern = new List<TileRow> ();

	/*void Awake () {
		for (int i = 0; i < rowNo; i++) {
			pattern.Add (new TileRow ());
		}
	}*/

	void OnValidate () {
		// Minimum 3 rows
		rowNo = Mathf.Max (3, rowNo);

		// Update the rows
		if (pattern.Count < rowNo) {
			int lackAmount = rowNo - pattern.Count;
			for (int i = 0; i < lackAmount; i++) {
				pattern.Add (new TileRow ());
			}
		} else if (pattern.Count > rowNo) {
			int extraAmount = pattern.Count - rowNo;
			for (int i = 0; i < extraAmount; i++) {
				pattern.RemoveAt (pattern.Count - 1);
			}
		}
	}
}

[System.Serializable]
public struct TileRow {
	public bool col1, col2, col3;
}