using Project.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Views
{
	public class CellViewForLootBox : MonoBehaviour
	{
		[SerializeField] 
		private Button _choiceButton;

		[SerializeField] 
		private TMP_Text _itemNameDisplay;

		[SerializeField] 
		private TMP_Text _itemAmountDisplay;

		[SerializeField] 
		private TMP_Text _priceDisplay;
		
		public event UnityAction<CellViewForLootBox> ChoiceButtonClicked;
		
		public Cell Cell { get; private set; }

		public void Init(Cell cell, BoxType boxType)
		{
			Cell = cell;
			_itemNameDisplay.text = cell.Item.Name;
			_itemAmountDisplay.text = cell.Amount.ToString();
			_choiceButton.gameObject.SetActive(boxType != BoxType.First);
			UpdatePriceDisplay(0);
		}

		public void UpdatePriceDisplay(int value)
		{
			_priceDisplay.text = $"{(value / 100).ToString()} g {(value % 100).ToString()} s";
		}

		public void DisableButton()
		{
			_choiceButton.gameObject.SetActive(false);
		}
		
		private void OnEnable()
		{
			_choiceButton.onClick.AddListener(OnChoiceButtonClicked);
		}

		private void OnDisable()
		{
			_choiceButton.onClick.RemoveListener(OnChoiceButtonClicked);
		}
		
		private void OnChoiceButtonClicked()
		{
			ChoiceButtonClicked?.Invoke(this);
		}
	}
}