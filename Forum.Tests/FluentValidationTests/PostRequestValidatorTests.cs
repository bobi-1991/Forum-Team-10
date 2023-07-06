using FluentValidation.TestHelper;
using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.DTOs.Validations;

namespace ForumTemplate.Tests.FluentValidationTests
{
    [TestClass]
    public class PostRequestValidatorTests
    {
        private PostRequestValidator validator;

        [TestInitialize]

        public void Initialize()
        {
            validator = new PostRequestValidator();
        }

        [TestMethod]

        public void PostRequest_ShouldReturn_WhenAllValid()
        {
            //Arrange
            var model = new PostRequest("TitleTitleTestTest", new string('a', 50), Guid.NewGuid());

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]

        public void PostRequest_ShouldThrow_WhenTitleEmpty()
        {
            string title = null;

            var model = new PostRequest(title, new string('a', 50), Guid.NewGuid());

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Title);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Title is required."));
        }

        [TestMethod]

        public void PostRequest_ShouldThrow_WhenContentEmpty()
        {
            string content = null;

            var model = new PostRequest(new string('a', 50), content, Guid.NewGuid());

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Content);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Content is required."));
        }

        [TestMethod]
        [DataRow(15)]
        [DataRow(65)]

        public void PostRequest_ShouldThrow_WhenTitleInvalid(int count)
        {
            //Arrange
            var title = new string('a', count);

            var model = new PostRequest(title, new string('a', 50), Guid.NewGuid());

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Title);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == 
            $"Title must be between 16 and 64 characters long. You entered {count} characters"));
        }

        [TestMethod]
        [DataRow(31)]
        [DataRow(8193)]

        public void PostRequest_ShouldThrow_WhenContentInvalid(int count)
        {
            //Arrange
            var content = new string('a', count);

            var model = new PostRequest(new string('a', 50), content, Guid.NewGuid());

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Content);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage ==
            $"Content must be between 32 and 8192 characters long. You entered {count} characters"));
        }
    }
}
