namespace Project.Entities.Prices
{
	public sealed class BasePrice : IPriceModifier
	{
		private readonly Item _item;

		public BasePrice(Cell cell)
		{
			_item = cell.Item;
		}

		public int GetPrice()
		{
			return _item.Price;
		}
	}
}