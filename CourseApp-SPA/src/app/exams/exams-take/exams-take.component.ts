import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ExamService } from 'src/app/_services/exam.service';
import { UserService } from 'src/app/_services/user.service';
import { Question } from 'src/app/_models/question';
import { AuthService } from 'src/app/_services/auth.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-exams-take',
  templateUrl: './exams-take.component.html',
  styleUrls: ['./exams-take.component.css']
})
export class ExamsTakeComponent implements OnInit {
  questionsWithAnswers: Question[];
  userAnswers: {};
  constructor(
    private alertify: AlertifyService,
    private examService: ExamService,
    private authService: AuthService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.questionsWithAnswers = data['questionsWithAnswers'];
    });
    console.log(this.questionsWithAnswers);
  }
}
