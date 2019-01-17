import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { Exam, ExamForCreation } from '../_models/exam';
import { map } from 'rxjs/operators';
import { Question } from '../_models/question';
import { Answer } from '../_models/answer';

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
  addUserToExam(userId: number, examId: number, password: string) {
    return this.http.post(this.baseUrl + 'exams/' + examId + '/enroll/' + userId, password);
  }
  getEnrolledExams() {
    return this.http.get<Exam[]>(this.baseUrl + 'exams/' + 'enrolled');
  }
  deleteExam(examId: number) {
    return this.http.delete(this.baseUrl + 'exams/' + examId);
  }
  createExam(exam: ExamForCreation) {
    return this.http.post(this.baseUrl + 'exams', exam);
  }

  createQuestion(question, examId: number) {
    return this.http.post(this.baseUrl + 'exams/' + examId + '/questions/', question);
  }
  createAnswer(answer, questionId: number) {
    return this.http.post(this.baseUrl + 'exams/' + 'questions/' + questionId + '/answers', answer);
  }
  createAnswers(answers: any, questionId: number) {
    return this.http.post(
      this.baseUrl + 'exams/' + 'questions/' + questionId + '/answers/create',
      answers
    );
  }

  getQuestionsWithAnswers(examId: number) {
    return this.http.get(this.baseUrl + 'exams/' + examId + '/questions');
  }
}
