  import { BrowserModule } from '@angular/platform-browser';
  import { NgModule } from '@angular/core';
  import { RouterModule, Routes } from '@angular/router';
  import { HttpClientModule } from '@angular/common/http';

  import { AppRoutingModule } from './app-routing.module';
  import { AppComponent } from './app.component';
  import { AppBarComponent } from './shared/app-bar/app-bar.component';
  import { NotificationPageComponent } from './pages/notification-page/notification-page.component';
  import { SubscriptionService } from './services/subscription-service/subscription.service';

  const routes: Routes = [
    { path: '', component: NotificationPageComponent },
  ];

  @NgModule({
    declarations: [
      AppComponent,
      AppBarComponent,
      NotificationPageComponent,
    ],
    imports: [
      BrowserModule,
      AppRoutingModule,
      RouterModule.forRoot(routes),
    ],
    providers: [
      SubscriptionService
    ],
    bootstrap: [AppComponent]
  })
  export class AppModule { }
