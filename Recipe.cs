namespace DustlandDeliveryCooking
{
  public class Recipe
  {
    public string[] MandatoryIngredients { get; set; }
    public string[] OptionalIngredients { get; set; }

    public Recipe(string[] mandatoryIngredients, string[] optionalIngredients)

    {
      MandatoryIngredients = mandatoryIngredients;
      OptionalIngredients = optionalIngredients;
    }
  }
}
