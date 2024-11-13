import { Component } from '@angular/core';
import { BroadcastMessageFormComponent } from '../../components/broadcast-message-form/broadcast-message-form.component';
import { UserMessageFormComponent } from '../../components/user-message-form/user-message-form.component';

@Component({
  selector: 'app-notification-page',
  standalone: true,
  imports: [BroadcastMessageFormComponent, UserMessageFormComponent],
  templateUrl: './notification-page.component.html',
  styleUrl: './notification-page.component.css'
})
export class NotificationPageComponent {

}
