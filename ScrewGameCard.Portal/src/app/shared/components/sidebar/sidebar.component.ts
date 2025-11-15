import { Component, Input } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
  styles: [`
    .sidebar {
      width: 250px;
      height: 100%;
      transition: width 0.3s ease;
      overflow: hidden;
    }

    .sidebar.collapsed {
      width: 60px;
    }

    .nav-link {
      display: flex;
      align-items: center;
      padding: var(--spacing-md);
      color: var(--text-color);
      text-decoration: none;
      border-radius: var(--border-radius);
      margin-bottom: var(--spacing-xs);
    }

    .nav-link:hover {
      background-color: var(--background-color);
    }

    .nav-link.active {
      background-color: var(--primary-color);
      color: white;
    }

    .nav-link span:first-child {
      margin-right: var(--spacing-sm);
    }

    .sidebar.collapsed .nav-link span:last-child {
      display: none;
    }
  `]
})
export class SidebarComponent {
  @Input() collapsed = false;
}
