using Project.Entities;
using TMPro;
using UnityEngine;

namespace Project.Views
{
	public class CellViewForTrade : MonoBehaviour
	{
		[SerializeField] 
		private TMP_Text _itemNameDisplay;

		[SerializeField] 
		private TMP_Text _itemAmountDisplay;
		
		public Cell Cell { get; private set; }
		
		public void Init(Cell cell)
		{
			Cell = cell;
			_itemNameDisplay.text = cell.Item.Name;
			_itemAmountDisplay.text = cell.Amount.ToString();
		}

		public void UpdateAmount(int value)
		{
			Cell.Amount += value;
			_itemAmountDisplay.text = Cell.Amount.ToString();
		}
	}
}