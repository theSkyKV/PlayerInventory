using Project.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Views
{
	public class CellViewForInventory : MonoBehaviour
	{
		[SerializeField] 
		private Button _choiceButton;

		[SerializeField] 
		private TMP_Text _itemNameDisplay;

		[SerializeField] 
		private TMP_Text _itemAmountDisplay;

		[SerializeField] 
		private TMP_Text _priceDisplay;
		
		public event UnityAction<Cell> ChoiceButtonClicked;

		public Cell Cell { get; private set; }

		public void Init(Cell cell)
		{
			Cell = cell;
			_itemNameDisplay.text = cell.Item.Name;
			_itemAmountDisplay.text = cell.Amount.ToString();
			_priceDisplay.text = $"{(cell.Item.Price / 100).ToString()} g {(cell.Item.Price % 100).ToString()} s";
		}

		public void UpdateAmount(int value)
		{
			Cell.Amount += value;
			_itemAmountDisplay.text = Cell.Amount.ToString();
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
			ChoiceButtonClicked?.Invoke(Cell);
		}
	}
}