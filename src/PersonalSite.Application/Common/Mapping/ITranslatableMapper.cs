namespace PersonalSite.Application.Common.Mapping;

public interface ITranslatableMapper<TEntity, TDto>
{
    TDto MapToDto(TEntity entity, string languageCode);
    List<TDto> MapToDtoList(IEnumerable<TEntity> entities, string lanuageCode);
}