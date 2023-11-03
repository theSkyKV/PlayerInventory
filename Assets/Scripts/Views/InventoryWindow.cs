using System.Collections.Generic;
using System.Linq;
using Project.Common.Enums;
using Project.Controllers;
using Project.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Views
{
	public class InventoryWindow : MonoBehaviour
	{
		[SerializeField]
		private Transform _contentTransform;

		[SerializeField]
		private CellViewForInventory _cellPrefab;
		
		[SerializeField]
		private InputFieldWindow _inputFieldWindow;
		
		[SerializeField]
		private LootBoxWindow _lootBoxWindow;

		private Inventory _inventory;
		private List<CellViewForInventory> _cells;
		
		private readonly LootBoxController _lootBoxController = new();
		
		public event UnityAction<Cell, int> CellUpdated;
		public event UnityAction InventoryUpdated;
		
		private void OnEnable()
		{
			_inputFieldWindow.OkButtonClicked += OnOkButtonClicked;
			_inputFieldWindow.CancelButtonClicked += OnCancelButtonClicked;
			_lootBoxWindow.GetRewardButtonClicked += OnGetRewardButtonClicked;
		}

		private void OnDisable()
		{
			_inputFieldWindow.OkButtonClicked -= OnOkButtonClicked;
			_inputFieldWindow.CancelButtonClicked -= OnCancelButtonClicked;
			_lootBoxWindow.GetRewardButtonClicked -= OnGetRewardButtonClicked;
		}

		public void FillInventory(Inventory inventory)
		{
			_inventory = inventory;
			_cells = new();
			
			foreach (var cell in inventory.Cells)
			{
				var item = Instantiate(_cellPrefab, _contentTransform);
				item.Init(cell);
				item.ChoiceButtonClicked += OnChoiceButtonClicked;
				_cells.Add(item);
			}
		}

		public void ResetWindow()
		{
			foreach (var cell in _cells)
				cell.ChoiceButtonClicked -= OnChoiceButtonClicked;
			
			_cells.Clear();
			var itemsToDestroy = new List<GameObject>();
			foreach (Transform item in _contentTransform)
				itemsToDestroy.Add(item.gameObject);
			
			itemsToDestroy.ForEach(Destroy);
		}
		
		private void OnOkButtonClicked(Cell cell, string value, InventoryWindow caller)
		{
			if (caller != this)
				return;
			
			var cellView = _cells.FirstOrDefault(c => c.Cell.ItemId == cell.ItemId);
			if (cellView == null)
				return;
			
			if (int.TryParse(value, out var amount) && amount > 0)
			{
				if (amount >= cell.Amount)
				{
					amount = cell.Amount;
					cellView.ChoiceButtonClicked -= OnChoiceButtonClicked;
					_cells.Remove(cellView);
					Destroy(cellView.gameObject);
					CellUpdated?.Invoke(cell, amount);
					_inputFieldWindow.gameObject.SetActive(false);
					return;
				}
				
				cellView.UpdateAmount(-amount);
				CellUpdated?.Invoke(cell, amount);
			}
			
			_inputFieldWindow.gameObject.SetActive(false);
		}
		
		private void OnCancelButtonClicked()
		{
			_inputFieldWindow.gameObject.SetActive(false);
		}

		private void OnChoiceButtonClicked(Cell cell)
		{
			if (cell.Item.Type == ItemType.LootBox && _inventory.Owner == Actor.Player)
				OpenLootBoxWindow(cell);
			else
				OpenInputFieldWindow(cell);
		}

		private void OpenLootBoxWindow(Cell cell)
		{
			if (_lootBoxWindow.gameObject.activeSelf)
				return;

			var box = _lootBoxController.Open(cell);
			_lootBoxWindow.gameObject.SetActive(true);
			_lootBoxWindow.Init(box);
		}
		
		private void OpenInputFieldWindow(Cell cell)
		{
			if (_inputFieldWindow.gameObject.activeSelf)
				return;
			
			_inputFieldWindow.gameObject.SetActive(true);
			_inputFieldWindow.Init(cell, this);
		}

		private void OnGetRewardButtonClicked(LootBox box)
		{
			if (_inventory.Owner != Actor.Player)
				return;
			
			_lootBoxWindow.gameObject.SetActive(false);
			_lootBoxWindow.ResetWindow();
			var result = _lootBoxController.TryGetReward(box);
			if (result)
				InventoryUpdated?.Invoke();
		}
	}
}