import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersComponent } from './members/members.component';
import { ExamsComponent } from './exams/exams.component';
import { MessagesComponent } from './messages/messages.component';

export const appRoutes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: '',
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
      }
    ]
  },
  { path: '*', redirectTo: '', pathMatch: 'full' }
];
