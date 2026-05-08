using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Icon { get; private set; } = string.Empty;
    public string Color { get; private set; } = "#000000";
    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;

    private Category() { }

    public static Category Create(Guid userId, string name, string icon = "", string color = "#000000")
    {
        return new Category
        {
            UserId = userId,
            Name = name,
            Icon = icon,
            Color = color
        };
    }

    public void Update(string name, string icon, string color)
    {
        Name = name;
        Icon = icon;
        Color = color;
        SetUpdatedAt();
    }
}
