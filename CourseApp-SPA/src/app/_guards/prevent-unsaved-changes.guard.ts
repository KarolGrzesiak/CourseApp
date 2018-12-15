import { Injectable } from '@angular/core';
import {
  CanDeactivate,
  ActivatedRoute,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { Observable } from 'rxjs';

export interface CanComponentDeactivate {
  canDeactivate: () => boolean | Observable<boolean> | Promise<boolean>;
}

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<any> {
  canDeactivate(
    component: CanComponentDeactivate,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState: RouterStateSnapshot
  ): boolean | Observable<boolean> | Promise<boolean> {

    return component.canDeactivate();
  }
}
