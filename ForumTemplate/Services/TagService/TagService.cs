using ForumTemplate.DTOs.TagDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Persistence.TagRepository;
using ForumTemplate.Persistence.UserRepository;
using ForumTemplate.Validation;

namespace ForumTemplate.Services.TagService
{
    public class TagService : ITagService
    {
        private readonly IUserRepository userRepositoty;
        private readonly IPostRepository postRepository;
        private readonly IUserAuthenticationValidator userValidator;
        private readonly ITagRepository tagRepository;
        private readonly ITagsValidator tagsValidator;
        private readonly ITagMapper tagsMapper;

        public TagService(IUserRepository userRepositoty, IPostRepository postRepository,
            IUserAuthenticationValidator userValidator, ITagRepository tagRepository, ITagsValidator tagsValidator,
            ITagMapper tagsMapper)
        {
            this.userRepositoty = userRepositoty;
            this.postRepository = postRepository;
            this.userValidator = userValidator;
            this.tagRepository = tagRepository;
            this.tagsValidator = tagsValidator;
            this.tagsMapper = tagsMapper;
        }

        public List<TagResponse> GetTagsByPostId(Guid postId)
        {
            tagsValidator.GetTagsByPostID(postId);

            var tagsById = this.tagRepository.GetByPostId(postId);

            return this.tagsMapper.MapToTagResponse(tagsById);
        }

        public TagResponse GetById(Guid id)
        {
            //Validation
            this.tagsValidator.Validate(id);

            var tag = tagRepository.GetById(id);

            return this.tagsMapper.MapToTagResponse(tag);
        }

        public TagResponse Create(User loggedUser, TagRequest tagRequest)
        {
            //Validation
            this.userValidator.ValidateUserIsNotBannedTagCreate(loggedUser);
            this.tagsValidator.Validate(tagRequest);

            var authorOfPost = userRepositoty.GetById(tagRequest.UserId);

            if (authorOfPost == null)
            {
                throw new EntityNotFoundException($"User with ID: {tagRequest.UserId} not found.");
            }

            var postOfTag = postRepository.GetById(tagRequest.PostId);

            if (postOfTag == null)
            {
                throw new EntityNotFoundException($"Post with ID: {tagRequest.PostId} not found.");
            }

            if(postOfTag.UserId != tagRequest.UserId) 
            {
                throw new EntityNotFoundException($"You Cannot Write Tag on someone else's post");
            }

            var tagsOfPost = postOfTag.Tags;
            if (tagsOfPost.Any(t => t.Content.Equals(tagRequest.Content)))
            {
                throw new EntityNotFoundException("You cannot apply the same Tag Twice");
            }

            var tag = this.tagsMapper.MapToTag(tagRequest);
            var createdTag = tagRepository.Create(tag);

            return this.tagsMapper.MapToTagResponse(createdTag);
        }

        public string Delete(User loggedUser, Guid id)
        {
            //Validation
            tagsValidator.Validate(id);

            var tagToDelete = tagRepository.GetById(id);

            var authorId = tagToDelete.UserId;

            //Validation
            userValidator.ValidateUserIdMatchAuthorIdTag(loggedUser, authorId);

            return this.tagRepository.Delete(id);
        }

        public void DeleteByPostId(Guid tagId)
        {
            var tagToDelete = tagRepository.GetById(tagId);

            this.tagRepository.DeleteByPostId(tagId);
        }

    }
}
