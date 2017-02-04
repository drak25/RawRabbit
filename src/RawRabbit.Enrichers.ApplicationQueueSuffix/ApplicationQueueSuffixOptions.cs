using System;
using System.Collections.Generic;
using RawRabbit.Configuration.Queue;
using RawRabbit.Pipe;

namespace RawRabbit.Enrichers.ApplicationQueueSuffix
{
	public class ApplicationQueueSuffixOptions
	{
		public Func<IPipeContext, List<Action<IQueueDeclarationBuilder>>> QueueActionListFunc { get; set; }
		public Func<IPipeContext, Action<IQueueDeclarationBuilder>> QueueSuffixFunc { get; set; }
		public Func<IPipeContext, bool> ActivatedFlagFunc { get; set; }
	}
}