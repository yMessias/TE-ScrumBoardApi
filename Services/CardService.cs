using ScrumBoardApi.Models;

namespace ScrumBoardApi.Services;

public class CardService
{
    private readonly List<Card> _cards = new();

    public CardService()
    {
        SeedData();
    }

    private void SeedData()
    {
        _cards.AddRange(new List<Card>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Configurar repositório Git",
                Description = "Criar o repositório no GitHub e configurar o .gitignore",
                Assignee = "Maria",
                Priority = "High",
                Status = "Done",
                CreatedAt = DateTime.Now.AddDays(-5),
                UpdatedAt = DateTime.Now.AddDays(-4)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Criar modelo de dados",
                Description = "Definir as classes e propriedades do modelo Card",
                Assignee = "João",
                Priority = "High",
                Status = "Doing",
                CreatedAt = DateTime.Now.AddDays(-3),
                UpdatedAt = DateTime.Now.AddDays(-1)
            }
        });
    }

    public List<Card> GetAll() => _cards;

    public Card? GetById(Guid id) => _cards.FirstOrDefault(c => c.Id == id);

    public List<Card> GetByStatus(string status) =>
        _cards.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();

    public Card Add(Card card)
    {
        card.Id = Guid.NewGuid();
        card.CreatedAt = DateTime.Now;
        card.UpdatedAt = DateTime.Now;
        _cards.Add(card);
        return card;
    }

    public Card? Update(Guid id, Card updated)
    {
        var card = _cards.FirstOrDefault(c => c.Id == id);
        if (card is null) return null;

        card.Title = updated.Title;
        card.Description = updated.Description;
        card.Assignee = updated.Assignee;
        card.Priority = updated.Priority;
        card.Status = updated.Status;
        card.UpdatedAt = DateTime.Now;

        return card;
    }

    public Card? MoveCard(Guid id, string newStatus)
    {
        var card = _cards.FirstOrDefault(c => c.Id == id);
        if (card is null) return null;

        card.Status = newStatus;
        card.UpdatedAt = DateTime.Now;

        return card;
    }

    public bool Delete(Guid id)
    {
        var card = _cards.FirstOrDefault(c => c.Id == id);
        if (card is null) return false;

        _cards.Remove(card);
        return true;
    }
}