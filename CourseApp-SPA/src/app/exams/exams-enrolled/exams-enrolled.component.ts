import { Component, OnInit } from '@angular/core';
import { Exam } from 'src/app/_models/exam';
import { ExamService } from 'src/app/_services/exam.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import * as moment from 'moment';

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
    private authService: AuthService
  ) { }


  ngOnInit() {
    this.loadEnrolledExams();
  }


  loadEnrolledExams() {
    const userId = this.authService.currentUser.id;
    this.examService.getEnrolledExams(userId).subscribe(
      response => {
        this.exams = response;
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
}
