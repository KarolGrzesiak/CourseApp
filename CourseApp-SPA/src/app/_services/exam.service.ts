import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { Exam } from '../_models/exam';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ExamService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getExams(pageNumber?, pageSize?): Observable<PaginatedResult<Exam[]>> {
    const paginatedResult = new PaginatedResult<Exam[]>();
    let params = new HttpParams();
    if (pageNumber != null && pageSize != null) {
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', pageSize);
    }

    return this.http.get<Exam[]>(this.baseUrl + 'exams', { observe: 'response', params }).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }
  addUserToExam(userId: number, examId: number) {
    return this.http.post(this.baseUrl + 'exams/' + examId + '/enroll/' + userId, {});
  }
  getEnrolledExams(userId: number) {
    return this.http.get<Exam[]>(this.baseUrl + 'exams/' + 'enrolled/' + userId);
  }
  deleteExam(examId: number) {
    return this.http.delete(this.baseUrl + 'exams/' + examId);
  }
}
