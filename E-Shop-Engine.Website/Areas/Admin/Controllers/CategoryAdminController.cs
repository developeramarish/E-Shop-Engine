﻿using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Areas.Admin.Models;
using E_Shop_Engine.Website.Controllers;
using X.PagedList;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    [RouteArea("Admin", AreaPrefix = "Admin")]
    [RoutePrefix("Category")]
    [Route("{action}")]
    public class CategoryAdminController : PagingBaseController
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryAdminController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: Admin/Category
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            IQueryable<Category> model = _categoryRepository.GetAll();
            IPagedList<CategoryAdminViewModel> viewModel = IQueryableToPagedList<Category, CategoryAdminViewModel, int>(model, x => x.ID, pageNumber);
            return View(viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Category category = _categoryRepository.GetById(id);
            CategoryAdminViewModel model = Mapper.Map<CategoryAdminViewModel>(category);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _categoryRepository.Update(Mapper.Map<Category>(model));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Create()
        {
            CategoryAdminViewModel model = new CategoryAdminViewModel();

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            _categoryRepository.Create(Mapper.Map<Category>(model));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Category category = _categoryRepository.GetById(id);
            CategoryAdminViewModel model = Mapper.Map<CategoryAdminViewModel>(category);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                _categoryRepository.Delete(id);
            }
            catch (DbUpdateException e)
            {
                return View("_Error", new string[] { "Move products to other category." });
            }

            return RedirectToAction("Index");
        }
    }
}