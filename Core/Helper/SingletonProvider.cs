using System;
using System.Collections;
using System.Reflection;

namespace Singleton
{
    public abstract class SingletonBase<T> where T: SingletonBase<T>
	{
		public static T GetInstance()
		{
			return SingletonProvider.GetInstance<T>();
		}
	}
	
	public static class SingletonProvider
	{
		private static Hashtable _selfs = new Hashtable();
		private static object _lock = new object();
		
		public static T GetInstance<T>() where T: class
		{
			var checkPrivateCtor = (typeof(T)).GetConstructor(Type.EmptyTypes);
			
			if(checkPrivateCtor != null)
			{
				throw new InvalidOperationException("Singleton means that you don't have any public constructors.");
			}
			
			lock(_lock)
			{
				if(_selfs.ContainsKey(typeof(T).GetTypeInfo().GUID))
				{
					return (T)_selfs[typeof(T).GetTypeInfo().GUID];
				}
				
				var privateCtors = (typeof(T)).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
				foreach(var privateCtor in privateCtors)
				{
					if(privateCtor.GetParameters().Length == 0)
					{
						T instance = (T)privateCtor.Invoke(new object[] { });
				
						_selfs.Add(typeof(T).GetTypeInfo().GUID, instance);
				
						return instance;
					}
				}
				
				throw new InvalidOperationException("Singleton must have a private constructor without any parameter.");
			}
		}
	}
}
