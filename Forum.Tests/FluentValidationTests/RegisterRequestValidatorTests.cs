using FluentValidation.TestHelper;
using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.DTOs.Validations;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Services.CommentService;
using ForumTemplate.Services.PostService;
using ForumTemplate.Validation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;

namespace ForumTemplate.Tests.FluentValidationTests
{
    [TestClass]
    public class RegisterRequestValidatorTests
    {
        private RegisterRequestValidator validator;

        [TestInitialize]

        public void Initialize()
        {
            validator = new RegisterRequestValidator();
        }

        [TestMethod]

        public void RegisterRequest_ShouldReturn_WhenAllValid()
        {
            //Arrange
            var model = new RegisterUserRequestModel()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Username = "Mladenov123",
                Password = "Passw0rd@",
                Email = "Abv@abv.bg",
                Country = "Bulgaria"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]

        public void RegisterRequest_ShouldThrow_WhenFirstNameEmpty()
        {
            var model = new RegisterUserRequestModel()
            {
                LastName = "Mladenov",
                Username = "Mladenov123",
                Password = "Passw0rd@",
                Email = "Abv@abv.bg",
                Country = "Bulgaria"
            };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.FirstName);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "First Name is required."));
        }

        [TestMethod]

        public void RegisterRequest_ShouldThrow_WhenLastNameEmpty()
        {
            var model = new RegisterUserRequestModel()
            {
                FirstName = "Strahil",
                Username = "Mladenov123",
                Password = "Passw0rd@",
                Email = "Abv@abv.bg",
                Country = "Bulgaria"
            };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.LastName);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Last Name is required."));
        }

        [TestMethod]

        public void RegisterRequest_ShouldThrow_WhenUsernameEmpty()
        {
            var model = new RegisterUserRequestModel()
            {
                LastName = "Mladenov",
                FirstName = "Strahil",
                Password = "Passw0rd@",
                Email = "Abv@abv.bg",
                Country = "Bulgaria"
            };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Username);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Username is required."));
        }

        [TestMethod]

        public void RegisterRequest_ShouldThrow_WhenPasswordEmpty()
        {
            var model = new RegisterUserRequestModel()
            {
                LastName = "Mladenov",
                FirstName = "Strahil",
                Username = "Mladenov123",
                Email = "Abv@abv.bg",
                Country = "Bulgaria"
            };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Password);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Password is required."));
        }

        [TestMethod]

        public void RegisterRequest_ShouldThrow_WhenEmailEmpty()
        {
            var model = new RegisterUserRequestModel()
            {
                LastName = "Mladenov",
                FirstName = "Strahil",
                Username = "Mladenov123",
                Password = "Passw0rd@",
                Country = "Bulgaria"
            };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Email);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Email is required."));
        }

        [TestMethod]

        public void RegisterRequest_ShouldThrow_WhenCountryEmpty()
        {
            var model = new RegisterUserRequestModel()
            {
                LastName = "Mladenov",
                FirstName = "Strahil",
                Username = "Mladenov123",
                Password = "Passw0rd@",
                Email = "abvbg@abv.bg"
            };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Country);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Country is required."));
        }

        [TestMethod]
        [DataRow(3)]
        [DataRow(35)]

        public void RegisterRequest_ShouldThrow_WhenFirstNameInvalid(int count)
        {
            //Arrange
            var firstName = new string('a', count);

            var model = new RegisterUserRequestModel()
            {
                FirstName = firstName,
                LastName = "Mladenov",
                Username = "Mladenov123",
                Password = "Passw0rd@",
                Email = "abvbg@abv.bg",
                Country = "BG"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == $"First Name must be between 4 and 32 characters long. You entered {count} characters"));
        }


    }
}
