import { Component, OnInit } from '@angular/core';
import { Exam } from 'src/app/_models/exam';
import { ExamService } from 'src/app/_services/exam.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import * as moment from 'moment';
import { Router } from '@angular/router';

@Component({
  selector: 'app-exams-enrolled',
  templateUrl: './exams-enrolled.component.html',
  styleUrls: ['./exams-enrolled.component.css']
})
export class ExamsEnrolledComponent implements OnInit {
  exams: Exam[];
  durationHelper = moment;
  constructor(
    private examService: ExamService,
    private alertify: AlertifyService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadEnrolledExams();
  }

  loadEnrolledExams() {
    this.examService.getEnrolledExams().subscribe(
      response => {
        this.exams = response;
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
  startExam(examId: number, examDuration) {
    localStorage.setItem('minutes', examDuration);
    this.router.navigate(['/exams/' + examId + '/take']);
  }
}
