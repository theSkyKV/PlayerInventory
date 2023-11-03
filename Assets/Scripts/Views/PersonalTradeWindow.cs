using System.Collections.Generic;
using Project.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Views
{
	public class PersonalTradeWindow : MonoBehaviour
	{
		[SerializeField] 
		private InventoryWindow _inventoryWindow;

		[SerializeField] 
		private ItemsToTradeWindow _itemsToTradeWindow;

		[SerializeField] 
		private TMP_Text _moneyDisplay;

		[SerializeField] 
		private TMP_Text _totalCostDisplay;

		private int _totalCost;
		private Inventory _inventory;
		public event UnityAction InventoryUpdated;
		
		private void OnEnable()
		{
			_inventoryWindow.CellUpdated += OnCellUpdated;
			_inventoryWindow.InventoryUpdated += OnInventoryUpdated;
		}

		private void OnDisable()
		{
			_inventoryWindow.CellUpdated -= OnCellUpdated;
			_inventoryWindow.InventoryUpdated -= OnInventoryUpdated;
		}

		public void Init(Inventory inventory)
		{
			_inventory = inventory;
			UpdateMoneyDisplay();
			UpdateTotalCostDisplay(0);

			_inventoryWindow.FillInventory(inventory);
		}
		
		private void OnCellUpdated(Cell cell, int amount)
		{
			_itemsToTradeWindow.AddOrUpdateCell(cell, amount);
			UpdateTotalCostDisplay(cell.Item.Price * amount);
		}

		private void OnInventoryUpdated()
		{
			InventoryUpdated?.Invoke();
		}
		
		private void UpdateMoneyDisplay()
		{
			_moneyDisplay.text =
				$"{(_inventory.SilverAmount / 100).ToString()} g {(_inventory.SilverAmount % 100).ToString()} s";
		}

		private void UpdateTotalCostDisplay(int value)
		{
			_totalCost += value;
			_totalCostDisplay.text = $"{(_totalCost / 100).ToString()} g {(_totalCost % 100).ToString()} s";
		}

		public void ResetWindow()
		{
			_totalCost = 0;
			UpdateTotalCostDisplay(0);
			_itemsToTradeWindow.ResetWindow();
			_inventoryWindow.ResetWindow();
		}

		public List<Cell> GetItemsToTrade()
		{
			return _itemsToTradeWindow.GetCells();
		}
	}
}