using FluentValidation.TestHelper;
using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.DTOs.Validations;

namespace ForumTemplate.Tests.FluentValidationTests
{
    [TestClass]
    public class CommentRequestValidatorTests
    {
        private CommentRequestValidator validator;

        [TestInitialize]

        public void Initialize()
        {
            validator = new CommentRequestValidator();
        }

        [TestMethod]

        public void CommentRequest_ShouldReturn_WhenAllValid()
        {
            //Arrange
            var model = new CommentRequest
            {
                Content = new string('a', 50),
                UserId = Guid.NewGuid(),
                PostId = Guid.NewGuid()
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]

        public void CommentRequest_ShouldThrow_WhenContentEmpty()
        {
            string content = null;

            var model = new CommentRequest
            { 
              Content = content,
                UserId = Guid.NewGuid(),
                 PostId = Guid.NewGuid()
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Content);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Content is required"));
        }

        [TestMethod]

        public void CommentRequest_ShouldThrow_WhenContentInvalid()
        {
            //Arrange
            var model = new CommentRequest
            {
                Content = new string('a', 8193),
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
            "Content cannot exceed 8192 characters. Please write less than 8193 characters."));
        }
    }
}
