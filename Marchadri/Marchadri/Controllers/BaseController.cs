using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Marchadri.Common.Helpers;
using Marchadri.Common.Models;
using Marchadri.Data.Entities.SystemRole;
using Marchadri.Filters;
using Marchadri.Helpers;
using Microsoft.AspNetCore.Mvc;
using static Marchadri.Data.Entities.SystemRole.SystemRole;


namespace Marchadri.Controllers
{
	public class BaseController : Controller
	{
		private readonly IMapper _mapper;

		public BaseController(IMapper mapper)
		{
			_mapper = mapper;
		}

		protected bool IsSuperAdministrator => SystemRoleId == GuidLibrary.GetId(ESystemRoles.SuperAdministrator);

		protected virtual Guid AccessToken => User.GetGuidClaim(TokenAuthenticationFilter.EClaimType.AccessToken);
		protected virtual Guid UserId => User.GetGuidClaim(TokenAuthenticationFilter.EClaimType.UserId);
		protected virtual string Username => User.GetStringClaim(TokenAuthenticationFilter.EClaimType.Username);
		protected virtual Guid SystemRoleId => User.GetGuidClaim(TokenAuthenticationFilter.EClaimType.SystemRole);

		[NonAction]
		protected T Map<T>(object source)
		{
			return _mapper.Map<T>(source);
		}

		[NonAction]
		protected T Map<T>(object source, Dictionary<string, object> options)
		{
			return _mapper.Map<T>(source, opt =>
			{
				foreach (var option in options)
				{
					opt.Items.Add(option.Key, option.Value);
				}
			});
		}

		[NonAction]
		protected BaseListResponseModel<TResult> GetListResponse<TResult, TEntity>(PagingRequestModel searchModel, IQueryable<TEntity> entities)
		{
			var result = new BaseListResponseModel<TResult>
			{
				TotalCount = entities.Count()
			};
			entities = PagingQuery(entities, searchModel.Page, searchModel.Limit);

			result.Data = Map<TResult[]>(entities);

			return result;
		}

		[NonAction]
		protected static IQueryable<TDbModel> PagingQuery<TDbModel>(IQueryable<TDbModel> dbQuery, int? page, int? limit)
		{
			if (page.HasValue && page > 1)
			{
				dbQuery = dbQuery.Skip((limit ?? 0) * (page.Value - 1));
			}
			if (limit.HasValue && limit > 0)
			{
				dbQuery = dbQuery.Take(limit.Value);
			}

			return dbQuery;
		}
	}
}