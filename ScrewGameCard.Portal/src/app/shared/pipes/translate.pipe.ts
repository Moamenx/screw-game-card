import { Pipe, PipeTransform, OnDestroy, effect } from '@angular/core';
import { LanguageService } from '../../core/services/language.service';
import { ChangeDetectorRef } from '@angular/core';

@Pipe({
  name: 'translate',
  standalone: true,
  pure: false // Impure to detect signal changes if needed, though effect handling is better
})
export class TranslatePipe implements PipeTransform {
  constructor(private languageService: LanguageService) {}

  transform(key: string): string {
    return this.languageService.translate(key);
  }
}
