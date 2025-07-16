export interface LanguageDto {
  id: string;           // GUID as string
  code: string;
  name: string;
}

export interface SocialMediaLinkDto {
  id: string;           // GUID
  platform: string;
  url: string;
  displayOrder: number;
  isActive: boolean;
}

export interface ResumeDto {
  id: string;           // GUID
  fileUrl: string;
  fileName: string;
  uploadedAt: string;   // ISO date string
  isActive: boolean;
}

export interface SiteInfoDto {
  languages: LanguageDto[];
  socialLinks: SocialMediaLinkDto[];
  resume?: ResumeDto | null;
}
