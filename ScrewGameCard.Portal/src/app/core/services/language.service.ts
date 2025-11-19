import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

export type Language = 'en' | 'ar';
export type Direction = 'ltr' | 'rtl';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  currentLang = signal<Language>('en');
  direction = signal<Direction>('ltr');
  translations = signal<any>({});

  constructor(private http: HttpClient) {
    // Initialize from localStorage or default
    const savedLang = localStorage.getItem('language') as Language;
    this.setLanguage(savedLang || 'en');
  }

  async setLanguage(lang: Language) {
    this.currentLang.set(lang);
    const dir = lang === 'ar' ? 'rtl' : 'ltr';
    this.direction.set(dir);
    
    // Update document attributes
    document.documentElement.lang = lang;
    document.documentElement.dir = dir;
    
    // Persist
    localStorage.setItem('language', lang);

    // Load translations
    await this.loadTranslations(lang);
  }

  async loadTranslations(lang: Language) {
    try {
      console.log(`Loading translations for ${lang}...`);
      const data = await firstValueFrom(this.http.get<any>(`assets/i18n/${lang}.json`));
      console.log('Translations loaded:', data);
      this.translations.set(data);
    } catch (error) {
      console.error(`Could not load translations for language ${lang}`, error);
    }
  }


  toggleLanguage() {
    const newLang = this.currentLang() === 'en' ? 'ar' : 'en';
    this.setLanguage(newLang);
  }

  translate(key: string): string {
    const keys = key.split('.');
    let value = this.translations();
    for (const k of keys) {
      value = value?.[k];
    }
    return value || key;
  }
}

