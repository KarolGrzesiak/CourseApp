import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersComponent } from './members/members.component';
import { ExamsComponent } from './exams/exams.component';
import { MessagesComponent } from './messages/messages.component';
import { AdminComponent } from './admin/admin.component';
import { AuthGuard } from './_guards/auth.guard';

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
        component: MembersComponent
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
