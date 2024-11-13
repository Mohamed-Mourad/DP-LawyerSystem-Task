import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs/internal/Subject';
import { NotificationService } from '../notification-service/notification.service';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection!:  signalR.HubConnection;
  public notificationReceived = new Subject<{ sender: string, message: string }>();

  constructor() { }

  public startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/notificationHub')
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  public addSubscriptionListener(): void {
    this.hubConnection.on('ReceiveSubscriptionUpdate', (userId: string, isSubscribed: boolean) => {
      console.log(`User ${userId} subscription status updated: ${isSubscribed}`);
    });
  }

  public addBroadCastListener(): void {
    this.hubConnection.on('ReceiveNotification', (sender: string, message: string) => {
      this.notificationReceived.next({ sender, message });
    });
  }
}
