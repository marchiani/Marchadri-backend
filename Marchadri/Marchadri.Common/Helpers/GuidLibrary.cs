using Marchadri.Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Marchadri.Common.Helpers
{
	public static class GuidLibrary
	{

		private static readonly TwoWayDictionary<SystemRole.ESystemRoles> SystemRolesDictionary = new TwoWayDictionary<SystemRole.ESystemRoles>(nameof(SystemRolesDictionary))
			{
				{SystemRole.ESystemRoles.SuperAdministrator, new Guid("38A7643B-EE8D-4C28-8949-ECB1B19B1DE7")},
				{SystemRole.ESystemRoles.User, new Guid("E5672782-53AF-4926-B181-38A00D893E54")}
			};

		public static TEnum GetEnumValue<TEnum>(Guid id) where TEnum : Enum
		{
			return (typeof(TEnum)) switch
			{
				Type t when t == typeof(SystemRole.ESystemRoles) =>
					(TEnum)(object)SystemRolesDictionary.GetEnumValue(id),

				_ => throw new ArgumentException(
					$"Two way dictionary for {typeof(TEnum)} is not defined in GuidsLibrary"),
			};
		}

		public static Guid GetId<TEnum>(TEnum value) where TEnum : Enum
		{
			return value switch
			{
				SystemRole.ESystemRoles systemRoleValue => SystemRolesDictionary.GetIdValue(systemRoleValue),

				_ => throw new ArgumentException(
					$"Two way dictionary for {typeof(TEnum)} is not defined in GuidsLibrary"),
			};
		}
	}

	internal class TwoWayDictionary<TEnum> : IEnumerable
	{
		private int Size { get; set; }
		private string Name { get; set; }

		private List<int> Counter { get; set; }

		private Dictionary<TEnum, Guid> EnumDictionary { get; set; }
		private Dictionary<Guid, TEnum> IdDictionary { get; set; }

		public TwoWayDictionary(string name)
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new Exception("TEnum must be of enum type");
			}

			Name = name;

			Size = 0;
			Counter = new List<int>();

			EnumDictionary = new Dictionary<TEnum, Guid>();
			IdDictionary = new Dictionary<Guid, TEnum>();
		}

		public void Add(TEnum enumValue, Guid id)
		{
			if (EnumDictionary.ContainsKey(enumValue))
			{
				throw new ArgumentException($"An item with the same enum value '{enumValue}' has already been added to {Name}");
			}
			if (IdDictionary.ContainsKey(id))
			{
				throw new ArgumentException($"An item with the same id '{id}' has already been added to {Name}");
			}

			Counter.Add(++Size);

			EnumDictionary.Add(enumValue, id);
			IdDictionary.Add(id, enumValue);
		}

		public TEnum GetEnumValue(Guid id)
		{
			if (!IdDictionary.ContainsKey(id))
			{
				throw new KeyNotFoundException($"The given id '{id}' was not present in the {Name}");
			}

			return IdDictionary[id];
		}

		public Guid GetIdValue(TEnum enumValue)
		{
			if (!EnumDictionary.ContainsKey(enumValue))
			{
				throw new KeyNotFoundException($"The given enum value '{enumValue}' was not present in the {Name}");
			}

			return EnumDictionary[enumValue];
		}

		public IEnumerator GetEnumerator()
		{
			return Counter.GetEnumerator();
		}
	}

}