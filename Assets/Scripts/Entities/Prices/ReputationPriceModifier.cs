using Project.Common.Enums;

namespace Project.Entities.Prices
{
	public class ReputationPriceModifier : PriceModifier
	{
		private readonly Actor _actor;

		public ReputationPriceModifier(Actor actor, IPriceModifier wrappedEntity) : base(wrappedEntity)
		{
			_actor = actor;
		}

		public override int GetPrice()
		{
			return _actor switch
			{
				Actor.Player => (int)(WrappedEntity.GetPrice() * 1.1f),
				Actor.Trader => (int)(WrappedEntity.GetPrice() * 0.9f),
				_ => -1
			};
		}
	}
}