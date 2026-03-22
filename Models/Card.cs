namespace ScrumBoardApi.Models;

public class Card
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Assignee { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public string Status { get; set; } = "Backlog";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}