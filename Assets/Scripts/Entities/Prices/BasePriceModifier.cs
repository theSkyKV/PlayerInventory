using Project.Common.Enums;

namespace Project.Entities.Prices
{
	public class BasePriceModifier : PriceModifier
	{
		private readonly Actor _actor;

		public BasePriceModifier(Actor actor, IPriceModifier wrappedEntity) : base(wrappedEntity)
		{
			_actor = actor;
		}

		public override int GetPrice()
		{
			return _actor switch
			{
				Actor.Player => (int)(WrappedEntity.GetPrice() * 0.8f),
				Actor.Trader => (int)(WrappedEntity.GetPrice() * 1.2f),
				_ => -1
			};
		}
	}
}