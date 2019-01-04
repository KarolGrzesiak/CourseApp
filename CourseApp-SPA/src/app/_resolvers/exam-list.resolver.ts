import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Exam } from '../_models/exam';
import { ExamService } from '../_services/exam.service';

@Injectable()
export class ExamListResolver implements Resolve<Exam[]> {
  pageNumber = 1;
  pageSize = 10;
  constructor(
    private examService: ExamService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Exam[]> {
    return this.examService.getExams(this.pageNumber, this.pageSize).pipe(
      catchError(error => {
        this.alertify.error('Problem retrieving data');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
