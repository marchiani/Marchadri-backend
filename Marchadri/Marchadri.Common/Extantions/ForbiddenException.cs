using System;
using System.Net;

namespace Marchadri.Common.Extantions
{
	public class ForbiddenException : Exception
	{
		public EStatusCodes StatusCode { get; set; }
		public string Reason { get; set; }

		public ForbiddenException(string reason = "You do not have permission to access to this function.", EStatusCodes statusCode = EStatusCodes.Forbidden)
		{
			Reason = reason;
			StatusCode = statusCode;
		}

		public enum EStatusCodes
		{
			Forbidden = HttpStatusCode.Forbidden,
			PaymentRequired = HttpStatusCode.PaymentRequired
		}
	}
}