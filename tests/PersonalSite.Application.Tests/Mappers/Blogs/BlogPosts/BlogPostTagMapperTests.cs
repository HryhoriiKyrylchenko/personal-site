using PersonalSite.Application.Features.Blogs.Blog.Mappers;
using PersonalSite.Domain.Entities.Blog;

namespace PersonalSite.Application.Tests.Mappers.Blogs.BlogPosts;

public class BlogPostTagMapperTests
{
    private readonly BlogPostTagMapper _mapper = new();

    [Fact]
    public void MapToDto_Should_Map_All_Properties()
    {
        var entity = new BlogPostTag
        {
            Id = Guid.NewGuid(),
            Name = "Tech"
        };

        var dto = _mapper.MapToDto(entity);

        dto.Id.Should().Be(entity.Id);
        dto.Name.Should().Be(entity.Name);
    }

    [Fact]
    public void MapToDtoList_Should_Map_All_Entities()
    {
        var entities = new List<BlogPostTag>
        {
            new BlogPostTag { Id = Guid.NewGuid(), Name = "Tech" },
            new BlogPostTag { Id = Guid.NewGuid(), Name = "Gaming" }
        };

        var dtos = _mapper.MapToDtoList(entities);

        dtos.Should().HaveCount(2);
        dtos[0].Name.Should().Be("Tech");
        dtos[1].Name.Should().Be("Gaming");
    }
}
