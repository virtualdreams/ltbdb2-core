﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ltbdb.Extensions
{
	/// <summary>
	/// Custom validation for max item length in a string array.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class ArrayItemMaxLengthAttribute : ValidationAttribute
	{
		/// <summary>
		/// Max item length.
		/// </summary>
		private int MaxLength { get; set; }

		/// <summary>
		/// Initializes a new instance of ArrayItemMaxLengthAttribute.
		/// </summary>
		/// <param name="length">Max item length.</param>
		public ArrayItemMaxLengthAttribute(int length)
		{
			MaxLength = length;
		}

		/// <summary>
		/// Validate value.
		/// </summary>
		/// <param name="value">The value to validate.</param>
		/// <returns>True on success.</returns>
		public override bool IsValid(object value)
		{
			var list = value as IList<string>;
			if (list != null)
			{
				foreach (var item in list)
				{
					if (item == null)
						continue;

					if (item.Length > MaxLength)
						return false;
				}
				return true;
			}
			return false;
		}
	}

	/// <summary>
	/// Custom validation for string array max item length.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class StringArrayItemMaxLengthAttribute : ValidationAttribute
	{
		/// <summary>
		/// Max item length.
		/// </summary>
		private int MaxLength { get; set; }

		/// <summary>
		/// Initializes a new instance of ArrayItemMaxLengthAttribute.
		/// </summary>
		/// <param name="length">Max item length.</param>
		public StringArrayItemMaxLengthAttribute(int length)
		{
			MaxLength = length;
		}

		/// <summary>
		/// Validate value.
		/// </summary>
		/// <param name="value">The value to validate.</param>
		/// <returns>True on success.</returns>
		public override bool IsValid(object value)
		{
			var _tmp = (value as String);
			if (_tmp != null)
			{
				var _list = _tmp.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (var _item in _list)
				{
					if (_item.Length > MaxLength)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}