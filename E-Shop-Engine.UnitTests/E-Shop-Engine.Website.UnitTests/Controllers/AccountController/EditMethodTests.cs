﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Services;
using E_Shop_Engine.Website.Models;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.AccountController
{
    public class EditMethodTests : AccountControllerBaseTest<UserEditViewModel>
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = new UserEditViewModel();
        }

        [Test(Description = "HTTPGET")]
        public void Edit_WhenCalled_ReturnsViewWithForm()
        {
            _mapper.Setup(m => m.Map<UserEditViewModel>(It.IsAny<AppUser>())).Returns(_model);
            SetupFindById(_user);

            ActionResult result = _controller.Edit();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(_model, (result as ViewResult).Model);
        }

        [Test(Description = "HTTPGET")]
        public void Edit_WhenCalled_FindByIdMethodCall()
        {
            _mapper.Setup(m => m.Map<UserEditViewModel>(It.IsAny<AppUser>())).Returns(_model);
            SetupFindById(_user);

            ActionResult result = _controller.Edit();

            _userManager.Verify(um => um.FindById(It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "HTTPGET")]
        public void Edit_WhenCalledAndUserNotFound_RedirectToAction()
        {
            SetupFindById();

            ActionResult result = _controller.Edit();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual("Index", (result as RedirectToRouteResult).RouteValues["action"]);
        }

        [Test(Description = "HTTPPOST")]
        [TestCase(null, null, null, null)]
        [TestCase("", "", "", "")]
        [TestCase(" ", " ", " ", " ")]
        [TestCase("a", "b", "c", "email@email.com")]
        [TestCase("a", "b", "123", "email")]
        [TestCase(" ", " ", "123", "email@email.com")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
    "b", "123", "email@email.com")]
        public void Edit_WhenModelStateNotValid_ValidationFails(string name, string surname, string phone, string email)
        {
            _model = new UserEditViewModel
            {
                Name = name,
                Surname = surname,
                PhoneNumber = phone,
                Email = email
            };

            IsModelStateValidationWorks(_model);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhnModelStateHasError_ReturnsViewWithModelError()
        {
            AddModelStateError("test");

            ActionResult result = await _controller.Edit(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertReturnsViewWithModelError(result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenValidModelPassed_RedirectToAction()
        {
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.Edit(_model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual("Index", (result as RedirectToRouteResult).RouteValues["action"]);
        }

        protected override void SetupMockedWhenValidModelPassed()
        {
            SetupFindById(_user);
            _userManager.Setup(um => um.UserValidator.ValidateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenValidModelPassed_UserValidationMethodCall()
        {
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.Edit(_model);

            _userManager.Verify(um => um.UserValidator.ValidateAsync(It.IsAny<AppUser>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenValidModelPassed_UpdateMethodCall()
        {
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.Edit(_model);

            _userManager.Verify(um => um.UpdateAsync(It.IsAny<AppUser>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenValidModelPassed_UserIsUpdated()
        {
            _model = new UserEditViewModel
            {
                Email = "email",
                Name = "name",
                PhoneNumber = "123",
                Surname = "surname"
            };
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.Edit(_model);

            AssertUserIsUpdated(_model);
        }

        private void AssertUserIsUpdated(UserEditViewModel model)
        {
            Assert.AreEqual(_user.Email, model.Email);
            Assert.AreEqual(_user.Name, model.Name);
            Assert.AreEqual(_user.PhoneNumber, model.PhoneNumber);
            Assert.AreEqual(_user.Surname, model.Surname);
            Assert.AreEqual(_user.UserName, model.Email);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenUserNotFound_ReturnsViewWithModelError()
        {
            SetupFindById();

            ActionResult result = await _controller.Edit(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage(ErrorMessage.NullUser);

            AssertReturnsViewWithModelError(result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenUserValidationFail_ReturnsViewWithModelError()
        {
            SetupFindById(_user);
            _userManager.Setup(um => um.UserValidator.ValidateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.Edit(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertReturnsViewWithModelError(result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenUpdatingUserFails_ReturnsViewWithModelError()
        {
            SetupFindById(_user);
            _userManager.Setup(um => um.UserValidator.ValidateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.Edit(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertReturnsViewWithModelError(result, errors);
        }
    }
}
