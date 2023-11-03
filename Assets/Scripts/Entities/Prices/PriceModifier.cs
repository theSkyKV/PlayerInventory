namespace Project.Entities.Prices
{
	public abstract class PriceModifier : IPriceModifier
	{
		protected readonly IPriceModifier WrappedEntity;

		protected PriceModifier(IPriceModifier wrappedEntity)
		{
			WrappedEntity = wrappedEntity;
		}

		public abstract int GetPrice();
	}
}