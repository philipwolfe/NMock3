using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NMock.Proxy;
using NMock.Proxy.Castle;

namespace NMock
{
	public class Fake<T>
	{
		private readonly object _proxy;

		public Fake()
		{
			var factory = new CastleMockObjectFactory();
			var compositeType = new CompositeType(typeof (T));
			var type = factory.GetProxyType(compositeType);

			_proxy = Activator.CreateInstance(type);
		}

		public T MockObject
		{
			get { return (T) _proxy; }
		}

		public void PropertyIs(Action<T> action)
		{
			
		}

		public IIs<TProperty> Property<TProperty>(Expression<Func<T, TProperty>> property)
		{
			return null;
		}

	}

	public interface IIs<TProperty>
	{
		void Is(TProperty property);
	}
}
