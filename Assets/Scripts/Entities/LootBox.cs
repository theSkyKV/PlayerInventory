using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Project.Common.Structs;

namespace Project.Entities
{
	public enum BoxType
	{
		First,
		Second
	}
	
	public class LootBox : Item
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public List<ValueWeight> AmountToDrop { get; set; }
		
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public BoxType BoxType { get; set; }
		
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int? BasePriceForRandomContent { get; set; }
		
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? PriceFactor { get; set; }
		
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public List<RandomContent> Content { get; set; }
		
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public List<Cell> ItemsToGet { get; set; }

		public LootBox(string id, string name, int price, BoxType boxType, List<RandomContent> content, 
			List<ValueWeight> amountToDrop, int? basePriceForRandomContent = null, float? priceFactor = null)
			: base(id, name, ItemType.LootBox, price)
		{
			AmountToDrop = amountToDrop;
			BoxType = boxType;
			Content = content;
			ItemsToGet = null;
			BasePriceForRandomContent = basePriceForRandomContent;
			PriceFactor = priceFactor;
		}
	}
}