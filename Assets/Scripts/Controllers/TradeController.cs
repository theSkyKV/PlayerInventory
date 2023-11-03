using System.Collections.Generic;
using System.Linq;
using Project.Common.Enums;
using Project.Entities;
using Project.Models;

namespace Project.Controllers
{
	public class TradeController
	{
		public struct Response
		{
			public Inventory PlayerInventory;
			public Inventory TraderInventory;

			public Response(Inventory playerInventory, Inventory traderInventory)
			{
				PlayerInventory = playerInventory;
				TraderInventory = traderInventory;
			}
		}

		private readonly TradeModel _tradeModel = new();
		private readonly InventoryModel _inventoryModel = new();

		public Response StartTrade()
		{
			var playerInventory = _inventoryModel.GetPlayerInventory();
			var traderInventory = _inventoryModel.GetTraderInventory();
			_tradeModel.CalculatePrices(playerInventory, traderInventory);
			return new(playerInventory, traderInventory);
		}

		public Response Buy(List<Cell> items)
		{
			var inventories = _tradeModel.Trade(TradeAction.Buy, items);
			return new(inventories?.FirstOrDefault(i => i.Owner == Actor.Player),
				inventories?.FirstOrDefault(i => i.Owner == Actor.Trader));
		}

		public Response Sell(List<Cell> items)
		{
			var inventories = _tradeModel.Trade(TradeAction.Sell, items);
			return new(inventories?.FirstOrDefault(i => i.Owner == Actor.Player),
				inventories?.FirstOrDefault(i => i.Owner == Actor.Trader));
		}
	}
}