using FluentValidation.TestHelper;
using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.Validations;

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

        [TestMethod]
        [DataRow(3)]
        [DataRow(35)]

        public void RegisterRequest_ShouldThrow_WhenLastNameInvalid(int count)
        {
            //Arrange
            var lastName = new string('a', count);

            var model = new RegisterUserRequestModel()
            {
                FirstName = "Strahil",
                LastName = lastName,
                Username = "Mladenov123",
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
        [DataRow(3)]
        [DataRow(35)]

        public void RegisterRequest_ShouldThrow_WhenUsernameInvalid(int count)
        {
            //Arrange
            var username = new string('a', count);

            var model = new RegisterUserRequestModel()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Username = username,
                Password = "Passw0rd@",
                Email = "abvbg@abv.bg",
                Country = "BG"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Username);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == $"Username must be between 4 and 32 characters long. You entered {count} characters"));
        }

        [TestMethod]

        public void RegisterRequest_ShouldThrow_WhenEmailInvalid()
        {
            //Arrange
            var model = new RegisterUserRequestModel()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Username = "Mladenov123",
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

        public void RegisterRequest_ShouldThrow_WhenPasswordInvalid()
        {
            //Arrange
            var model = new RegisterUserRequestModel()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Username = "Mladenov123",
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

        public void RegisterRequest_ShouldThrow_WhenPasswordInvalid2()
        {
            //Arrange
            var model = new RegisterUserRequestModel()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Username = "Mladenov123",
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
