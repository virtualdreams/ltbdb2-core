using System;
using System.Collections.Generic;
using AutoMapper;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ltbdb.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Policy = "AdministratorOnly")]
	public class AccountController: Controller
	{
		private readonly ILogger<AccountController> Log;

		private readonly UserService Account;

		public AccountController(UserService authentication, ILogger<AccountController> logger)
		{
			Account = authentication;
			Log = logger;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var _users = Account.Get();

			var users = Mapper.Map<IEnumerable<UserModel>>(_users);

			var view = new UserViewContainer
			{
				Users = users
			};

			return View(view);
		}

		[HttpPost]
		public IActionResult Create(UserModel model)
		{
			if(!ModelState.IsValid)
			{
				return RedirectToAction("index");
			}

			if(model.Password.Equals(model.PasswordRepeat))
			{
				Account.CreateUser(model.Username, model.Password, model.Role);
			}

			return RedirectToAction("index");
		}
	}
}