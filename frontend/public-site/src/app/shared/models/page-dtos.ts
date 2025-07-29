export interface HomePageDto {
  pageData: PageDto;
  userSkills: UserSkillDto[];
  lastProject: ProjectDto | null;
}

export interface PageDto {
  id: string;  // GUID
  key: string;
  language: string;
  data: Record<string, string>;
  title: string;
  description: string;
  metaTitle: string;
  metaDescription: string;
  ogImage: string;
}

export interface CookiesPageDto {
  pageData: PageDto;
}

export interface PrivacyPageDto {
  pageData: PageDto;
}

export interface UserSkillDto {
  id: string;
  skill: SkillDto;
  proficiency: number;
}

export interface SkillDto {
  id: string;
  key: string;
  name: string;
  description: string;
  category: SkillCategoryDto;
}

export interface SkillCategoryDto {
  id: string;
  key: string;
  displayOrder: number;
  name: string;
  description: string;
}

export interface ProjectDto {
  id: string;
  slug: string;
  coverImage: string;
  demoUrl: string;
  repoUrl: string;
  title: string;
  shortDescription: string;
  descriptionSections: Record<string, string>;
  metaTitle: string;
  metaDescription: string;
  ogImage: string;
  skills: ProjectSkillDto[];
}

export interface ProjectSkillDto {
  id: string;
  projectId: string;
  skill: SkillDto;
}

export interface AboutPageDto {
  pageData: PageDto;
  userSkills: UserSkillDto[];
  learningSkills: LearningSkillDto[];
}

export interface LearningSkillDto {
  id: string;
  skill: SkillDto;
  learningStatus: string;
  displayOrder: number;
}

export interface PortfolioPageDto {
  pageData: PageDto;
  projects: ProjectDto[];
}

export interface BlogPageDto {
  pageData: PageDto;
  blogPosts: BlogPostDto[];
}

export interface BlogPostDto {
  id: string;
  slug: string;
  coverImage: string;
  isPublished: boolean;
  publishedAt: string | null; // ISO 8601 date string
  title: string;
  excerpt: string;
  content: string;
  metaTitle: string;
  metaDescription: string;
  ogImage: string;
  tags: BlogPostTagDto[];
}

export interface BlogPostTagDto {
  id: string;
  name: string;
}

export interface ContactPageDto {
  pageData: PageDto;
}
