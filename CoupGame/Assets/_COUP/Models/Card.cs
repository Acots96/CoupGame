
namespace CoupGame.GameLogic.Cards
{
	public class Card
	{
		private CardType _type;
		public CardType Type => _type;

		public Card(CardType type)
		{
			_type = type;
		}
	}
}