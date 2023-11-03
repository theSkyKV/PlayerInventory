using Project.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Views
{
	public class TradeWindow : MonoBehaviour
	{
		[SerializeField] 
		private PersonalTradeWindow _playerTradeWindow;

		[SerializeField] 
		private PersonalTradeWindow _traderTradeWindow;

		[SerializeField] 
		private Button _sellButton;

		[SerializeField] 
		private Button _buyButton;

		private readonly TradeController _tradeController = new();

		private void OnEnable()
		{
			var data = _tradeController.StartTrade();
			
			_playerTradeWindow.Init(data.PlayerInventory);
			_traderTradeWindow.Init(data.TraderInventory);

			_sellButton.onClick.AddListener(OnSellButtonClicked);
			_buyButton.onClick.AddListener(OnBuyButtonClicked);
			_playerTradeWindow.InventoryUpdated += ResetWindow;
		}

		private void OnDisable()
		{
			_sellButton.onClick.RemoveListener(OnSellButtonClicked);
			_buyButton.onClick.RemoveListener(OnBuyButtonClicked);
			_playerTradeWindow.InventoryUpdated -= ResetWindow;
		}

		private void OnSellButtonClicked()
		{
			_tradeController.Sell(_playerTradeWindow.GetItemsToTrade());
			ResetWindow();
		}

		private void OnBuyButtonClicked()
		{
			_tradeController.Buy(_traderTradeWindow.GetItemsToTrade());
			ResetWindow();
		}

		private void ResetWindow()
		{
			_traderTradeWindow.ResetWindow();
			_playerTradeWindow.ResetWindow();
			
			var data = _tradeController.StartTrade();
			_traderTradeWindow.Init(data.TraderInventory);
			_playerTradeWindow.Init(data.PlayerInventory);
		}
	}
}