#region Using

using System;
using System.IO;
using NMock;

#endregion

#if NetFx35
namespace NMockTests.Matchers35
#else
#if SILVERLIGHT
namespace NMockTests.MatchersSL
#else
namespace NMockTests.Matchers
#endif
#endif
{
	public class MatcherWithDescription : Matcher
	{
		private readonly string description;

		public MatcherWithDescription(string description)
		{
			this.description = description;
		}

		public override bool Matches(object o)
		{
			throw new NotImplementedException();
		}

		public override void DescribeTo(TextWriter writer)
		{
			writer.Write(description);
		}
	}
}