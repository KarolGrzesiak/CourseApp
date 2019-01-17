import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  Output,
  EventEmitter,
  TemplateRef
} from '@angular/core';
import { ExamService } from 'src/app/_services/exam.service';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { Exam } from 'src/app/_models/exam';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { ExamsEnrolledComponent } from '../exams-enrolled/exams-enrolled.component';
import { HttpParams } from '@angular/common/http';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: 'app-exams-list',
  templateUrl: './exams-list.component.html',
  styleUrls: ['./exams-list.component.css']
})
export class ExamsListComponent implements OnInit {
  pagination: Pagination;
  exams: Exam[];
  passwordModalRef: BsModalRef;
  examToEnroll: Exam;
  examToEnrollPassword: string;
  @Output() enrolled = new EventEmitter();
  constructor(
    private examService: ExamService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private modalService: BsModalService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.exams = data['exams'].result;
      this.pagination = data['exams'].pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadExams();
  }

  loadExams() {
    this.examService.getExams(this.pagination.currentPage, this.pagination.pageSize).subscribe(
      (res: PaginatedResult<Exam[]>) => {
        this.exams = res.result;
        this.pagination = res.pagination;
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  showPasswordModal(examId: number, passwordTemplate: TemplateRef<any>) {
    this.examToEnroll = this.exams.find(e => e.id === examId);
    this.passwordModalRef = this.modalService.show(passwordTemplate);
  }
  enrollToExam() {
    const userId = this.authService.currentUser.id;
    const examId = this.examToEnroll.id;
    const examPassword = this.examToEnrollPassword;
    this.examToEnroll = null;
    this.examToEnrollPassword = null;
    this.examService.addUserToExam(userId, examId, examPassword).subscribe(
      () => {
        this.exams.splice(this.exams.findIndex(e => e.id === examId), 1);
        this.alertify.success('Successfully enrolled on test');
      },
      error => {
        this.alertify.error(error);
      }
    );
    this.passwordModalRef.hide();
  }
}
