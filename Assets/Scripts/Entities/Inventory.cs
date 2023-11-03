using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Project.Common.Enums;

namespace Project.Entities
{
	public class Inventory
	{
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public Actor Owner { get; set; }
		
		[JsonProperty]
		public List<Cell> Cells { get; set; }

		[JsonProperty]
		public int SilverAmount { get; set; }

		public Inventory(Actor owner, int silverAmount, List<Cell> cells = null)
		{
			Owner = owner;
			SilverAmount = silverAmount;
			Cells = cells ?? new List<Cell>();
		}
	}
}