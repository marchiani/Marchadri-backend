using System;

namespace Marchadri.Common.Extantions
{
	public class KeyValueException : Exception
	{
		public string Key { get; set; }
		public string Value { get; set; }

		public ErrorStatusCode StatusCode { get; set; }

		public KeyValueException()
		{
		}

		public KeyValueException(string key, string value, ErrorStatusCode statusCode = ErrorStatusCode.BadRequest)
		{
			Key = key;
			Value = value;
			StatusCode = statusCode;
		}

		public KeyValueException(string value)
		{
			Key = "Error";
			Value = value;
			StatusCode = ErrorStatusCode.BadRequest;
		}

		public enum ErrorStatusCode
		{
			BadRequest = 400,
			NotFound = 404,
			UnprocessableEntity = 422,
			Locked = 423,
			SubscriptionRestriction = 424
		}
	}
}