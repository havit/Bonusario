using System;
using Havit.Bonusario.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Bonusario.Entity.Tests
{
	[TestClass]
	public class BonusarioDbContextTests
	{
		[TestMethod]
		public void BonusarioDbContext_CheckModelConventions()
		{
			// Arrange
			DbContextOptions<BonusarioDbContext> options = new DbContextOptionsBuilder<BonusarioDbContext>()
				.UseInMemoryDatabase(nameof(BonusarioDbContext))
				.Options;
			BonusarioDbContext dbContext = new BonusarioDbContext(options);

			// Act
			Havit.Data.EntityFrameworkCore.ModelValidation.ModelValidator modelValidator = new Havit.Data.EntityFrameworkCore.ModelValidation.ModelValidator();
			string errors = modelValidator.Validate(dbContext);

			// Assert
			if (!String.IsNullOrEmpty(errors))
			{
				Assert.Fail(errors);
			}
		}
	}
}
