namespace PersonalSite.Application.Features.Common.Language.Mappers;

public class LanguageMapper : IMapper<Domain.Entities.Common.Language, LanguageDto>
{
    public LanguageDto MapToDto(Domain.Entities.Common.Language entity)
    {
        return new LanguageDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name
        };
    }

    public List<LanguageDto> MapToDtoList(IEnumerable<Domain.Entities.Common.Language> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}