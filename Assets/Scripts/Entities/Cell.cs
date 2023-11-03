using Newtonsoft.Json;
using Project.Models;

namespace Project.Entities
{
	public class Cell : Content
	{
		[JsonIgnore]
		public Item Item { get; set; }
		
		public int? ChoiceOrder { get; set; }

		private readonly ItemModel _itemModel = new();

		[JsonConstructor]
		public Cell(string itemId, int amount) : base(itemId, amount)
		{
			Item = _itemModel.Get(itemId);
		}

		public Cell(Content content) : base(content.ItemId, content.Amount)
		{
			Item = _itemModel.Get(content.ItemId);
		}
	}
}