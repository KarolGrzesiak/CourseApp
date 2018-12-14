import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { AlertifyService } from './_services/alertify.service';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { UserService } from './_services/user.service';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';


import { JwtModule } from '@auth0/angular-jwt';
import { MembersComponent } from './members/members.component';
import { MessagesComponent } from './messages/messages.component';
import { ExamsComponent } from './exams/exams.component';
import { HomeComponent } from './home/home.component';
import { appRoutes } from './routes';


export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [AppComponent, NavComponent, MembersComponent, MessagesComponent, ExamsComponent,
  HomeComponent],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:5000'],
        blacklistedRoutes: ['localhost:5000/api/auth']
      }
    })
  ],
  providers: [AlertifyService, AuthService, UserService],
  bootstrap: [AppComponent]
})
export class AppModule {}
