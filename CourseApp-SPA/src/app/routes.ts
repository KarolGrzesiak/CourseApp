import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberListComponent } from './members/member-list/member-list.component';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { ExamListResolver } from './_resolvers/exam-list.resolver';
import { ExamsPanelComponent } from './exams/exams-panel/exams-panel.component';
import { SnakeComponent } from './games/snake/snake.component';
import { ExamsCreateComponent } from './exams/exams-create/exams-create.component';
import { RockPaperScissorsComponent } from './games/rock-paper-scissors/rock-paper-scissors.component';

export const appRoutes: Routes = [
  {
    path: '',

    component: HomeComponent
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'members',
        component: MemberListComponent,
        resolve: { users: MemberListResolver }
      },
      {
        path: 'members/:id',
        component: MemberDetailComponent,
        resolve: { user: MemberDetailResolver }
      },
      {
        path: 'member/edit',
        component: MemberEditComponent,
        resolve: { user: MemberEditResolver },
        canDeactivate: [PreventUnsavedChanges]
      },
      {
        path: 'exams',
        component: ExamsPanelComponent,
        resolve: { exams: ExamListResolver }
      },
      {
        path: 'exams/create',
        component: ExamsCreateComponent,
        canDeactivate: [PreventUnsavedChanges]
      },

      { path: 'messages', component: MessagesComponent, resolve: { messages: MessagesResolver } },
      { path: 'games/snake', component: SnakeComponent },
      { path: 'games/rockpaperscissors', component: RockPaperScissorsComponent },

      {
        path: 'admin',
        component: AdminPanelComponent,
        data: { roles: ['Admin'] }
      }
    ]
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];
