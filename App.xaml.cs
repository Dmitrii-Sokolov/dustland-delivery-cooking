using System.Data;
using System.IO;
using System.Text;
using System.Windows;

namespace DustlandDeliveryCooking
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private const string RecipesFileName = "Recipes.txt";
    private const string ClassesFileName = "Classes.txt";

    public Recipe[] Recipes { get; private set; } = [];
    public IngredientClass[] Classes { get; private set; } = [];
    public HashSet<string> Ingredients { get; private set; } = [];

    public App()
    {
      Startup += LoadDataBase;
    }

    private void LoadDataBase(object sender, StartupEventArgs e)
    {
      LoadRecipes();
      LoadClasses();

      foreach (var recipe in Recipes)
      {
          foreach (var ingredient in recipe.OptionalIngredients)
              Ingredients.Add(ingredient);

          foreach (var ingredient in recipe.MandatoryIngredients)
              Ingredients.Add(ingredient);
      }

      MessageBox.Show($"Total unique ingredients: {Ingredients.Count}");
      MessageBox.Show(Ingredients.Aggregate(
        new StringBuilder(),
        (sb, ingredient) => sb.AppendLine(ingredient),
        sb => sb.ToString()));
    }

    private void LoadRecipes()
    {
      Recipes = LoadData(RecipesFileName, line =>
      {
        var parts = line.Split('|');
        return new Recipe(
          parts.Where(s => s[^1] != '?')
               .Select(p => p.Trim())
               .ToArray(),
          parts.Where(s => s[^1] == '?')
               .Select(p => p.Trim('?'))
               .ToArray());
      });
    }

    private void LoadClasses()
    {
      Classes = LoadData(ClassesFileName, line =>
      {
        var parts = line.Split(':');

        if (parts.Length < 2)
          throw new FormatException($"Invalid class format: {line}");

        return new IngredientClass(parts[0].Trim(), parts[1].Split(',').Select(s => s.Trim()).ToArray());
      });
    }

    private static T[] LoadData<T>(string fileName, Func<string, T> parser)
    {
      string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

      if (!File.Exists(path))
      {
        MessageBox.Show($"File '{path}' not found 😔");
        return [];
      }

      try
      {
        string[] lines = File.ReadAllLines(path, Encoding.UTF8);
        return lines
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(s => parser(s))
            .ToArray();
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Can't parse '{path}':\n{ex}");
        return [];
      }
    }
  }
}
