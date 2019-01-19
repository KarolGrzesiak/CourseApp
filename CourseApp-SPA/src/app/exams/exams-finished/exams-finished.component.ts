import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ExamService } from 'src/app/_services/exam.service';
import { Exam } from 'src/app/_models/exam';

@Component({
  selector: 'app-exams-finished',
  templateUrl: './exams-finished.component.html',
  styleUrls: ['./exams-finished.component.css']
})
export class ExamsFinishedComponent implements OnInit {
  exams: Exam[];
  stats: any;
  constructor(private alertify: AlertifyService, private examService: ExamService) {}

  ngOnInit() {
    this.loadFinishedExams();
  }

  loadFinishedExams() {
    this.examService.getFinishedExams().subscribe(
      response => {
        this.exams = response;
        this.stats = Array(this.exams.length).fill({
          numberOfWrongAnswers: 0,
          numberOfCorrectAnswers: 0
        });
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
  showStats(event: boolean, examId, place: number) {
    if (
      event === false ||
      this.stats[place].numberOfWrongAnswers !== 0 ||
      this.stats[place].numberOfCorrectAnswers !== 0
    ) {
      return;
    }

    this.examService
      .getStatsForUser(examId)
      .subscribe(({ numberOfWrongAnswers, numberOfCorrectAnswers }) => {
        this.stats[place] = {
          numberOfWrongAnswers: numberOfWrongAnswers,
          numberOfCorrectAnswers: numberOfCorrectAnswers
        };
      });
  }
}
