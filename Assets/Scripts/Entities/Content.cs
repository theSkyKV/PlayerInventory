using Newtonsoft.Json;

namespace Project.Entities
{
	public class Content
	{
		[JsonProperty]
		public string ItemId { get; set; }

		[JsonProperty]
		public int Amount { get; set; }

		public Content(string itemId, int amount)
		{
			ItemId = itemId;
			Amount = amount;
		}
	}
}