using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Bonusario.Web.Client
{
	public static class Routes
	{
		public const string Home = "/";
		public const string ReceivedEntries = "/received-entries";
		public const string GivenEntries = "/given-entries";
		public const string SubmissionBoard = "/submission-board";
		public const string SubmissionTable = "/submission-table";
		public const string Results = "/results";
		public const string AggregateResults = "/aggregate-results";

		public static class Administration
		{
			public const string Index = "/admin/";
		}

		public static class UserAdministration
		{
			public const string PageName = "/admin/user/page-name";
		}

		public static class Diagnostics
		{
			public const string Info = "/diag/info";
		}
	}
}
