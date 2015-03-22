﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using SnowyImageCopy.Common;

namespace SnowyImageCopy.ViewModels
{
	public abstract class ViewModel : NotificationObject, IDisposable
	{
		public ViewModel()
		{ }


		#region Property name

		private Dictionary<string, string> _propertyNameMap;

		protected virtual string GetPropertyName([CallerMemberName] string callerPropertyName = null)
		{
			return ((_propertyNameMap != null) && _propertyNameMap.ContainsKey(callerPropertyName))
				? _propertyNameMap[callerPropertyName]
				: null;
		}

		protected virtual string GetPropertyName<T>(Expression<Func<T>> propertyExpression, [CallerMemberName] string callerPropertyName = null)
		{
			var calledPropertyName = PropertySupport.GetPropertyName(propertyExpression);

			if (_propertyNameMap == null)
				_propertyNameMap = new Dictionary<string, string>();

			_propertyNameMap.Add(callerPropertyName, calledPropertyName);

			return calledPropertyName;
		}

		#endregion


		#region Dispose

		public CompositeDisposable Subscription
		{
			get { return _subscription ?? (_subscription = new CompositeDisposable()); }
		}
		private CompositeDisposable _subscription;

		bool _disposed = false;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				if (_subscription != null)
					_subscription.Dispose();
			}

			_disposed = true;
		}

		#endregion
	}
}