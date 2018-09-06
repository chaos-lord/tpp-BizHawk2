﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizHawk.Client.Common.Api.Public
{
	public abstract class ApiProvider
	{
		public abstract IEnumerable<ApiCommand> Commands { get; }

		private class ApiMissingError : ApiError
		{
			public ApiMissingError(string message = null) : base(message) { }
		}

		protected T ParseRequired<T>(IEnumerable<string> args, int index, Func<string, T> process, string name, string invalidError = null)
		{
			invalidError = invalidError ?? $"Provided {name} is invalid";
			if (args.Count() <= index)
			{
				throw new ApiMissingError($"Parameter {name} is missing");
			}
			try
			{
				return process(args.ElementAt(index));
			}
			catch (ApiError)
			{
				throw;
			}
			catch
			{
				throw new ApiError(invalidError);
			}
		}

		protected T? ParseOptional<T>(IEnumerable<string> args, int index, Func<string, T> process, string name, string invalidError = null) where T: struct
		{
			try
			{
				return ParseRequired(args, index, process, name, invalidError);
			}
			catch (ApiMissingError)
			{
				return null;
			}
		}
	}
}
