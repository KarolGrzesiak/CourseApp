import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { UserService } from './_services/user.service';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MembersComponent } from './members/members.component';
import { MessagesComponent } from './messages/messages.component';
import { ExamsComponent } from './exams/exams.component';
import { HomeComponent } from './home/home.component';
import { appRoutes } from './routes';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AdminComponent } from './admin/admin.component';
import { HasRoleDirective } from './_directives/hasRole.directive';
import { RegisterComponent } from './home/register/register.component';

import { BsDropdownModule, BsDatepickerModule } from 'ngx-bootstrap';
import { JwtModule } from '@auth0/angular-jwt';
import { AlertifyService } from './_services/alertify.service';
import { AuthGuard } from './_guards/auth.guard';


export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    MembersComponent,
    MessagesComponent,
    HasRoleDirective,
    ExamsComponent,
    HomeComponent,
    AdminComponent,
    RegisterComponent
  ],
  imports: [
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:5000'],
        blacklistedRoutes: ['localhost:5000/api/auth']
      }
    }),
    BrowserModule,
    HttpClientModule,
    FormsModule,
    BsDatepickerModule.forRoot(),
    BsDropdownModule.forRoot(),
    ReactiveFormsModule,
    RouterModule.forRoot(appRoutes)
  ],
  providers: [AlertifyService, AuthService, UserService, ErrorInterceptorProvider,
  AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule {}
