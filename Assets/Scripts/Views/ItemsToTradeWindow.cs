using System.Collections.Generic;
using System.Linq;
using Project.Entities;
using UnityEngine;

namespace Project.Views
{
	public class ItemsToTradeWindow : MonoBehaviour
	{
		[SerializeField] 
		private Transform _contentTransform;

		[SerializeField] 
		private CellViewForTrade _cellPrefab;

		private readonly List<CellViewForTrade> _cells = new();

		public void AddOrUpdateCell(Cell cell, int amount)
		{
			var cellView = _cells.FirstOrDefault(c => c.Cell.ItemId == cell.ItemId);
			if (cellView == null)
			{
				Cell newCell = new(cell.ItemId, amount);
				var item = Instantiate(_cellPrefab, _contentTransform);
				item.Init(newCell);
				_cells.Add(item);
			}
			else
			{
				cellView.UpdateAmount(amount);
			}
		}

		public List<Cell> GetCells()
		{
			return _cells?.Select(cell => cell.Cell).ToList();
		}

		public void ResetWindow()
		{
			_cells.Clear();
			var itemsToDestroy = new List<GameObject>();
			foreach (Transform item in _contentTransform)
				itemsToDestroy.Add(item.gameObject);
			
			itemsToDestroy.ForEach(Destroy);
		}
	}
}