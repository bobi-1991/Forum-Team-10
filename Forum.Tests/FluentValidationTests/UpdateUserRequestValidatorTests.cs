using FluentValidation.TestHelper;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.DTOs.Validations;

namespace ForumTemplate.Tests.FluentValidationTests
{
    [TestClass]
    public class UpdateUserRequestValidatorTests
    {
        private UpdateUserRequestValidator validator;

        [TestInitialize]

        public void Initialize()
        {
            validator = new UpdateUserRequestValidator();
        }

        [TestMethod]

        public void UpdateRequest_ShouldReturn_WhenAllValid()
        {
            //Arrange
            var model = new UpdateUserRequest()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
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

        public void UpdateRequest_ShouldThrow_WhenFirstNameEmpty()
        {
            var model = new UpdateUserRequest()
            {
                LastName = "Mladenov",
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

        public void UpdateRequest_ShouldThrow_WhenLastNameEmpty()
        {
            var model = new UpdateUserRequest()
            {
                FirstName = "Strahil",
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

        public void UpdateRequest_ShouldThrow_WhenPasswordEmpty()
        {
            var model = new UpdateUserRequest()
            {
                LastName = "Mladenov",
                FirstName = "Strahil",
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

        public void UpdateRequest_ShouldThrow_WhenEmailEmpty()
        {
            var model = new UpdateUserRequest()
            {
                LastName = "Mladenov",
                FirstName = "Strahil",
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

        public void UpdateRequest_ShouldThrow_WhenCountryEmpty()
        {
            var model = new UpdateUserRequest()
            {
                LastName = "Mladenov",
                FirstName = "Strahil",
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

        public void UpdateRequest_ShouldThrow_WhenFirstNameInvalid(int count)
        {
            //Arrange
            var firstName = new string('a', count);

            var model = new UpdateUserRequest()
            {
                FirstName = firstName,
                LastName = "Mladenov",
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

        [TestMethod]
        [DataRow(3)]
        [DataRow(35)]

        public void UpdateRequest_ShouldThrow_WhenLastNameInvalid(int count)
        {
            //Arrange
            var lastName = new string('a', count);

            var model = new UpdateUserRequest()
            {
                FirstName = "Strahil",
                LastName = lastName,
                Password = "Passw0rd@",
                Email = "abvbg@abv.bg",
                Country = "BG"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == $"Last Name must be between 4 and 32 characters long. You entered {count} characters"));
        }

        [TestMethod]

        public void UpdateRequest_ShouldThrow_WhenEmailInvalid()
        {
            //Arrange
            var model = new UpdateUserRequest()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Password = "Passw0rd@",
                Email = "abv.bg",
                Country = "BG"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Email is invalid. You entered abv.bg"));
        }

        [TestMethod]

        public void UpdateRequest_ShouldThrow_WhenPasswordInvalid()
        {
            //Arrange
            var model = new UpdateUserRequest()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Password = "Pass",
                Email = "abv@abv.bg",
                Country = "BG"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);

            var msg = result.Errors;

            Assert.AreEqual(2, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Password must be at least 8 characters long. You entered 4 characters"));
        }

        [TestMethod]

        public void UpdateRequest_ShouldThrow_WhenPasswordInvalid2()
        {
            //Arrange
            var model = new UpdateUserRequest()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Password = "Password123",
                Email = "abv@abv.bg",
                Country = "BG"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage ==
            "Password must contain at least one uppercase letter, one lowercase letter and one digit. You entered Password123"));
        }
    }
}
