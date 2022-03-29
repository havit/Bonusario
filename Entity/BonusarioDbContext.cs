using Havit.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Havit.Bonusario.Entity;

public class BonusarioDbContext : Havit.Data.EntityFrameworkCore.DbContext
{
	/// <summary>
	/// Konstruktor.
	/// Pro použití v unit testech, jiné použití nemá.
	/// </summary>
	internal BonusarioDbContext()
	{
		// NOOP
	}

	/// <summary>
	/// Konstruktor.
	/// </summary>
	public BonusarioDbContext(DbContextOptions options) : base(options)
	{
		// NOOP
	}

	/// <inheritdoc />
	protected override void CustomizeModelCreating(ModelBuilder modelBuilder)
	{
		base.CustomizeModelCreating(modelBuilder);

		modelBuilder.HasSequence<int>("ContactSequence");

		modelBuilder.RegisterModelFromAssembly(typeof(Havit.Bonusario.Model.Localizations.Language).Assembly);
		modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
	}
}
