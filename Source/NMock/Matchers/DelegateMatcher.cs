using System;

namespace NMock.Matchers
{
	/// <summary>
	/// Represents a delegate in an expectation that can be matched
	/// </summary>
	public class DelegateMatcher : Matcher
	{
		private readonly MulticastDelegate _delegate;

		/// <summary>
		/// Initializes an instance of this class with a <see cref="MulticastDelegate"/> to match.
		/// </summary>
		/// <param name="delegate"></param>
		public DelegateMatcher(MulticastDelegate @delegate)
			: base("<" + @delegate.Method.Name + "[" + @delegate + "]>")
		{
			_delegate = @delegate;
		}

		/// <summary>
		/// Determines if this delegate matches the specified parameter
		/// </summary>
		/// <param name="o">The delegate to match</param>
		/// <returns>true if the delegates match, false if the object is null or does not match</returns>
		public override bool Matches(object o)
		{
			return o != null && _delegate.Equals(o);
		}
	}
}
