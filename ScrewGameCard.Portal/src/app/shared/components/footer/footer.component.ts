import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: true,
  templateUrl: './footer.component.html',
  styles: [`
    .footer {
      color: var(--text-muted);
    }
  `]
})
export class FooterComponent {}
