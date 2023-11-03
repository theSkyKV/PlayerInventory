using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Project.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Models
{
	public class LootBoxModel
	{
		private readonly string _itemsPath = $"{Application.dataPath}/Config/items.json";
		private readonly string _openedLootBoxesPath = $"{Application.dataPath}/Config/openedLootBoxes.json";
		private readonly InventoryModel _inventoryModel = new();
		
		private enum OpenedLootBoxesAction
		{
			Add,
			Remove
		}

		public List<LootBox> GetAll()
		{
			List<LootBox> list = null;
			try
			{
				var data = File.ReadAllText(_itemsPath);
				list = JsonConvert.DeserializeObject<IEnumerable<LootBox>>(data)
					.Where(i => i.Type == ItemType.LootBox)
					.ToList();
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}

			return list;
		}
		
		public LootBox Get(string id)
		{
			return GetAll()?.FirstOrDefault(i => i.Id == id);
		}
		
		public List<LootBox> GetAllOpened()
		{
			List<LootBox> list = null;
			try
			{
				var data = File.ReadAllText(_openedLootBoxesPath);
				list = JsonConvert.DeserializeObject<List<LootBox>>(data);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}

			return list;
		}

		public LootBox Open(LootBox box)
		{
			string message;
			
			if (box == null)
			{
				message = "Box can't be NULL";
				Debug.Log(message);
				return null;
			}
			
			var playerInventory = _inventoryModel.GetPlayerInventory();
			var cell = playerInventory.Cells.FirstOrDefault(c => c.ItemId == box.Id);
			if (cell == null)
			{
				message = $"You do not have Loot Box with Id {box.Id}";
				Debug.Log(message);
				return null;
			}
			
			var openedBox = GetAllOpened().FirstOrDefault(b => b.Id == box.Id);
			if (openedBox != null)
			{
				message = $"You have already opened Loot Box with Id {box.Id}. Take reward first";
				Debug.Log(message);
				return openedBox;
			}

			var amount = GetAmount(box);
			box.ItemsToGet = new();
			for (var i = 0; i < amount; i++)
				box.ItemsToGet.Add(new(GetItem(box)));

			box.Content = null;
			box.AmountToDrop = null;
			RewriteOpenedLootBoxes(box, OpenedLootBoxesAction.Add);
			message = "Success";
			Debug.Log(message);
			
			return box;
		}

		public bool TryGetReward(LootBox box)
		{
			string message;
			
			if (box == null)
			{
				message = "Box can't be NULL";
				Debug.Log(message);
				return false;
			}
			
			var existingBox = GetAllOpened().FirstOrDefault(b => b.Id == box.Id);
			if (existingBox == null)
			{
				message = $"You do not have opened Loot Box with Id {box.Id}";
				Debug.Log(message);
				return false;
			}

			var playerInventory = _inventoryModel.GetPlayerInventory();
			var cells = new List<Cell>();
			switch (existingBox.BoxType)
			{
				case BoxType.First:
					cells.AddRange(existingBox.ItemsToGet.Select(item => new Cell(item)));
					break;
				case BoxType.Second:
					if (existingBox.BasePriceForRandomContent == null || existingBox.PriceFactor == null)
					{
						message = "Base Price and Price Factor can't be NULL for the Second Type of Loot Box";
						Debug.LogError(message);
						return false;
					}
					
					var totalCost = 0;
					for (var i = 0; i < existingBox.ItemsToGet.Count; i++)
					{
						var choiceOrder = box.ItemsToGet[i].ChoiceOrder;
						if (choiceOrder == null)
							continue;

						var price = choiceOrder == 1 ? 0 : existingBox.BasePriceForRandomContent.Value;
						if (choiceOrder > 2)
							price = (int)(price * existingBox.PriceFactor.Value * (choiceOrder - 2));
						
						totalCost += price;
						cells.Add(new(existingBox.ItemsToGet[i]));
					}

					if (totalCost > playerInventory.SilverAmount)
					{
						message = $"You do not have enough silver {totalCost.ToString()}";
						Debug.Log(message);
						return false;
					}
					
					playerInventory.SilverAmount -= totalCost;
					break;
				default:
					message = "Incorrect Box Type";
					Debug.LogError(message);
					return false;
			}
			
			var boxCell = playerInventory.Cells.First(c => c.ItemId == box.Id);
			boxCell.Amount--;
			if (boxCell.Amount == 0)
				playerInventory.Cells.Remove(boxCell);
			
			foreach (var cell in cells)
			{
				var inventoryCell = playerInventory.Cells.FirstOrDefault(c => c.ItemId == cell.ItemId);
				if (inventoryCell == null)
					playerInventory.Cells.Add(cell);
				else
					inventoryCell.Amount += cell.Amount;
			}
			
			_inventoryModel.UpdatePlayerInventory(playerInventory);
			RewriteOpenedLootBoxes(existingBox, OpenedLootBoxesAction.Remove);
			message = "Success";
			Debug.Log(message);
			
			return true;
		}

		private void RewriteOpenedLootBoxes(LootBox box, OpenedLootBoxesAction action)
		{
			var list = GetAllOpened() ?? new();

			switch (action)
			{
				case OpenedLootBoxesAction.Add:
					list.Add(box);
					break;
				case OpenedLootBoxesAction.Remove:
					var itemToRemove = list.FirstOrDefault(i => i.Id == box.Id);
					list.Remove(itemToRemove);
					break;
			}
			
			try
			{
				var data = JsonConvert.SerializeObject(list, Formatting.Indented);
				File.WriteAllText(_openedLootBoxesPath, data);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		private int GetAmount(LootBox box)
		{
			var index = GetRandomIndex(box.AmountToDrop.Select(i => i.Weight).ToList());
			return box.AmountToDrop[index].Value;
		}
		
		private RandomContent GetItem(LootBox box)
		{
			var index = GetRandomIndex(box.Content.Select(i => i.Weight).ToList());
			return box.Content[index];
		}

		private int GetRandomIndex(List<int> weights)
		{
			var totalWeight = weights.Sum();
			var number = Random.Range(0, totalWeight);
			var sum = 0;
			for (var i = 0; i < weights.Count; i++)
			{
				sum += weights[i];
				if (number < sum)
					return i;
			}

			return 0;
		}
	}
}