import { Component, OnInit, HostListener, OnDestroy } from '@angular/core';
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
  finishFlag = false;
  interval: Observable<number>;
  subscription: Subscription;
  counter: number;

  @HostListener('window:beforeunload', ['$event'])
  beforeUnloadHander(event) {
    event.returnValue = false;
  }

  canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
    if (this.finishFlag === false) {
      const result = confirm('Do you want to finish?');
      if (result === true) {
        this.finish();
      }
      return result;
    }
    return true;
  }

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
    this.counter = +localStorage.getItem('counter');
    this.interval = interval(60000);
    this.subscription = this.interval.subscribe(() => {
      if (this.time <= 0) {
        return;
      }
      this.counter++;
      localStorage.setItem('counter', this.counter.toString());
      if (this.counter === this.time) {
        this.alertify.error('Times up');
        this.finish();
      }
    });
  }

  finish() {
    this.finishFlag = true;
    this.subscription.unsubscribe();
    localStorage.removeItem('minutes');
    localStorage.removeItem('counter');
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
        }
      );
    this.router.navigate(['/exams']);
  }
}
