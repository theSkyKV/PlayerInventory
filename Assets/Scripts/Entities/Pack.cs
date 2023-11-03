using System.Collections.Generic;
using Newtonsoft.Json;

namespace Project.Entities
{
	public class Pack : Item
	{
		[JsonProperty]
		public List<Content> Content { get; set; }

		public Pack(string id, string name, int price, List<Content> content) : base(id, name, ItemType.Pack, price)
		{
			Content = content;
		}
	}
}