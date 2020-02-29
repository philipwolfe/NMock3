#region Using

using System;
using System.Globalization;
using System.Reflection;

#endregion

namespace NMockTests.Monitoring
{
#if !SILVERLIGHT
	public class MethodInfoStub
		: MethodInfo
	{
		private readonly string name;
		private readonly ParameterInfo[] parameters;
		public Type StubReturnType = typeof (object);

		public MethodInfoStub(string name, params ParameterInfo[] parameters)
		{
			this.name = name;
			this.parameters = parameters;
		}

		public override string Name
		{
			get
			{
				return name;
			}
		}

		public override Type DeclaringType
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override Type ReturnType
		{
			get
			{
				return StubReturnType;
			}
		}

		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override MethodAttributes Attributes
		{
			get
			{
				return MethodAttributes.ReuseSlot;
			}
		}

		public override Type ReflectedType
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override ParameterInfo[] GetParameters()
		{
			return parameters;
		}

		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			throw new NotImplementedException();
		}

		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override MethodInfo GetBaseDefinition()
		{
			throw new NotImplementedException();
		}

		public override Type[] GetGenericArguments()
		{
			return new Type[0];
		}
	}
#endif
}