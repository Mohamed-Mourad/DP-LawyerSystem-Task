// services/notification.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Notification } from '../../models/notification';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private apiUrl = 'http://localhost:5001/api/notification';

  constructor(private http: HttpClient) {}

  broadcast(notification: Notification): Observable<any> {
    return this.http.post(`${this.apiUrl}/broadcast`, notification);
  }

  sendNotificationToUser(userId: number, notification: Notification): Observable<any> {
    return this.http.post(`${this.apiUrl}/send?userId=${userId}`, notification);
  }

  deliverPendingNotifications(userId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/deliver?userId=${userId}`, null);
  }
}
