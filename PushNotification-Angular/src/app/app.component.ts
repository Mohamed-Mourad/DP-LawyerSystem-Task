import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { AppBarComponent } from './shared/app-bar/app-bar.component';
import { NotificationPageComponent } from './pages/notification-page/notification-page.component';
import { SubscriptionService } from './services/subscription-service/subscription.service';

const routes = [
  { path: '', component: NotificationPageComponent },
];

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AppBarComponent, NotificationPageComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'PushNotification-Angular';
  constructor(private subscriptionService: SubscriptionService) {}
}
