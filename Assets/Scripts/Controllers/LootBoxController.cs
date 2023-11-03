using Project.Entities;
using Project.Models;

namespace Project.Controllers
{
	public class LootBoxController
	{
		private readonly LootBoxModel _lootBoxModel = new();

		public LootBox Open(Cell cell)
		{
			var lootBox = _lootBoxModel.Get(cell.ItemId);
			return _lootBoxModel.Open(lootBox);
		}

		public bool TryGetReward(LootBox box)
		{
			return _lootBoxModel.TryGetReward(box);
		}
	}
}