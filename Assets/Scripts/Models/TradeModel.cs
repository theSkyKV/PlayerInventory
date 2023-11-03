using System.Collections.Generic;
using System.Linq;
using Project.Common.Enums;
using Project.Entities;
using UnityEngine;

namespace Project.Models
{
	public class TradeModel
	{
		private readonly InventoryModel _inventoryModel = new();
		private readonly AssortmentModel _assortmentModel = new();
		private readonly PackModel _packModel = new();

		public void CalculatePrices(Inventory playerInventory, Inventory traderInventory)
		{
			_inventoryModel.CalculatePrices(Actor.Player, playerInventory);
			_inventoryModel.CalculatePrices(Actor.Trader, traderInventory);
		}

		public List<Inventory> Trade(TradeAction action, List<Cell> items)
		{
			var playerInventory = _inventoryModel.GetPlayerInventory();
			var traderInventory = _inventoryModel.GetTraderInventory();
			CalculatePrices(playerInventory, traderInventory);
			
			Inventory buyer, seller;

			switch (action)
			{
				case TradeAction.Buy:
					buyer = playerInventory;
					seller = traderInventory;
					break;
				case TradeAction.Sell:
					buyer = traderInventory;
					seller = playerInventory;
					break;
				default:
					buyer = null;
					seller = null;
					break;
			}

			string message;
			
			if (buyer == null || seller == null)
			{
				message = "Actor can't be NULL";
				Debug.Log(message);
				return null;
			}
			
			var totalCost = 0;
			foreach (var item in items)
			{
				var cell = seller.Cells.FirstOrDefault(c => c.ItemId == item.ItemId);

				if (cell == null)
				{
					message = $"Seller does not have this item {item.ItemId}";
					Debug.Log(message);
					return null;
				}

				if (cell.Amount < item.Amount)
				{
					message = $"Seller does not have so many items {item.Amount.ToString()}";
					Debug.Log(message);
					return null;
				}

				cell.Amount -= item.Amount;
				if (cell.Amount == 0)
					seller.Cells.Remove(cell);

				totalCost += cell.Item.Price * item.Amount;
			}

			if (totalCost > buyer.SilverAmount)
			{
				message = $"You do not have enough silver {totalCost.ToString()}";
				Debug.Log(message);
				return null;
			}

			foreach (var item in items)
			{
				if (action == TradeAction.Sell)
				{
					var exists = _assortmentModel.Get().Exists(id => id == item.ItemId);
					if (!exists)
					{
						message = $"You cannot sell item {item.ItemId} to this trader";
						Debug.Log(message);
						return null;
					}
				}

				if (item.Item.Type == ItemType.Pack && action == TradeAction.Buy)
				{
					var pack = _packModel.Get(item.ItemId);
					foreach (var content in pack.Content)
						AddItemToInventory(buyer, content, item.Amount);

					continue;
				}

				AddItemToInventory(buyer, item);
			}

			seller.SilverAmount += totalCost;
			buyer.SilverAmount -= totalCost;

			_inventoryModel.UpdatePlayerInventory(playerInventory);
			_inventoryModel.UpdateTraderInventory(traderInventory);

			message = "Success";
			Debug.Log(message);
			
			return new() { playerInventory, traderInventory };
		}

		private void AddItemToInventory(Inventory buyer, Content content, int packageAmount = 1)
		{
			var cell = buyer.Cells.FirstOrDefault(c => c.ItemId == content.ItemId);

			if (cell == null)
			{
				cell = new(content.ItemId, 0);
				buyer.Cells.Add(cell);
			}

			cell.Amount += content.Amount * packageAmount;
		}
	}
}