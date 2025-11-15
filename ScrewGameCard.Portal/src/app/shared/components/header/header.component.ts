import { Component, EventEmitter, Output } from '@angular/core';
import { ThemeService } from '../../../core/services/theme.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styles: [`
    .header {
      position: sticky;
      top: 0;
      z-index: 1000;
    }

    .btn {
      border: none;
      background: none;
      cursor: pointer;
      padding: var(--spacing-sm);
      border-radius: var(--border-radius);
    }

    .btn-outline-secondary {
      border: 1px solid var(--border-color);
      color: var(--text-color);
    }

    .btn-outline-secondary:hover {
      background-color: var(--surface-color);
    }
  `]
})
export class HeaderComponent {
  @Output() toggleSidebar = new EventEmitter<void>();

  constructor(public themeService: ThemeService) {}
}
