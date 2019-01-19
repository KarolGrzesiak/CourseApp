import { Component, OnInit, TemplateRef } from '@angular/core';
import { ExamService } from 'src/app/_services/exam.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Exam } from 'src/app/_models/exam';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';

@Component({
  selector: 'app-exams-owned',
  templateUrl: './exams-owned.component.html',
  styleUrls: ['./exams-owned.component.css']
})
export class ExamsOwnedComponent implements OnInit {
  exams: Exam[];
  modalRef: BsModalRef;
  stats: { userName; numberOfWrongAnswers; numberOfCorrectAnswers }[];
  constructor(
    private modalService: BsModalService,
    private examService: ExamService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.loadCreatedExams();
  }

  loadCreatedExams() {
    this.examService.getCreatedExams().subscribe(
      response => {
        this.exams = response;
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
  openModal(template: TemplateRef<any>, examId) {
    this.examService.getStatsForTeacher(examId).subscribe(
      response => {
        this.stats = response;
        console.log(this.stats);
      },
      error => {
        this.alertify.error(error);
      }
    );

    this.modalRef = this.modalService.show(template);
  }
}
