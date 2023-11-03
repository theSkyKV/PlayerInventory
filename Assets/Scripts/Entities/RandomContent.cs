using Newtonsoft.Json;

namespace Project.Entities
{
	public class RandomContent : Content
	{
		[JsonProperty]
		public int Weight { get; set; }

		public RandomContent(string itemId, int amount, int weight) : base(itemId, amount)
		{
			Weight = weight;
		}
	}
}