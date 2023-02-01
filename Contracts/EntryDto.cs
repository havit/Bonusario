using FluentValidation;
using Havit.Bonusario.Contracts.ModelMetadata;
using Havit.Bonusario.Primitives;

namespace Havit.Bonusario.Contracts;

public record EntryDto
{
	public const EntryVisibility DefaultVisibility = EntryVisibility.RecipientOnlyWithAuthorIdentity;

	public int Id { get; set; }

	public int? CreatedById { get; set; }

	public int? RecipientId { get; set; }

	public string Text { get; set; }

	public int Value { get; set; }

	public List<string> Tags { get; set; } = new();

	public DateTime? Submitted { get; set; }
	public DateTime? Created { get; set; }
	public int PeriodId { get; set; }

	public EntryVisibility Visibility { get; set; } = DefaultVisibility;

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
			RuleFor(e => e.Text).MaximumLength(EntryMetadata.TextMaxLength);
		}
	}
}
