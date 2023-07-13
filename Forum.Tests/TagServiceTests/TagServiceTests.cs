using ForumTemplate.DTOs.TagDTOs;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Persistence.TagRepository;
using ForumTemplate.Persistence.UserRepository;
using ForumTemplate.Services.TagService;
using ForumTemplate.Validation;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumTemplate.Tests.TagServiceTests
{
    [TestClass]
    public class TagServiceTests
    {
        private TagService sut;

        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IPostRepository> postRepositoryMock;
        private Mock<IUserAuthenticationValidator> userValidatorMock;
        private Mock<ITagRepository> tagRepositoryMock;
        private Mock<ITagsValidator> tagValidatorMock;
        private Mock<ITagMapper> tagMapperMock;

        private Guid userId;
        private Guid tagId;
        private Guid postId;

        [TestInitialize]

        public void Initialize()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            postRepositoryMock = new Mock<IPostRepository>();
            userValidatorMock = new Mock<IUserAuthenticationValidator>();
            tagRepositoryMock = new Mock<ITagRepository>();
            tagValidatorMock = new Mock<ITagsValidator>();
            tagMapperMock = new Mock<ITagMapper>();

            userId = Guid.NewGuid();
            tagId = Guid.NewGuid();
            postId = Guid.NewGuid();

            SetupUserRepositoryMock();

            SetupPostRepositoryMock();

            SetupUserValidatorMock();

            SetupTagRepositoryMock();

            SetupTagValidatorMock();

            SetupTagMapperMock();

            sut = new TagService(userRepositoryMock.Object,
                postRepositoryMock.Object,
                userValidatorMock.Object,
                tagRepositoryMock.Object,
                tagValidatorMock.Object,
                tagMapperMock.Object);
        }

        private void SetupUserRepositoryMock()
        {

        }

        private void SetupPostRepositoryMock()
        {

        }

        private void SetupUserValidatorMock()
        {
            userValidatorMock
                .Setup(x => x.ValidateUserIdMatchAuthorIdTag(It.IsAny<User>(), It.IsAny<Guid?>()));
        }

        private void SetupTagRepositoryMock()
        {
            tagRepositoryMock
                .Setup(x => x.GetByPostId(It.IsAny<Guid>()))
                .Returns(new List<Tag>());

            tagRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns(new Tag());

            tagRepositoryMock
                .Setup(x => x.DeleteByPostId(It.IsAny<Guid>()));

            tagRepositoryMock
                .Setup(x => x.Delete(It.IsAny<Guid>()))
                .Returns("Tag was successfully deleted.");
        }

        private void SetupTagValidatorMock()
        {
            tagValidatorMock
                .Setup(x => x.Validate(It.IsAny<Guid>()));
        }

        private void SetupTagMapperMock() 
        {
            tagMapperMock
                .Setup(x => x.MapToTagResponse(It.IsAny<List<Tag>>()))
                .Returns(new List<TagResponse>());

            tagMapperMock
                .Setup(x => x.MapToTagResponse(It.IsAny<Tag>()))
                .Returns(It.IsAny<TagResponse>());
        }

        //public string Delete(User loggedUser, Guid id)
        //{
        //    //Validation
        //    tagsValidator.Validate(id);

        //    var tagToDelete = tagRepository.GetById(id);

        //    var authorId = tagToDelete.UserId;

        //    //Validation
        //    userValidator.ValidateUserIdMatchAuthorIdTag(loggedUser, authorId);

        //    return this.tagRepository.Delete(id);
        //}

        [TestMethod]

        public void GetTagsByPostId_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.GetTagsByPostId(postId);

            //Assert
            tagRepositoryMock.Verify(x => x.GetByPostId(It.IsAny<Guid>()), Times.Once);

            tagMapperMock.Verify(x => x.MapToTagResponse(It.IsAny<List<Tag>>()), Times.Once);
        }

        [TestMethod]

        public void GetById_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.GetById(tagId);

            //Assert
            tagRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            tagMapperMock.Verify(x => x.MapToTagResponse(It.IsAny<Tag>()), Times.Once);
        }

        [TestMethod]

        public void DeleteByPostId_ShouldInvokeCorrectMethods()
        {
            //Act
            sut.DeleteByPostId(tagId);

            //Assert
            tagRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            tagRepositoryMock.Verify(x => x.DeleteByPostId(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]

        public void Delete_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.Delete(It.IsAny<User>(), It.IsAny<Guid>());

            //Assert
            tagValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>()), Times.Once);

            tagRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserIdMatchAuthorIdTag(It.IsAny<User>(), It.IsAny<Guid?>()), Times.Once);

            tagRepositoryMock.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Once);
        }

       

    }
}
