import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BroadcastMessageFormComponent } from '../../components/broadcast-message-form/broadcast-message-form.component';
import { UserMessageFormComponent } from '../../components/user-message-form/user-message-form.component';
import { NotificationPopupComponent } from '../../components/notification-popup/notification-popup.component';
import { SignalrService } from '../../services/signalr-service/signalr.service';

@Component({
  selector: 'app-notification-page',
  templateUrl: './notification-page.component.html',
  styleUrls: ['./notification-page.component.css']
})
export class NotificationPageComponent {
  constructor(private signalrService: SignalrService, public dialog: MatDialog) {}

  ngOnInit(): void {
    this.signalrService.startConnection();
    this.signalrService.addBroadCastListener();

    this.signalrService.notificationReceived.subscribe(notification => {
      this.openNotificationPopup(notification.sender, notification.message);
    });
  }

  openNotificationPopup(sender: string, message: string): void {
    this.dialog.open(NotificationPopupComponent, {
      data: { sender, message }
    });
  }
}
