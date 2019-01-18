import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ExamService } from 'src/app/_services/exam.service';
import { UserService } from 'src/app/_services/user.service';
import { Question } from 'src/app/_models/question';
import { AuthService } from 'src/app/_services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { interval, Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-exams-take',
  templateUrl: './exams-take.component.html',
  styleUrls: ['./exams-take.component.css']
})
export class ExamsTakeComponent implements OnInit {
  questionsWithAnswers: Question[];
  userAnswers = [''];
  time: number;

  interval: Observable<number>;
  counter: number;
  constructor(
    private alertify: AlertifyService,
    private examService: ExamService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.questionsWithAnswers = data['questionsWithAnswers'];
    });
    this.userAnswers = Array(this.questionsWithAnswers.length).fill('');
    this.time = +localStorage.getItem('minutes');
    this.counter = 0;
    this.interval = interval(60000);
    this.interval.subscribe(() => {
      if (this.time <= 0) {
        return;
      }
      this.counter++;
      if (this.counter === this.time) {
        this.alertify.error('Times up');
        this.finish();
      }
    });
  }

  finish() {
    const answers = [];
    for (let i = 0; i < this.questionsWithAnswers.length; i++) {
      answers.push({
        content: this.userAnswers[i],
        questionId: this.questionsWithAnswers[i].id
      });
    }

    this.examService
      .createUserAnswers(answers, this.route.snapshot.paramMap.get('examId'))
      .subscribe(
        () => {
          this.alertify.success('Your answers were saved');
        },
        error => {
          this.alertify.error(error);
        },
        () => {
          this.router.navigate(['/exams']);
        }
      );
  }
}
