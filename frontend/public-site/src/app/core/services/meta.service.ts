import {inject, Injectable} from '@angular/core';
import { Meta, Title } from '@angular/platform-browser';

export interface SeoData {
  title: string;
  description: string;
  imageUrl?: string;
  url?: string;
  type?: 'website' | 'profile' | 'article';
  jsonLd?: Record<string, unknown>;
}

@Injectable({
  providedIn: 'root'
})
export class MetaService {
  private meta = inject(Meta);
  private title = inject(Title);

  setSeo(data: SeoData) {
    this.title.setTitle(data.title);

    this.meta.updateTag({ name: 'description', content: data.description });

    this.meta.updateTag({ property: 'og:title', content: data.title });
    this.meta.updateTag({ property: 'og:description', content: data.description });
    this.meta.updateTag({ property: 'og:type', content: data.type || 'website' });
    if (data.url) {
      this.meta.updateTag({ property: 'og:url', content: data.url });
    }
    if (data.imageUrl) {
      this.meta.updateTag({ property: 'og:image', content: data.imageUrl });
    }

    this.meta.updateTag({ name: 'twitter:title', content: data.title });
    this.meta.updateTag({ name: 'twitter:description', content: data.description });
    if (data.imageUrl) {
      this.meta.updateTag({ name: 'twitter:image', content: data.imageUrl });
    }

    if (data.jsonLd) {
      const existing = document.head.querySelector('script[type="application/ld+json"]');
      if (existing) {
        existing.remove();
      }

      const script = document.createElement('script');
      script.type = 'application/ld+json';
      script.text = JSON.stringify(data.jsonLd);
      document.head.appendChild(script);
    }
  }
}
