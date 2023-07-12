using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.DTOs.TagDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Mappers
{
    public class TagMapper : ITagMapper
    {
        public TagResponse MapToTagResponse(Tag tag)
        {
            return new TagResponse(
                 tag.TagId,
                 tag.Content,
                 tag.UserId.Value,
                 tag.PostId,
                 tag.CreatedAt,
                 tag.UpdatedAt);
        }

        public TagRequest MapToTagRequest(TagResponse tagResponse)
        {
            return new TagRequest
            {
                Content = tagResponse.Content,
                UserId = tagResponse.UserId,
                PostId = tagResponse.PostId
            };
        }

        public List<TagResponse> MapToTagResponse(List<Tag> tags)
        {
            var tagResponses = new List<TagResponse>();

            foreach (var tag in tags)
            {
                TagResponse tagResponse = new TagResponse(
                    tag.TagId,
                    tag.Content,
                    tag.UserId.Value,
                    tag.PostId,
                    tag.CreatedAt,
                    tag.UpdatedAt);

                tagResponses.Add(tagResponse);
            }

            return tagResponses;
        }

        public Tag MapToTag(TagRequest tagRequest)
        {
            var tag = Tag.Create(
                  tagRequest.Content,
                  tagRequest.UserId,
                  tagRequest.PostId);

            return tag;
        }
    }
}
