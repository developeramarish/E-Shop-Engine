﻿using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Areas.Admin.Models;
using E_Shop_Engine.Website.Controllers;
using E_Shop_Engine.Website.CustomFilters;
using NLog;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    [RouteArea("Admin", AreaPrefix = "Admin")]
    [RoutePrefix("Settings")]
    [Route("{action}")]
    [ReturnUrl]
    public class SettingsAdminController : BaseController
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsAdminController(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
            logger = LogManager.GetCurrentClassLogger();
        }

        public ActionResult Edit()
        {
            Settings model = _settingsRepository.Get();
            SettingsAdminViewModel viewModel = Mapper.Map<SettingsAdminViewModel>(model);
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(SettingsAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _settingsRepository.Update(Mapper.Map<Settings>(model));
            return Redirect("/Admin/Order/Index");
        }
    }
}