using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Project.Entities
{
	public enum ItemType
	{
		Weapon,
		Armor,
		Consumable,
		Pack,
		LootBox
	}

	public class Item
	{
		[JsonProperty]
		public string Id { get; set; }

		[JsonProperty]
		public string Name { get; set; }

		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public ItemType Type { get; set; }

		[JsonProperty]
		public int Price { get; set; }

		public Item(string id, string name, ItemType type, int price)
		{
			Id = id;
			Name = name;
			Type = type;
			Price = price;
		}
	}
}