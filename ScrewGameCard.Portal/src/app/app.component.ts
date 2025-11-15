import { Component } from '@angular/core';
import { AppLayoutComponent } from './shared/components/app-layout/app-layout.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [AppLayoutComponent],
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ScrewGameCard.Portal';
}
