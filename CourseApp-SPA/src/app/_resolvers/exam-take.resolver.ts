import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ExamService } from '../_services/exam.service';
import { Question } from '../_models/question';

@Injectable()
export class ExamTakeResolver implements Resolve<Question[]> {
  constructor(
    private examService: ExamService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Question[]> {
    return this.examService.getQuestionsWithAnswers(+route.paramMap.get('examId')).pipe(
      catchError(error => {
        this.alertify.error('Problem retrieving data');
        this.router.navigate(['/exams']);
        return of(null);
      })
    );
  }
}
