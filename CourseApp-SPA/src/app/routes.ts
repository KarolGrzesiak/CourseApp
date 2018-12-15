import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ExamsComponent } from './exams/exams.component';
import { MessagesComponent } from './messages/messages.component';
import { AdminComponent } from './admin/admin.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberListComponent } from './members/member-list/member-list.component';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { MemberListResolver } from './_resolvers/member-list.resolver';

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
        path: 'exams',
        component: ExamsComponent
      },
      {
        path: 'messages',
        component: MessagesComponent
      },
      {
        path: 'admin',
        component: AdminComponent,
        data: { roles: ['Admin'] }
      }
    ]
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];
