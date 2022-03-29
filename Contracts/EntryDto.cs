using FluentValidation;

namespace Havit.Bonusario.Contracts;

public class EntryDto
{
	public int Id { get; set; }

	public int? CreatedById { get; set; }

	public int? RecipientId { get; set; }

	public string Text { get; set; }

	public int Value { get; set; }

	public List<string> Tags { get; set; } = new();

	public DateTime? Submitted { get; set; }
	public DateTime? Created { get; set; }
	public int PeriodId { get; set; }

	public bool HasValues()
	{
		if (!String.IsNullOrWhiteSpace(Text)
			|| (Value != 0)
			|| (Submitted is not null)
			|| Tags.Any())
		{
			return true;
		}
		return false;
	}

	public class EntryDtoValidator : AbstractValidator<EntryDto>
	{
		public EntryDtoValidator()
		{
			RuleFor(e => e.RecipientId).NotNull().WithMessage("Příjemce musí být určen.");
			RuleFor(e => e.Value).InclusiveBetween(0, 100).WithMessage("Hodnota musí být v rozmezí 0 až 100.");
		}
	}
}
