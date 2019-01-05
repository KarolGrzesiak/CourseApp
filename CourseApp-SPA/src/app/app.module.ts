import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { UserService } from './_services/user.service';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MessagesComponent } from './messages/messages.component';
import { HomeComponent } from './home/home.component';
import { appRoutes } from './routes';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { HasRoleDirective } from './_directives/hasRole.directive';
import { RegisterComponent } from './home/register/register.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { AdminService } from './_services/admin.service';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { RolesModalComponent } from './admin/roles-modal/roles-modal.component';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { ExamsListComponent } from './exams/exams-list/exams-list.component';
import { ExamService } from './_services/exam.service';
import { ExamListResolver } from './_resolvers/exam-list.resolver';
import { ExamsPanelComponent } from './exams/exams-panel/exams-panel.component';
import { ExamsEnrolledComponent } from './exams/exams-enrolled/exams-enrolled.component';
import { SnakeComponent } from './games/snake/snake.component';
import { ScoreService } from './_services/score.service';
import { ExamsCreateComponent } from './exams/exams-create/exams-create.component';
import { ExamsDeleteModalComponent } from './exams/exams-delete-modal/exams-delete-modal.component';

import {
  BsDropdownModule,
  BsDatepickerModule,
  PaginationModule,
  TabsModule,
  ButtonsModule,
  ModalModule
} from 'ngx-bootstrap';
import { JwtModule } from '@auth0/angular-jwt';
import { AlertifyService } from './_services/alertify.service';
import { AuthGuard } from './_guards/auth.guard';
import { NgxGalleryModule } from 'ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import { TimeAgoPipe } from 'time-ago-pipe';
import { NgcFloatButtonModule } from 'ngc-float-button';

export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    MessagesComponent,
    HasRoleDirective,
    ExamsListComponent,
    HomeComponent,
    AdminPanelComponent,
    RolesModalComponent,
    UserManagementComponent,
    RegisterComponent,
    MemberListComponent,
    MemberCardComponent,
    MemberDetailComponent,
    MemberEditComponent,
    MemberMessagesComponent,
    PhotoEditorComponent,
    TimeAgoPipe,
    ExamsPanelComponent,
    ExamsListComponent,
    ExamsEnrolledComponent,
    SnakeComponent,
    ExamsCreateComponent,
    ExamsDeleteModalComponent
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
    PaginationModule.forRoot(),
    NgxGalleryModule,
    FileUploadModule,
    TabsModule.forRoot(),
    ButtonsModule,
    ModalModule.forRoot(),
    BsDropdownModule.forRoot(),
    ReactiveFormsModule,
    RouterModule.forRoot(appRoutes),
    NgcFloatButtonModule
  ],
  providers: [
    AlertifyService,
    AuthService,
    UserService,
    ErrorInterceptorProvider,
    AuthGuard,
    PreventUnsavedChanges,
    MemberListResolver,
    MemberEditResolver,
    MemberDetailResolver,
    MessagesResolver,
    AdminService,
    ExamService,
    ExamListResolver,
    ScoreService
  ],
  entryComponents: [RolesModalComponent, ExamsDeleteModalComponent],
  bootstrap: [AppComponent]
})
export class AppModule {}
