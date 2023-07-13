using FluentValidation.TestHelper;
using ForumTemplate.DTOs.TagDTOs;
using ForumTemplate.DTOs.Validations;

namespace ForumTemplate.Tests.FluentValidationTests
{
    [TestClass]
    public class TagRequestValidatorTests
    {
        private TagRequestValidator validator;

        [TestInitialize]

        public void Initialize()
        {
            validator = new TagRequestValidator();
        }

        [TestMethod]

        public void TagRequest_ShouldReturn_WhenAllValid()
        {
            //Arrange
            var model = new TagRequest
            {
                Content = new string('a', 5),
                UserId = Guid.NewGuid(),
                PostId = Guid.NewGuid()
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]

        public void TagRequest_ShouldThrow_WhenContentEmpty()
        {
            string content = null;

            var model = new TagRequest
            {
                Content = content,
                UserId = Guid.NewGuid(),
                PostId = Guid.NewGuid()
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Content);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Content is required."));
        }

        [TestMethod]

        public void TagRequest_ShouldThrow_WhenContentInvalid()
        {
            //Arrange
            var model = new TagRequest
            {
                Content = new string('a', 2),
                UserId = Guid.NewGuid(),
                PostId = Guid.NewGuid()
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Content);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage ==
            "Content must be between 3 and 9 characters long. You entered 2 characters"));
        }

        [TestMethod]

        public void TagRequest_ShouldThrow_WhenContentInvalid2()
        {
            //Arrange
            var model = new TagRequest
            {
                Content = new string('a', 10),
                UserId = Guid.NewGuid(),
                PostId = Guid.NewGuid()
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Content);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage ==
            "Content must be between 3 and 9 characters long. You entered 10 characters"));
        }
    }
}
