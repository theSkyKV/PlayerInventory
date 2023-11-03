using System.Collections.Generic;
using Project.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Views
{
	public class LootBoxWindow : MonoBehaviour
	{
		[SerializeField]
		private Transform _contentTransform;
		
		[SerializeField]
		private CellViewForLootBox _cellPrefab;
		
		[SerializeField] 
		private Button _getRewardButton;
		
		[SerializeField] 
		private TMP_Text _totalPriceDisplay;
		
		private List<CellViewForLootBox> _cells;
		private LootBox _box;
		private int _choiceOrder;
		private int _totalPrice;
		private int _price;

		public event UnityAction<LootBox> GetRewardButtonClicked;
		
		private void OnEnable()
		{
			_getRewardButton.onClick.AddListener(OnGetRewardButtonClicked);
		}

		private void OnDisable()
		{
			_getRewardButton.onClick.RemoveListener(OnGetRewardButtonClicked);
		}

		public void Init(LootBox box)
		{
			_box = box;
			_cells = new();
			_choiceOrder = 1;
			_totalPrice = 0;
			_price = 0;
			_totalPriceDisplay.gameObject.SetActive(box.BoxType != BoxType.First);
			UpdateTotalPriceDisplay();
			
			foreach (var cell in box.ItemsToGet)
			{
				var item = Instantiate(_cellPrefab, _contentTransform);
				item.Init(cell, box.BoxType);
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
		
		private void UpdateTotalPriceDisplay()
		{
			_totalPriceDisplay.text = $"{(_totalPrice / 100).ToString()} g {(_totalPrice % 100).ToString()} s";
		}

		private void OnGetRewardButtonClicked()
		{
			GetRewardButtonClicked?.Invoke(_box);
		}

		private void OnChoiceButtonClicked(CellViewForLootBox cell)
		{
			_totalPrice += _price;
			UpdateTotalPriceDisplay();
			cell.DisableButton();
			cell.Cell.ChoiceOrder = _choiceOrder;
			_choiceOrder++;
			_price = _choiceOrder == 1 ? 0 : _box.BasePriceForRandomContent.Value;
			if (_choiceOrder > 2)
				_price = (int)(_price * _box.PriceFactor.Value * (_choiceOrder - 2));

			foreach (var cellView in _cells)
				cellView.UpdatePriceDisplay(_price);
		}
	}
}