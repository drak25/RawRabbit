﻿using System;

namespace RawRabbit.Common
{
	public interface INamingConvetions
	{
		Func<Type, string> ExchangeNamingConvention { get; set; }
		Func<Type, string> QueueNamingConvention { get; set; }
		Func<Type, Type, string> RpcExchangeNamingConvention { get; set; }
		Func<string> ErrorExchangeNamingConvention { get; set; }
		Func<string> ErrorQueueNamingConvention { get; set; }
		Func<string> DeadLetterExchangeNamingConvention { get; set; }
		Func<string> RetryQueueNamingConvention { get; set; }
	}

	public class NamingConvetions : INamingConvetions
	{
		public virtual Func<Type, string> ExchangeNamingConvention { get; set; }
		public virtual Func<Type, string> QueueNamingConvention { get; set; }
		public virtual Func<Type, Type, string> RpcExchangeNamingConvention { get; set; }
		public virtual Func<string> ErrorExchangeNamingConvention { get; set; }
		public virtual Func<string> ErrorQueueNamingConvention { get; set; }
		public virtual Func<string> DeadLetterExchangeNamingConvention { get; set; }
		public virtual Func<string> RetryQueueNamingConvention { get; set; }

		public NamingConvetions()
		{
			ExchangeNamingConvention = type => type?.Namespace?.ToLower() ?? string.Empty;
			RpcExchangeNamingConvention = (request, response) => request?.Namespace?.ToLower() ?? "default_rpc_exchange";
			QueueNamingConvention = type => CreateShortAfqn(type);
			ErrorQueueNamingConvention = () => "default_error_queue";
			ErrorExchangeNamingConvention = () => "default_error_exchange";
			DeadLetterExchangeNamingConvention = () => "default_dead_letter_exchange";
			RetryQueueNamingConvention = () => $"retry_{Guid.NewGuid()}";
		}

		private static string CreateShortAfqn(Type type, string path = "", string delimeter = ".")
		{
			var t = $"{path}{(string.IsNullOrEmpty(path) ? string.Empty : delimeter)}{GetNonGenericTypeName(type)}";

			if (type.IsGenericType)
			{
				t += "[";
				foreach (var argument in type.GenericTypeArguments)
				{
					t = CreateShortAfqn(argument, t, t.EndsWith("[") ? string.Empty : ",");
				}
				t += "]";
			}

			return (t.Length > 254
				? string.Concat("...", t.Substring(t.Length - 250))
				: t).ToLowerInvariant();
		}

		public static string GetNonGenericTypeName(Type type)
		{
			var name = !type.IsGenericType
				? new[] { type.Name }
				: type.Name.Split('`');

			return name[0];
		}
	}
}