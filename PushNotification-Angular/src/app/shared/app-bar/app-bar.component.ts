import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserSubscriptionDto } from '../../models/user-subscription-dto';
import { SubscriptionService } from '../../services/subscription-service/subscription.service';

@Component({
  selector: 'app-app-bar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app-bar.component.html',
  styleUrl: './app-bar.component.css'
})
export class AppBarComponent {
  userSubscription: UserSubscriptionDto = { userId: 1, isSubscribed: false };
  isSubscribed: boolean = false;

  constructor(private subscriptionService: SubscriptionService) {}

  toggleSubscription() {
    try {
      if (this.userSubscription.isSubscribed) {
        this.subscriptionService.unsubscribe(this.userSubscription).subscribe(response => {
          console.log(response);
        });
      } else {
        this.subscriptionService.subscribe(this.userSubscription).subscribe(response => {
          console.log(response);
        });
      }
    } catch (error) {
      console.log(error);
    }
    this.isSubscribed = !this.isSubscribed;
  }
}
