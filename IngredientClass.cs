namespace DustlandDeliveryCooking
{
  public class IngredientClass
  {
    public string Name { get; set; }
    public string[] Ingredients { get; set; }

    public IngredientClass(string name, string[] ingredients)
    {
      Name = name;
      Ingredients = ingredients;
    }
  }
}
