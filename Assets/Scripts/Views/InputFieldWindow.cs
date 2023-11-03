using Project.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Views
{
	public class InputFieldWindow : MonoBehaviour
	{
		[SerializeField] 
		private TMP_InputField _inputField;

		[SerializeField] 
		private Button _okButton;

		[SerializeField] 
		private Button _cancelButton;

		private Cell _cell;
		private InventoryWindow _caller;

		public event UnityAction<Cell, string, InventoryWindow> OkButtonClicked;
		public event UnityAction CancelButtonClicked;

		public void Init(Cell cell, InventoryWindow caller)
		{
			_cell = cell;
			_caller = caller;
		}

		private void OnEnable()
		{
			_okButton.onClick.AddListener(OnOkButtonClicked);
			_cancelButton.onClick.AddListener(OnCancelButtonClicked);
		}

		private void OnDisable()
		{
			_okButton.onClick.RemoveListener(OnOkButtonClicked);
			_cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
		}

		private void OnOkButtonClicked()
		{
			OkButtonClicked?.Invoke(_cell, _inputField.text, _caller);
			ResetInput();
		}
		
		private void OnCancelButtonClicked()
		{
			CancelButtonClicked?.Invoke();
			ResetInput();
		}

		private void ResetInput()
		{
			_inputField.text = string.Empty;
		}
	}
}