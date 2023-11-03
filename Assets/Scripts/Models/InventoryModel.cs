using System;
using System.IO;
using Newtonsoft.Json;
using Project.Common.Enums;
using Project.Entities;
using Project.Entities.Prices;
using UnityEngine;

namespace Project.Models
{
	public class InventoryModel
	{
		private readonly string _pathToPlayerInventory = $"{Application.dataPath}/Config/playerInventory.json";
		private readonly string _pathToTraderInventory = $"{Application.dataPath}/Config/traderInventory.json";

		public Inventory GetPlayerInventory()
		{
			return Get(_pathToPlayerInventory);
		}
		
		public Inventory GetTraderInventory()
		{
			return Get(_pathToTraderInventory);
		}

		public void UpdatePlayerInventory(Inventory state)
		{
			UpdateState(state, _pathToPlayerInventory);
		}
		
		public void UpdateTraderInventory(Inventory state)
		{
			UpdateState(state, _pathToTraderInventory);
		}

		private void UpdateState(Inventory state, string path)
		{
			try
			{
				var data = JsonConvert.SerializeObject(state, Formatting.Indented);
				File.WriteAllText(path, data);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		private Inventory Get(string path)
		{
			Inventory inventory = null;
			try
			{
				var data = File.ReadAllText(path);
				inventory = JsonConvert.DeserializeObject<Inventory>(data);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}

			return inventory;
		}

		public void CalculatePrices(Actor actor, Inventory inventory)
		{
			if (inventory == null)
				return;
			
			foreach (var cell in inventory.Cells)
			{
				IPriceModifier modifier = new BasePrice(cell);
				modifier = new BasePriceModifier(actor, modifier);
				modifier = new ReputationPriceModifier(actor, modifier);
				cell.Item.Price = modifier.GetPrice();
			}
		}
	}
}