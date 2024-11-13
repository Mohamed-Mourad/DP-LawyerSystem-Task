import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { NotificationService } from '../../services/notification-service/notification.service';
import { Notification } from '../../models/notification';

@Component({
  selector: 'app-broadcast-message-form',
  templateUrl: './broadcast-message-form.component.html',
  styleUrls: ['./broadcast-message-form.component.css']
})
export class BroadcastMessageFormComponent {
  broadcastForm: FormGroup;
  isSubmitting = false;

  constructor(private fb: FormBuilder, private notificationService: NotificationService) {
    this.broadcastForm = this.fb.group({
      message: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.broadcastForm.valid) {
      this.isSubmitting = true;
      const notification: Notification = {
        userId: 1,
        message: this.broadcastForm.value.message,
      };

      this.notificationService.broadcast(notification).subscribe({
        next: (response) => {
          console.log('Broadcast successful:', response);
          this.broadcastForm.reset();
          this.isSubmitting = false;
          alert('Message broadcasted successfully.');
        },
        error: (error) => {
          console.error('Error broadcasting message:', error);
          this.isSubmitting = false;
        }
      });
    }
  }
}
