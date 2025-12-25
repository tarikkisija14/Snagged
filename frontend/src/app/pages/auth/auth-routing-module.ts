import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {Login} from './login/login';
import {Register} from './register/register';
import {AuthWrapper} from './auth-wrapper/auth-wrapper';

const routes: Routes = [
  {
    path: '',
    component: AuthWrapper,
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: Login },
      { path: 'register', component: Register },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
