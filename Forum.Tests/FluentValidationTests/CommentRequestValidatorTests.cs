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
            var model = new CommentRequest(new string('a', 50), Guid.NewGuid(), Guid.NewGuid());

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]

        public void CommentRequest_ShouldThrow_WhenContentEmpty()
        {
            string content = null;

            var model = new CommentRequest(content, Guid.NewGuid(), Guid.NewGuid());

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
            var model = new CommentRequest(new string('a', 8193), Guid.NewGuid(), Guid.NewGuid());

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
