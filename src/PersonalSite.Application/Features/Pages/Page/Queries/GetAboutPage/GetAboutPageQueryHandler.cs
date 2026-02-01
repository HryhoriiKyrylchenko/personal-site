using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Pages;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetAboutPage;

public class GetAboutPageQueryHandler : IRequestHandler<GetAboutPageQuery, Result<AboutPageDto>>
{
    private const string Key = "about";
    private readonly LanguageContext _language;
    private readonly IPageRepository _pageRepository;
    private readonly IUserSkillRepository _userSkillRepository;
    private readonly ILearningSkillRepository _learningSkillRepository;
    private readonly ILogger<GetAboutPageQueryHandler> _logger;
    private readonly ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> _pageMapper;
    private readonly ITranslatableMapper<LearningSkill, LearningSkillDto> _learningSkillMapper;
    private readonly ITranslatableMapper<UserSkill, UserSkillDto> _userSkillMapper;
    private readonly IS3UrlBuilder _urlBuilder;

    public GetAboutPageQueryHandler(
        LanguageContext language,
        IPageRepository pageRepository,
        IUserSkillRepository userSkillRepository,
        ILearningSkillRepository learningSkillRepository,
        ILogger<GetAboutPageQueryHandler> logger,
        ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> pageMapper,
        ITranslatableMapper<LearningSkill, LearningSkillDto> learningSkillMapper,
        ITranslatableMapper<UserSkill, UserSkillDto> userSkillMapper,
        IS3UrlBuilder urlBuilder)
    {
        _language = language;
        _pageRepository = pageRepository;
        _userSkillRepository = userSkillRepository;
        _learningSkillRepository = learningSkillRepository;
        _logger = logger;
        _pageMapper = pageMapper;  
        _learningSkillMapper = learningSkillMapper;
        _userSkillMapper = userSkillMapper;
        _urlBuilder = urlBuilder;
    }
    
    public async Task<Result<AboutPageDto>> Handle(GetAboutPageQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_language.LanguageCode))
            {
                return Result<AboutPageDto>.Failure("Invalid language context.");
            }
        
            var page = await _pageRepository.GetByKeyAsync(Key, cancellationToken);
            if (page == null)
            {
                _logger.LogWarning("Home page not found.");
                return Result<AboutPageDto>.Failure("Home page not found.");
            }
            var pageData = _pageMapper.MapToDto(page, _language.LanguageCode);
        
            var userSkills = await _userSkillRepository.GetAllActiveAsync(cancellationToken);
            if (userSkills.Count < 1)
            {
                _logger.LogWarning("No skills found.");     
            }
            var userSkillsData = _userSkillMapper.MapToDtoList(userSkills, _language.LanguageCode);
        
            var learningSkills = await _learningSkillRepository.GetAllOrderedAsync(cancellationToken);
            if (learningSkills.Count < 1)
            {
                _logger.LogWarning("No skills found.");     
            }
            var learningSkillsData = _learningSkillMapper.MapToDtoList(learningSkills, _language.LanguageCode);
        
            var aboutPage = new AboutPageDto
            {
                PageData = pageData,
                UserSkills = userSkillsData,
                LearningSkills = learningSkillsData
            };
        
            return Result<AboutPageDto>.Success(aboutPage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting About page data.");
            return Result<AboutPageDto>.Failure("An unexpected error occurred.");
        }
    }
}