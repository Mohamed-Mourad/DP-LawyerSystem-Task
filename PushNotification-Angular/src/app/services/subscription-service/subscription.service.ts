// services/subscription.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserSubscriptionDto } from '../../models/user-subscription-dto';

@Injectable({
  providedIn: 'root',
})
export class SubscriptionService {
  private apiUrl = 'https://localhost:5001/api/subscription';

  constructor(private http: HttpClient) {}

  subscribe(userSubscription: UserSubscriptionDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/subscribe`, userSubscription);
  }

  unsubscribe(userSubscription: UserSubscriptionDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/unsubscribe`, userSubscription);
  }
}
